using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(AudioSource))]
public class SpeechProcessor : MonoBehaviour {

	enum State
	{
		PICK_UP,
		SAY_YES,
		CONFIRM_BOARD,
		REMIND_NAME
	}

	private Dictionary<string, AudioClip> audioDictionary; 
	private AudioSource audioSrc;
	Dictionary<string, Action> responseMap;
	State state;

	void Awake()
	{
		audioDictionary = new Dictionary<string, AudioClip> ();
		audioSrc = GetComponent<AudioSource>();
		audioSrc.enabled = true;

		AudioClip errorClip = Resources.Load("didnt_catch_that", typeof(AudioClip)) as AudioClip;
		audioDictionary.Add("error", errorClip);

		responseMap = new Dictionary<string, Action>();

		LoadAudio("say_yes");
		LoadAudio("alex_russell_info");
		LoadAudio("killed_with_trophy_blunt_force_trauma");
		LoadAudio("lawrenceville_community_center_pool_locker_room");
		LoadAudio("nothing_in_the_bag");
		LoadAudio("northriver_prep_is_a_high_school");

		MapResponse("yes", Confirm);
		MapResponse("yeah", Confirm);
		
		LoadAudio("who", "alex_russell_info");
		LoadAudio("victim", "alex_russell_info");
		LoadAudio("weapon", "killed_with_trophy_blunt_force_trauma");
		LoadAudio("picture", "lawrenceville_community_center_pool_locker_room");
		LoadAudio("bag", "nothing_in_the_bag");
		LoadAudio("northriver", "northriver_prep_is_a_high_school");

		state = State.PICK_UP;
	}

	private void LoadAudio(string clipName)
	{
		AudioClip audioClip = Resources.Load(clipName, typeof(AudioClip)) as AudioClip;
		audioDictionary.Add(clipName, audioClip);
	}

	private void MapResponse(string word, Action response)
	{
		responseMap.Add(word, response);
	}

	private void MapResponseSimple(string word, string responseName)
	{
		responseMap.Add(word, () => PlayAudio(responseName));
	}

	private void PlayAudio(string clipName)
	{
		audioSrc.PlayOneShot(audioDictionary[clipName]);
	}

	public void Process(string text)
	{
		Debug.Log("Speech recognized: " + text);
		
		//special case- accept anything
		if (state == State.PICK_UP)
		{
			PickUp();
			return;
		}

		foreach (string word in responseMap.Keys)
		{
			if (text.ToLower().Contains(word))
			{
				responseMap[word]();
				return;
			}
		}
		if (state == State.REMIND_NAME)
		{
			PlayAudio("confirmboard_unrelated_1"); //TODO: switch between 1 and 2
        }
		else
		{
			PlayAudio("error");
		}
	}

	private void PickUp()
	{
		state = State.SAY_YES;
		PlayAudio("pick_up");
	}

	private void Confirm()
	{
		if (state == State.SAY_YES)
		{
			state = State.CONFIRM_BOARD;
			PlayAudio("say_yes");
		}
		else if (state == State.CONFIRM_BOARD)
		{
			state = State.REMIND_NAME;
			PlayAudio("confirmboard");
		}
    }
}
