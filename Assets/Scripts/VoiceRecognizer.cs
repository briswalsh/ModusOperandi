using System;
using System.Speech.Recognition;
using UnityEngine;

public class VoiceRecognizer : MonoBehaviour {

	// TODO:
	// enable/disable windows speech recognition through button press
	// easier way of doing function to process speech

	void Start () {
		SpeechRecognizer recognizer = new SpeechRecognizer();

		// Create a simple grammar that recognizes "red", "green", or "blue".
		Choices colors = new Choices();
		colors.Add(new string[] { "red", "green", "blue" });

		// Create a GrammarBuilder object and append the Choices object.
		GrammarBuilder gb = new GrammarBuilder();
		gb.Append(colors);

		Grammar g = new Grammar(gb);
		//recognizer.LoadGrammar(g);
		recognizer.LoadGrammar(new DictationGrammar());
		
		recognizer.SpeechRecognized +=
		  new EventHandler<SpeechRecognizedEventArgs>(sre_SpeechRecognized);
	}

	void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
	{
		print("Speech recognized: " + e.Result.Text);
		// process e.Result.Text.ToLower()
	}
}
