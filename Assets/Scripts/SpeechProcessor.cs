using UnityEngine;

public class SpeechProcessor {

	public static void Process(string text)
	{
		Debug.Log("Speech recognized: " + text);

		if (text.ToLower().Contains("ball"))
		{
			Debug.Log("You said ball");
		}
	}

}
