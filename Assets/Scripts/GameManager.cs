using UnityEngine;

public class GameManager : MonoBehaviour {

	VoiceRecognizer recognizer;
	
	void Start ()
	{
		recognizer = new VoiceRecognizer();
	}
	
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			recognizer.StartListening();
		}
		if (Input.GetKeyUp(KeyCode.Space))
		{
			recognizer.StopListening();
		}
    }
}
