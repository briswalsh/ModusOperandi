using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public GameObject AudioManager;

	const string SPEECH_EXE_PATH = "Assets\\Plugins\\SpeechRecognitionEngine.exe";
	const string MUTE_EXE_PATH = "Assets\\Plugins\\MuteApp.exe";
	const string UNMUTE_EXE_PATH = "Assets\\Plugins\\UnmuteApp.exe";
	const string SPEECH_PATH = "speech.txt";
	string[] empty = { };
	Process speechProcess;

	public SteamVR_TrackedObject Controller;

	void Start()
	{
		File.WriteAllLines(SPEECH_PATH, empty);
		speechProcess = Process.Start(SPEECH_EXE_PATH);
		StartCoroutine("Listen");
		Mute();
	}

	IEnumerator Listen()
	{
		while (true)
		{
			if (File.Exists(SPEECH_PATH))
			{
				string[] speech = File.ReadAllLines(SPEECH_PATH);
				if (!IsEmpty(speech))
				{
					AudioManager.GetComponent<SpeechProcessor>().Process(speech[0]);
					File.WriteAllLines(SPEECH_PATH, empty);
				}
			}
			yield return new WaitForSeconds(0.1f);
		}
	}

	private static bool IsEmpty(string[] text)
	{
		if (text.Length == 0) return true;
		foreach (string line in text)
		{
			if (line != string.Empty) return false;
		}
		return true;
	}

	void Update()
	{
		var ipt = SteamVR_Controller.Input((int)Controller.index);
		if (ipt.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) || Input.GetKeyDown(KeyCode.T))
		{
			UnityEngine.Debug.Log ("unmute");
			Unmute();
		}
	
		if (ipt.GetPressUp(SteamVR_Controller.ButtonMask.Trigger) || Input.GetKeyUp(KeyCode.T))
		{
			UnityEngine.Debug.Log ("mute");
			Mute();
		}
	}

	void OnApplicationQuit()
	{
		speechProcess.Kill();
		Unmute();
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
