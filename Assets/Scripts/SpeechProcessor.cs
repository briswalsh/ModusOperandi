using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class SpeechProcessor : MonoBehaviour {

	enum State
	{
		DEFAULT,
		QUESTION_SPECIFIC,
		CLARIFY
	}

	private Dictionary<string, AudioClip> audioDictionary; 
	private AudioSource audioSrc;
	Dictionary<string, string> responseMap;

	void Awake()
	{
		audioDictionary = new Dictionary<string, AudioClip> ();
		audioSrc = GetComponent<AudioSource>();
		audioSrc.enabled = true;

		AudioClip errorClip = Resources.Load("didnt_catch_that", typeof(AudioClip)) as AudioClip;
		audioDictionary.Add("error", errorClip);

		responseMap = new Dictionary<string, string>();

		LoadAudio("who", "alex_russell_info");
		LoadAudio("victim", "alex_russell_info");
		LoadAudio("weapon", "killed_with_trophy_blunt_force_trauma");
		LoadAudio("picture", "lawrenceville_community_center_pool_locker_room");
		LoadAudio("bag", "nothing_in_the_bag");
		LoadAudio("northriver", "northriver_prep_is_a_high_school");
	}

	private void LoadAudio(string word, string clipName)
	{
		AudioClip audioClip;
		if (audioDictionary.ContainsKey(clipName))
		{
			audioClip = audioDictionary[clipName];
		}
		else
		{
			audioClip = Resources.Load(clipName, typeof(AudioClip)) as AudioClip;
        }
		audioDictionary.Add(clipName, audioClip);
		responseMap.Add(word, clipName);
	}

	public void Process(string text)
	{
		Debug.Log("Speech recognized: " + text);
		
		foreach (string word in responseMap.Keys)
		{
			if (text.ToLower().Contains(word))
			{
				audioSrc.PlayOneShot(audioDictionary[responseMap[word]]);
				return;
			}
		}
		audioSrc.PlayOneShot(audioDictionary["error"]);
    }
}
