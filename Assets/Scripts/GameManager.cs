using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public GameObject AudioManager;

	const string EXE_PATH = "Assets\\SpeechRecognitionEngine.exe";
	const string SPEECH_PATH = "speech.txt";
	string[] empty = { };
	Process speechProcess;

	void Start ()
	{
		speechProcess = Process.Start(EXE_PATH);
		StartCoroutine("Listen");
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
                    AudioManager.GetComponent<SpeechProcessor>().Process(speech[0]); //more in other lines?
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

	void Update ()
	{
		
    }

	void OnApplicationQuit()
	{
		speechProcess.Kill();
	}
}
