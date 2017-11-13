using System;
using System.Diagnostics;
using System.Speech.Recognition;
using UnityEngine;

public class VoiceRecognizer {

	// TODO:
	// make it not crash

	SpeechRecognizer recognizer;
	
	public VoiceRecognizer() {
		recognizer = new SpeechRecognizer();
		
		// Create a simple grammar that recognizes "red", "green", or "blue".
		Choices colors = new Choices();
		colors.Add(new string[] { "red", "green", "blue" });
		
		// Create a GrammarBuilder object and append the Choices object.
		GrammarBuilder gb = new GrammarBuilder();
		gb.Append(colors);
		
		Grammar g = new Grammar(gb);
		
		recognizer.LoadGrammarAsync(g);
		//recognizer.LoadGrammarAsync(new DictationGrammar()); // THIS LINE CRASHES UNITY :( put it in a separate thread?

		recognizer.SpeechRecognized +=
		  new EventHandler<SpeechRecognizedEventArgs>(sre_SpeechRecognized);
	}

	void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
	{
		SpeechProcessor.Process(e.Result.Text);
	}

	public void StartListening()
	{
		recognizer.Enabled = true;
	}

	public void StopListening()
	{
		recognizer.Enabled = false;
	}
}
