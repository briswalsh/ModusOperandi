using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(AudioSource))]
public class SpeechProcessor : MonoBehaviour {

	/*
	 * TODO:
	 * play clips as time passes
	 * toggle between different versions of clips
	 * found_jessenote
	 * all TODOs
	 * general_school_murder_connection
	 * alex-specific
	 */

	enum State
	{
		PICK_UP,
		SAY_YES,
		CONFIRM_BOARD,
		REMIND_NAME,
		READ_FILES,
		GOT_IT,
		CONFIRM_LCC_PHOTO,
		LCC_COD,
		LCC_WEAPON,
		LCC_QUESTIONS,
		ALL_QUESTIONS,
		YEARBOOK,
		SOLVED_QUESTION,
		SOLVED_ANSWER
	}

	private Dictionary<string, AudioClip> audioDictionary; 
	private AudioSource audioSrc;
	Dictionary<string, Action> responseMap;
	State state;

	private bool LCCmedal;
	private bool LCCbag;
	private bool LCCvictim;
	private bool LCCwhere;
	private bool LCCLCC;
	private bool LCCwife;
	private bool LCCwhy;
	private int LCCquestions = 0;

	private bool yearbookMedal;
	private bool yearbookNYCC;
	private bool yearbookSwim;

	void Awake()
	{
		audioDictionary = new Dictionary<string, AudioClip> ();
		audioSrc = GetComponent<AudioSource>();
		audioSrc.enabled = true;

		//AudioClip errorClip = Resources.Load("didnt_catch_that", typeof(AudioClip)) as AudioClip;
		//audioDictionary.Add("error", errorClip);

		responseMap = new Dictionary<string, Action>();

		MapResponse("yes", Confirm);
		MapResponse("yeah", Confirm);

		MapResponse("alex russell", Alex);
		MapResponse("sam ellsworth", Sam);

		MapResponse("where", Where);

		MapResponse("strangled", Strangled);
		MapResponse("strangulation", Strangled);
		MapResponse("choked", Strangled);
		MapResponse("choking", Strangled);

		MapResponse("medal", Medal);
		MapResponse("ribbon", Medal);

		MapResponse("bag", Bag);

		MapResponse("victim", Victim);

		MapResponse("lawrenceville", LCC);
		MapResponse("center", LCC);

		MapResponse("wife", Wife);

		MapResponse("why", Why);

		MapResponse("death", CheckFile);
		MapResponse("time", CheckFile);
		MapResponse("weapon", CheckFile);

		MapResponse("witness", Witness);
		MapResponse("witnesses", Witness);

		MapResponse("locker", Locker);

		MapResponse("sitting", Sit);

		MapResponse("fingerprint", Fingerprint);
		MapResponse("fingerprints", Fingerprint);

		MapResponse("trophy", Trophy);

		MapResponse("struggle", Struggle);
		MapResponse("fight", Struggle);

		MapResponse("blood", Blood);

		MapResponse("northriver", Northriver);

		MapResponse("highschool", Highschool);

		MapResponse("championship", Championship);
		MapResponse("competition", Championship);

		MapResponse("swim", Swim);
		MapResponse("swimmer", Swim);

		MapResponse("relationship", Relationship);
		MapResponse("friends", Relationship);
		MapResponse("know", Relationship);

		MapResponse("who", Who);
		MapResponse("members", Who);

		MapResponse("pool", Pool);

		MapResponse("en why see see", NYCC);

		state = State.PICK_UP;
	}

	private void MapResponse(string word, Action response)
	{
		responseMap.Add(word, response);
	}

	/*private void MapResponseSimple(string word, string responseName)
	{
		responseMap.Add(word, () => PlayAudio(responseName));
	}*/

	private void PlayAudio(string clipName)
	{
		if (!audioDictionary.ContainsKey(clipName))
		{
			AudioClip audioClip = Resources.Load(clipName, typeof(AudioClip)) as AudioClip;
			audioDictionary.Add(clipName, audioClip);
		}
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
		PlayError();
	}

	private void PickUp()
	{
		state = State.SAY_YES;
		PlayAudio("pick_up");
	}

