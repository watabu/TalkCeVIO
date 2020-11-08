using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class CeVIOControl : MonoBehaviour {
    
    private Process m_Process;
	
    // Use this for initialization
	private void Start () {
        m_Process = null;
	}

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (null == m_Process)
            {
                //changeCast 名前　で変更
                m_Process = new Process();
                m_Process.StartInfo.FileName = Application.dataPath + "/External/CeVIOCtrl_forNewPC.exe";
                m_Process.StartInfo.Arguments = "changecast";
                m_Process.StartInfo.Arguments = "こんにちわ さとうささらです。 はじめまして！";
                // for Redirect
                {
                    m_Process.StartInfo.CreateNoWindow = true;
                    m_Process.StartInfo.UseShellExecute = false;
                    m_Process.StartInfo.RedirectStandardOutput = true;
                }
                // for ExitEvent
                {
                    m_Process.EnableRaisingEvents = true;
                    m_Process.Exited += Process_Exited;
                }
                UnityEngine.Debug.Log("startTalking");
                m_Process.Start();
                UnityEngine.Debug.Log("endTalking");

                // Redirect
                string output = m_Process.StandardOutput.ReadToEnd();
                UnityEngine.Debug.Log(output);
            }
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (null != m_Process)
            {
                if (!m_Process.HasExited)
                {
                    m_Process.CloseMainWindow();
                    m_Process.Dispose();
                    m_Process = null;
                }
            }
        }
    }

    private void Process_Exited(object sender, System.EventArgs e)
    {
        UnityEngine.Debug.Log("Process_Exited");
        m_Process.Dispose();
        m_Process = null;
    }
}
