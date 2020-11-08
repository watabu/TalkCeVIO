//参考文献
//https://qiita.com/koukiwf/items/b959fed12f9267b61c6e
//https://simplestar-tech.hatenablog.com/entry/2016/01/17/111218
//https://qiita.com/TakaoIto/items/7989ecb2ea862bb77e64
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using MiniJSON;
using System.Diagnostics;

/// <summary>
/// 返答文の生成、合成音声出力を行う
/// </summary>
public class RequestTalkAPI : MonoBehaviour
{
    private const string m_APIURL = "https://api.a3rt.recruit-tech.co.jp/talk/v1/smalltalk";
    private const string m_APIKEY = "DZZ0ailgYJUxo9GZRokQXY6BKOsxGTdU";
    [SerializeField]
    private string m_Query = "";
    [SerializeField]
    private Text m_QueryText;
    [SerializeField]
    private Text m_ReplyText;
    [SerializeField]
    private Sasara m_Sasara;
    [SerializeField]
    private InputField m_InputField;

    private Process m_Process;

    private void Start()
    {
        m_Process = null;
    }

    public void StartChat()
    {
        StartCoroutine(GetReplyText());
    }

    public void ResetText()
    {
        m_InputField.text = "";
    }

    public IEnumerator GetReplyText()
    {
        // ChatAPIに送る情報を入力
        WWWForm form = new WWWForm();
        m_Query = m_QueryText.text;
        UnityEngine.Debug.Log("send text:" + m_QueryText.text);
        //     query = "おなかがすきました";
        form.AddField("apikey", m_APIKEY);
        form.AddField("query", m_Query, Encoding.UTF8);

        // 通信
        using (UnityWebRequest request = UnityWebRequest.Post(m_APIURL, form))
        {
            //UnityEngine.Debug.Log("send");
            yield return request.Send();

            if (request.isNetworkError)
            {
                UnityEngine.Debug.Log(request.error);
            }
            else
            {
                m_Sasara.ChangeSasara();
                try
                {
                    // 取得したものをJsonで整形
                    string itemJson = request.downloadHandler.text;
                    UnityEngine.Debug.Log(itemJson);
                    JsonNode jsnode = JsonNode.Parse(itemJson);
                    // Jsonから会話部分だけ抽出してTextに代入               
                    if (m_ReplyText.text != null)
                    {
                        m_ReplyText.text = jsnode["results"][0]["reply"].Get<string>();

                        //StartCoroutine("TalkCeVIO", m_ReplyText.text);
                        StartCoroutine(TalkCeVIO(m_ReplyText.text));
                    }
                    else
                    {
                        //StartCoroutine("TalkCeVIO", "ごめんなさい、よく聞こえませんでした");
                        StartCoroutine(TalkCeVIO("ごめんなさい、よく聞こえませんでした"));
                    }

                    UnityEngine.Debug.Log("reply text: " + m_ReplyText.text);

                }
                catch (Exception e)
                {
                    // エラーが出たらこれがログに吐き出される
                    UnityEngine.Debug.Log("JsonNode:" + e.Message);
                    StartCoroutine(TalkCeVIO("ごめんなさい、よく聞こえませんでした"));
                }
            }

        }
    }



    //以下合成音声出力について----------------------------------
   
    private int ChangeCast(string name)
    {


        return 0;
    }

    private IEnumerator TalkCeVIO(string text)
    {
        // UnityEngine.Debug.Log(text);
        yield return new WaitForSeconds(0.125f);
       // UnityEngine.Debug.Log(text);
        if (null == m_Process)
        {
            //changeCast 名前　で変更
            m_Process = new Process();
            m_Process.StartInfo.FileName = Application.dataPath + "/External/CeVIOCtrl_forNewPC.exe";
            //_process.StartInfo.Arguments = "changecast";
            m_Process.StartInfo.Arguments = text;
            // for Redirect
            {
                m_Process.StartInfo.CreateNoWindow = true;
                m_Process.StartInfo.UseShellExecute = false;
                m_Process.StartInfo.RedirectStandardOutput = true;
            }
            // for ExitEvent
            { 
                m_Process.EnableRaisingEvents = true;
                m_Process.Exited += ProcessExited;
            }
            m_Process.Start();
            // Redirect
            string output = m_Process.StandardOutput.ReadToEnd();
            UnityEngine.Debug.Log(output);
        }
    }

    private void ProcessExited(object sender, System.EventArgs e)
    {
        //UnityEngine.Debug.Log("Process_Exited");
        m_Process.Dispose();
        m_Process = null;
    }
}

