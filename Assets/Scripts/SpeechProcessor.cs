using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class SpeechProcessor : MonoBehaviour {

	private Dictionary<string, AudioClip> audioDictionary; 
	private AudioSource audio;

	void Awake(){
		audioDictionary = new Dictionary<string, AudioClip> ();
		audio = GetComponent<AudioSource>();
		audio.enabled = true;

		AudioClip errorClip = Resources.Load ("didnt_catch_that", typeof(AudioClip)) as AudioClip;

		audioDictionary.Add("error", errorClip);

		audioDictionary.Add("who", Resources.Load("alex_russell_info", typeof(AudioClip)) as AudioClip);
		audioDictionary.Add("weapon", Resources.Load("killed_with_trophy_blunt_force_trauma", typeof(AudioClip)) as AudioClip);
		audioDictionary.Add("picture", Resources.Load("lawrenceville_community_center_pool_locker_room", typeof(AudioClip)) as AudioClip);
		audioDictionary.Add("bag",Resources.Load("lawrenceville_community_center_pool_locker_room", typeof(AudioClip)) as AudioClip);
		audioDictionary.Add("northriver", Resources.Load("northriver_prep_is_a_high_school", typeof(AudioClip)) as AudioClip);
	}

	public void Process(string text)
	{
		Debug.Log("Speech recognized: " + text);
        /*
		if (text.ToLower().Contains("ball"))
		{
			Debug.Log("You said ball");
		}
        */

		if(text.ToLower().Contains("who"))
		{
			AudioClip whoClip = audioDictionary["who"];
			audio.PlayOneShot(whoClip);
			return;
		}
        if (text.ToLower().Contains("weapon"))
        {
			audio.PlayOneShot (audioDictionary["weapon"]);
			return;
        }
        if (text.ToLower().Contains("picture"))
        {
			audio.PlayOneShot (audioDictionary["picture"]);
			return;
        }
        if (text.ToLower().Contains("bag"))
		{
			audio.PlayOneShot (audioDictionary["bag"]);
			return;
		}
        if (text.ToLower().Contains("northriver"))
        {
			audio.PlayOneShot(audioDictionary["northriver"]);
			return;
        }

		audio.PlayOneShot (audioDictionary["error"]);


    }
}
