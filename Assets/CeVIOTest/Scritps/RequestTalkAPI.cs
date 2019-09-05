using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using MiniJSON;
using System.Diagnostics;

public class RequestTalkAPI : MonoBehaviour
{

    string url = "https://api.a3rt.recruit-tech.co.jp/talk/v1/smalltalk";
    string apikey = "DZZ0ailgYJUxo9GZRokQXY6BKOsxGTdU";
    public string query = "";
    public Text queryText;
    public Text replyText;

    public InputField inputField;

    private Process _process;
    //  IEnumerator Start()


    void Start()
    {
        _process = null;
    }

    public void Chatting()
    {
        StartCoroutine(ChatBot());
    }

    public void resetText()
    {
        inputField.text = "";
    }

    public IEnumerator ChatBot()
    {
        // ChatAPIに送る情報を入力
        WWWForm form = new WWWForm();
        query = queryText.text;
        UnityEngine.Debug.Log("send text:" +queryText.text);
   //     query = "おなかがすきました";
        form.AddField("apikey", apikey);
        form.AddField("query", query, Encoding.UTF8);

        // 通信
        using (UnityWebRequest request = UnityWebRequest.Post(url, form))
        {

            yield return request.Send();

            if (request.isError)
            {
                UnityEngine.Debug.Log(request.error);
            }
            else
            {
                try
                {
                    // 取得したものをJsonで整形
                    string itemJson = request.downloadHandler.text;
                    UnityEngine.Debug.Log(itemJson);
                    JsonNode jsnode = JsonNode.Parse(itemJson);
                    // Jsonから会話部分だけ抽出してTextに代入               
                    if (replyText.text != null)
                    {
                        replyText.text = jsnode["results"][0]["reply"].Get<string>();

                        TalkCeVIO(jsnode["results"][0]["reply"].Get<string>());
                    }
                    else
                    {
                        TalkCeVIO("ごめんなさい、よく聞こえませんでした");
                    }

                    UnityEngine.Debug.Log("reply text: " +replyText.text);

                }
                catch (Exception e)
                {
                    // エラーが出たらこれがログに吐き出される
                    UnityEngine.Debug.Log("JsonNode:" + e.Message);
                    TalkCeVIO("ごめんなさい、よく聞こえませんでした");
                }
             
            }
        }
    }

    int ChangeCast(string name)
    {


        return 0;
    }
    void TalkCeVIO(string text)
    {
        if (null == _process)
        {
            //changeCast 名前　で変更
            _process = new Process();
            _process.StartInfo.FileName = Application.dataPath + "/External/CeVIO1.exe";
            //_process.StartInfo.Arguments = "changecast";
            _process.StartInfo.Arguments = text;
            // for Redirect
            {
                _process.StartInfo.CreateNoWindow = true;
                _process.StartInfo.UseShellExecute = false;
                _process.StartInfo.RedirectStandardOutput = true;
            }
            // for ExitEvent
            { 
                _process.EnableRaisingEvents = true;
                _process.Exited += Process_Exited;
            }
            _process.Start();
            // Redirect
            string output = _process.StandardOutput.ReadToEnd();
            UnityEngine.Debug.Log(output);
        }
    }
    void Process_Exited(object sender, System.EventArgs e)
    {
        UnityEngine.Debug.Log("Process_Exited");
        _process.Dispose();
        _process = null;
    }
}

