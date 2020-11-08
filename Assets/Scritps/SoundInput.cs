using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;   //Windowsの音声認識で使用
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// Windows10の機能を用いて音声入力を行う
/// </summary>
public class SoundInput : MonoBehaviour
{
    [SerializeField]
    private UnityEvent m_OnSpeaked;
    [SerializeField]
    private InputField m_QueryText;
    [SerializeField]
    private Button m_StartButton;
    [SerializeField]
    private Button m_EndButton;

    private DictationRecognizer dictationRecognizer;

    private void Start()
    {
        dictationRecognizer = new DictationRecognizer();
        dictationRecognizer.InitialSilenceTimeoutSeconds = 20;
        dictationRecognizer.AutoSilenceTimeoutSeconds = 90;

        //DictationRecognizer_DictationResult処理を行う
        dictationRecognizer.DictationResult += DictationRecognizer_DictationResult;
        //DictationRecognizer_DictationHypothesis処理を行う
        dictationRecognizer.DictationHypothesis += DictationRecognizer_DictationHypothesis;
        //DictationRecognizer_DictationComplete処理を行う
        dictationRecognizer.DictationComplete += DictationRecognizer_DictationComplete;
        //DictationRecognizer_DictationError処理を行う
        dictationRecognizer.DictationError += DictationRecognizer_DictationError;

        StartDictation();
    }

    public void StartDictation()
    {
        //ディクテーションを開始
        dictationRecognizer.Start();
        Debug.Log("音声認識開始");
        var colors = m_StartButton.GetComponent<Button>().colors;
        colors.normalColor = new Color(180, 180, 180);
        m_StartButton.GetComponent<Button>().colors = colors;
        colors = m_EndButton.GetComponent<Button>().colors;
        colors.normalColor = new Color(255, 255, 255);
        m_EndButton.GetComponent<Button>().colors = colors;
    }
    //DictationResult：音声が特定の認識精度で認識されたときに発生するイベント
    private void DictationRecognizer_DictationResult(string text, ConfidenceLevel confidence)
    {
        Debug.Log("認識した音声：" + text);
        if (m_QueryText != null)
            m_QueryText.text = text;
        else
        {
            m_QueryText.text = "こんにちは！";
        }
        m_OnSpeaked.Invoke();
    }

    //DictationHypothesis：音声入力中に発生するイベント
    private void DictationRecognizer_DictationHypothesis(string text)
    {
        // Debug.Log("音声認識中：" + text);
    }

    //DictationComplete：音声認識セッションを終了したときにトリガされるイベント
    private void DictationRecognizer_DictationComplete(DictationCompletionCause cause)
    {
        Debug.Log("音声認識完了");
        //    DictationStart();
    }

    //DictationError：音声認識セッションにエラーが発生したときにトリガされるイベント
    private void DictationRecognizer_DictationError(string error, int hresult)
    {
        Debug.Log("音声認識エラー");
        //   DictationStart();
    }

    public void StopDictation()
    {
        dictationRecognizer.Stop();
        var colors = m_StartButton.GetComponent<Button>().colors;
        colors.normalColor = new Color(255, 255, 255);
        m_StartButton.GetComponent<Button>().colors = colors;
        colors = m_EndButton.GetComponent<Button>().colors;
        colors.normalColor = new Color(180, 180, 180);
        m_EndButton.GetComponent<Button>().colors = colors;

    }
    public void Fin()
    {
        dictationRecognizer.Dispose();
    }

}