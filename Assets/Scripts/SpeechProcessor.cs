using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SpeechProcessor : MonoBehaviour {

	public void Process(string text)
	{
        AudioSource audio = GetComponent<AudioSource>();
        AudioClip reply = (AudioClip)Resources.Load("Sounds/Errors/didnt_catch_that.wav");
		Debug.Log("Speech recognized: " + text);
        /*
		if (text.ToLower().Contains("ball"))
		{
			Debug.Log("You said ball");
		}
        */

        if (text.ToLower().Contains("who"))
        {
            reply = (AudioClip) Resources.Load("Sounds/alex_russell_info.wav");
        }
        if (text.ToLower().Contains("weapon"))
        {
            reply = (AudioClip)Resources.Load("Sounds/killed_with_trophy_blunt_force_trauma.wav");
        }
        if (text.ToLower().Contains("picture"))
        {
            reply = (AudioClip)Resources.Load("Sounds/lawrenceville_community_center_pool_locker_room.wav");
        }
        if (text.ToLower().Contains("bag"))
        {
            reply = (AudioClip)Resources.Load("Sounds/nothing_in_the_bag.wav");
        }
        if (text.ToLower().Contains("northriver"))
        {
            reply = (AudioClip)Resources.Load("Sounds/northriver_prep_is_a_high_school.wav");
        }

        audio.PlayOneShot(reply);
    }
}
