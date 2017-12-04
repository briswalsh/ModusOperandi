using System.Diagnostics;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamRadio : MonoBehaviour {

	private SteamVR_TrackedObject Controller;
	const string MUTE_EXE_PATH = "Assets\\Plugins\\MuteApp.exe";
	const string UNMUTE_EXE_PATH = "Assets\\Plugins\\UnmuteApp.exe";

	// Use this for initialization
	void Start () {
		Controller = GetComponent<SteamVR_TrackedObject> ();
	}
	
	// Update is called once per frame
	void Update () {
		var ipt = SteamVR_Controller.Input ((int)Controller.index);

		if (ipt.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
		{
			Mute();
		}
		if (ipt.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
		{
			Unmute();
		}		
		
	}

	void Mute()
	{
		SetMute(MUTE_EXE_PATH);
	}

	void Unmute()
	{
		SetMute(UNMUTE_EXE_PATH);
	}
				
	void SetMute(string path)
	{
		Process p = new Process();
		p.StartInfo.FileName = path;
		p.StartInfo.RedirectStandardOutput = true;
		p.StartInfo.UseShellExecute = false;
		p.StartInfo.CreateNoWindow = true;
		p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
		p.Start();
	}
}