	private void PlayError()
	{
		if (state == State.REMIND_NAME)
		{
			PlayAudio("confirmboard_unrelated_1"); //TODO: switch between 1 and 2
		}
		else if (state == State.READ_FILES)
		{
			PlayAudio("readfile_no_confirm");
		}
		else if (state == State.SOLVED_QUESTION)
		{
			state = State.ALL_QUESTIONS;
			PlayAudio("solved_no");
		}
		else if (state == State.SOLVED_ANSWER)
		{
			state = State.ALL_QUESTIONS;
			PlayAudio("solved_other");
		}
		else
		{
			PlayError();
		}
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
		else if (state == State.READ_FILES)
		{
			state = State.GOT_IT;
			PlayAudio("read_file_confirm");
		}
		else if (state == State.GOT_IT)
		{
			state = State.CONFIRM_LCC_PHOTO;
			PlayAudio("gotit");
		}
		else if (state == State.CONFIRM_LCC_PHOTO)
		{
			state = State.LCC_COD;
			PlayAudio("LCCphoto_confirm");
		}
		else if (state == State.SOLVED_QUESTION)
		{
			state = State.SOLVED_ANSWER;
			PlayAudio("solved_yes");
		}
		else
		{
			PlayError();
		}
    }

	private void Alex()
	{
		if (state == State.REMIND_NAME)
		{
			PlayAudio("confirmboard_alex");
		}
		else
		{
			//TODO
			PlayError();
		}
	}

	private void Sam()
	{
		if (state == State.REMIND_NAME)
		{
			PlayAudio("confirmboard_sam");
			state = State.READ_FILES;
		}
		else
		{
			//TODO
			PlayError();
		}
	}

	private void Where()
	{
		if (state == State.READ_FILES)
		{
			PlayAudio("readfile_whatfile");
		}
		else if (state == State.CONFIRM_LCC_PHOTO)
		{
			PlayAudio("LCCphoto_where");
		}
		else if (state == State.LCC_QUESTIONS)
		{
			if (!LCCwhere)
			{
				LCCquestions++;
				LCCwhere = true;
				CheckLCCQuestions();
			}
			PlayAudio("LCC_where");
		}
		else if (state == State.YEARBOOK)
		{
			PlayAudio("championship_where");
		}
		else
		{
			PlayError();
		}
	}

	private void Strangled()
	{
		if (state == State.LCC_COD)
		{
			state = State.LCC_WEAPON;
			PlayAudio("LCCphoto_causeofdeath");
        }
		else
		{
			PlayError();
		}
	}

	private void Medal()
	{
		if (state == State.LCC_WEAPON)
		{
			state = State.LCC_QUESTIONS;
			PlayAudio("LCCphoto_murderweapon");
		}
		else if (state == State.LCC_QUESTIONS)
		{
			if (!LCCmedal)
			{
				LCCquestions++;
				LCCmedal = true;
				CheckLCCQuestions();
			}
			PlayAudio("LCC_medal");
		}
		else if (state == State.ALL_QUESTIONS)
		{
			PlayAudio("general_medal");
		}
		else if (state == State.YEARBOOK)
		{
			PlayAudio("championship_medal");
			yearbookMedal = true;
			CheckSolved();
		}
		else
		{
			PlayError();
		}
	}

	private void Bag()
	{
		if (state == State.LCC_QUESTIONS)
		{
			if (!LCCbag)
			{
				LCCquestions++;
				LCCbag = true;
				CheckLCCQuestions();
			}
			PlayAudio("LCC_bag");
		}
		else if (state == State.ALL_QUESTIONS)
		{
			PlayAudio("general_bag");
		}
		else
		{
			PlayError();
		}
	}

	private void Victim()
	{
		if (state == State.LCC_QUESTIONS)
		{
			if (!LCCbag)
			{
				LCCquestions++;
				LCCbag = true;
				CheckLCCQuestions();
			}
			PlayAudio("LCC_victim");
		}
		else if (state == State.ALL_QUESTIONS)
		{
			//TODO: get name of victim then respond based on it?
			PlayError();
		}
		else
		{
			PlayError();
		}
	}

	private void LCC()
	{
		if (state == State.LCC_QUESTIONS)
		{
			if (!LCCLCC)
			{
				LCCquestions++;
				LCCLCC = true;
				CheckLCCQuestions();
			}
			PlayAudio("LCC_LCC");
		}
		else
		{
			PlayError();
		}
	}

	private void Wife()
	{
		if (state == State.LCC_QUESTIONS)
		{
			if (!LCCwife)
			{
				LCCquestions++;
				LCCwife = true;
				CheckLCCQuestions();
			}
			PlayAudio("LCC_wife");
		}
		else
		{
			PlayError();
		}
	}

