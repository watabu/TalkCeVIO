using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class CeVIOControl : MonoBehaviour {
    
    private Process _process;
	
    // Use this for initialization
	void Start () {
        _process = null;
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (null == _process)
            {
                //changeCast 名前　で変更
                _process = new Process();
                _process.StartInfo.FileName = Application.dataPath + "/External/CeVIOVoice.exe";
                //_process.StartInfo.Arguments = "changecast";
                _process.StartInfo.Arguments = "こんにちわ さとうささらです。 はじめまして！";
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
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (null != _process)
            {
                if (!_process.HasExited)
                {
                    _process.CloseMainWindow();
                    _process.Dispose();
                    _process = null;
                }
            }
        }
    }

    void Process_Exited(object sender, System.EventArgs e)
    {
        UnityEngine.Debug.Log("Process_Exited");
        _process.Dispose();
        _process = null;
    }
}
