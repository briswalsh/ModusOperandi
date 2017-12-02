using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(AudioSource))]
public class SpeechProcessor : MonoBehaviour {

	/*
	 * TODO:
	 * get back to all_questions from yearbook?
	 * alex-specific
	 * wipe speech.txt at start
	 */

	enum State
	{
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

	public GameObject yearbookPhoto;
	private Dictionary<string, AudioClip> audioDictionary; 
	private AudioSource audioSrc;
	private Dictionary<string, Action> responseMap;
	private State state;
	private Dictionary<string, int> multilinePositions;

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
		audioDictionary = new Dictionary<string, AudioClip>();
		audioSrc = GetComponent<AudioSource>();
		audioSrc.enabled = true;
		
		responseMap = new Dictionary<string, Action>();
		multilinePositions = new Dictionary<string, int>();

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

		MapResponse("en wy see see", NYCC);

		state = State.SAY_YES;
		StartCoroutine(PlayIntro());
	}

	void Update()
	{
        if (Input.GetKeyDown(KeyCode.Space))
		{
			Advance();
		}
		if (Input.GetKeyDown(KeyCode.Y))
		{
			//skip to yearbook reveal
			Highschool();
		}
		if (Input.GetKeyDown(KeyCode.L))
		{
			//skip to LCC_Done
			LCCquestions = 3;
			CheckLCCQuestions();
        }

		if (Input.GetKeyDown(KeyCode.S))
		{
			//skip to solved question
			Solve();
        }
	}

	private void MapResponse(string word, Action response)
	{
		responseMap.Add(word, response);
	}
	
	private void PlayAudio(string clipName)
	{
		if (!audioDictionary.ContainsKey(clipName))
		{
			AudioClip audioClip = Resources.Load(clipName, typeof(AudioClip)) as AudioClip;
			audioDictionary.Add(clipName, audioClip);
		}
		audioSrc.Stop();
		audioSrc.PlayOneShot(audioDictionary[clipName]);
	}

	public void Process(string text)
	{
		Debug.Log("Speech recognized: " + text);
		
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

	private void PlayMultilineAudio(string prefix, int min, int max)
	{
		//min and max are both INCLUSIVE
		int n = min;
		if (multilinePositions.ContainsKey(prefix))
		{
			n = multilinePositions[prefix];
		}
		PlayAudio(prefix + "_" + n);
		n++;
		if (n > max)
		{
			n = min;
		}
		multilinePositions[prefix] = n;
	}

	private void Advance()
	{
		switch (state)
		{
			case State.SAY_YES:
				Confirm();
				break;
			case State.CONFIRM_BOARD:
				Confirm();
				break;
			case State.REMIND_NAME:
				Sam();
                break;
			case State.READ_FILES:
				Confirm();
				break;
			case State.GOT_IT:
				Confirm();
				break;
			case State.CONFIRM_LCC_PHOTO:
				Confirm();
				break;
			case State.LCC_COD:
				Strangled();
                break;
			case State.LCC_WEAPON:
				Medal();
                break;
			case State.LCC_QUESTIONS:
				//TODO: how should this one be done? it requires 3 parts
				break;
			case State.ALL_QUESTIONS:
				Highschool();
                break;
			case State.YEARBOOK:
				//TODO: this one also requires multiple parts
				break;
			case State.SOLVED_QUESTION:
				Confirm();
				break;
			case State.SOLVED_ANSWER:
				NYCC();
                break;
		}
	}

	private IEnumerator PlayIntro()
	{
		while (state == State.SAY_YES)
		{
			PlayAudio("pick_up");
			while (audioSrc.isPlaying)
			{
				yield return new WaitForSeconds(0.1f);
			}
			yield return new WaitForSeconds(5);
		}
	}
	
	private void PlayError()
	{
		if (state == State.REMIND_NAME)
		{
			PlayMultilineAudio("confirmboard_unrelated", 1, 2);
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
			PlayMultilineAudio("error", 1, 6);
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
			PlayAudio("readfile_confirm");
			StartCoroutine(JesseNote());
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

	private IEnumerator ReadFiles()
	{
		while (audioSrc.isPlaying)
		{
			yield return new WaitForSeconds(0.1f);
		}
		yield return new WaitForSeconds(5.0f);
		PlayAudio("readfile_time_pass");
	}

	private IEnumerator JesseNote()
	{
		while (audioSrc.isPlaying)
		{
			yield return new WaitForSeconds(0.1f);
		}
		yield return new WaitForSeconds(1.0f);
		PlayAudio("found_jessenote");
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
			StartCoroutine(ReadFiles());
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
			PlayAudio("LCC_where");
			if (!LCCwhere)
			{
				LCCquestions++;
				LCCwhere = true;
				CheckLCCQuestions();
			}
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
			PlayAudio("LCC_medal");
			if (!LCCmedal)
			{
				LCCquestions++;
				LCCmedal = true;
				CheckLCCQuestions();
			}
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
			PlayAudio("LCC_bag");
			if (!LCCbag)
			{
				LCCquestions++;
				LCCbag = true;
				CheckLCCQuestions();
			}
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
			PlayAudio("LCC_victim");
			if (!LCCvictim)
			{
				LCCquestions++;
				LCCvictim = true;
				CheckLCCQuestions();
			}
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
			PlayAudio("LCC_LCC");
			if (!LCCLCC)
			{
				LCCquestions++;
				LCCLCC = true;
				CheckLCCQuestions();
			}
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
			PlayAudio("LCC_wife");
			if (!LCCwife)
			{
				LCCquestions++;
				LCCwife = true;
				CheckLCCQuestions();
			}
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
			PlayAudio("LCC_why");
			if (!LCCwhy)
			{
				LCCquestions++;
				LCCwhy = true;
				CheckLCCQuestions();
			}
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
		StartCoroutine(CheckLCCDelayed());
	}

	private IEnumerator CheckLCCDelayed()
	{
		if (LCCquestions >= 3)
		{
			while (audioSrc.isPlaying)
			{
				yield return new WaitForSeconds(0.1f);
			}
			state = State.ALL_QUESTIONS;
			yield return new WaitForSeconds(1);
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
			yearbookPhoto.SetActive(true);
			StartCoroutine(RevealYearbook());
        }
		else
		{
			PlayError();
		}
	}

	private IEnumerator RevealYearbook()
	{
		while (audioSrc.isPlaying)
		{
			yield return new WaitForSeconds(0.1f);
		}
		yield return new WaitForSeconds(1);
		PlayAudio("general_school_murder_connection");
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
