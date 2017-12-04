using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickup : MonoBehaviour {
	public GameObject static_sam;
	public GameObject static_alex;
	public GameObject sam;
	public GameObject alex;

	public SteamVR_TrackedObject Controller;

	private bool trigger_inside;


	void Update() {
		var ipt = SteamVR_Controller.Input((int)Controller.index);
		if (ipt.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
		{
			Debug.Log ("Pressed");
			if (trigger_inside) {
				if (gameObject.tag == "sam") {
					Debug.Log ("pickem up sam");
					sam.SetActive (true);
					static_sam.SetActive (false);
				}

				if (gameObject.tag == "alex") {
					alex.SetActive (true);
					static_alex.SetActive (false);
				}
			}
		}

		if (ipt.GetPressUp (SteamVR_Controller.ButtonMask.Trigger)) {
			Debug.Log ("RELEASE");
			if (gameObject.tag == "sam") {
				sam.SetActive (false);
				static_sam.SetActive (true);
			}

			if (gameObject.tag == "alex") {
				alex.SetActive (false);
				static_alex.SetActive (true);
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log ("enter");
		trigger_inside = true;
	}

	void OnTriggerExit(Collider other) {
		Debug.Log ("exit");
		trigger_inside = false;
	}

}