	private void Why()
	{
		if (state == State.LCC_QUESTIONS)
		{
			if (!LCCwhy)
			{
				LCCquestions++;
				LCCwhy = true;
				CheckLCCQuestions();
			}
			PlayAudio("LCC_why");
		}
		else if (state == State.ALL_QUESTIONS)
		{
			PlayAudio("general_why");
		}
		else
		{
			PlayError();
		}
	}

	private void CheckLCCQuestions()
	{
		if (LCCquestions >= 3)
		{
			state = State.ALL_QUESTIONS;
			PlayAudio("LCC_done");
		}
	}

	private void CheckFile()
	{
		if (state == State.LCC_QUESTIONS || state == State.ALL_QUESTIONS)
		{
			PlayAudio("checkfile");
		}
		else
		{
			PlayError();
		}
	}

	private void Witness()
	{
		if (state == State.ALL_QUESTIONS)
		{
			PlayAudio("general_witness");
		}
		else
		{
			PlayError();
		}
	}

	private void Locker()
	{
		if (state == State.ALL_QUESTIONS)
		{
			PlayAudio("general_locker");
		}
		else
		{
			PlayError();
		}
	}

	private void Sit()
	{
		if (state == State.ALL_QUESTIONS)
		{
			PlayAudio("general_sit");
		}
		else
		{
			PlayError();
		}
	}

	private void Fingerprint()
	{
		if (state == State.ALL_QUESTIONS)
		{
			PlayAudio("general_fingerprint");
		}
		else
		{
			PlayError();
		}
	}

	private void Trophy()
	{
		if (state == State.ALL_QUESTIONS)
		{
			PlayAudio("general_trophy");
		}
		else if (state == State.YEARBOOK)
		{
			PlayAudio("championship_trophy");
			yearbookMedal = true;
			CheckSolved();
		}
		else
		{
			PlayError();
		}
	}

	private void Struggle()
	{
		if (state == State.ALL_QUESTIONS)
		{
			PlayAudio("general_struggle");
		}
		else
		{
			PlayError();
		}
	}

	private void Blood()
	{
		if (state == State.ALL_QUESTIONS)
		{
			PlayAudio("general_blood");
		}
		else
		{
			PlayError();
		}
	}

	private void Northriver()
	{
		if (state == State.ALL_QUESTIONS)
		{
			PlayAudio("yearbook_northriver");
		}
		else
		{
			PlayError();
		}
	}

	private void Highschool()
	{
		if (state == State.ALL_QUESTIONS)
		{
			state = State.YEARBOOK;
			PlayAudio("yearbook_highschool");
			//TODO: unlock yearbook in scene
        }
		else
		{
			PlayError();
		}
	}

	private void Championship()
	{
		if (state == State.YEARBOOK)
		{
			PlayAudio("championship_championship");
		}
		else
		{
			PlayError();
		}
	}

	private void Swim()
	{
		if (state == State.YEARBOOK)
		{
			PlayAudio("championship_swim");
			yearbookSwim = true;
			CheckSolved();
		}
		else
		{
			PlayError();
		}
	}

	private void Who()
	{
		if (state == State.YEARBOOK)
		{
			PlayAudio("championship_who");
			yearbookSwim = true;
			CheckSolved();
		}
		else
		{
			PlayError();
		}
	}

	private void Relationship()
	{
		if (state == State.YEARBOOK)
		{
			PlayAudio("championship_relationship");
		}
		else
		{
			PlayError();
		}
	}

	private void Pool()
	{
		if (state == State.YEARBOOK)
		{
			PlayAudio("championship_pool");
		}
		else
		{
			PlayError();
		}
	}

	private void NYCC()
	{
		if (state == State.YEARBOOK)
		{
			PlayAudio("championship_nycc");
			yearbookNYCC = true;
			CheckSolved();
		}
		else if (state == State.SOLVED_QUESTION || state == State.SOLVED_ANSWER)
		{
			PlayAudio("solved_nycc");
			GameComplete();
		}
		else
		{
			PlayError();
		}
	}

	private void CheckSolved()
	{
		if (yearbookMedal && yearbookNYCC && yearbookSwim)
		{
			StartCoroutine(Solve());
        }
	}

	private IEnumerator Solve()
	{
		//wait for current audio to complete
		while (audioSrc.isPlaying)
		{
			yield return new WaitForSeconds(0.1f);
		}
		PlayAudio("solved_question");
		state = State.SOLVED_QUESTION;
	}

	private void GameComplete()
	{
		//TODO: any ending things
	}
}
