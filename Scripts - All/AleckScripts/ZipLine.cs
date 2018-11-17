using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZipLine : MonoBehaviour {
	//Author: Alecksandar Jackowicz
	//When activated, turns off the player controller.
	//Then Lerps the player from one pole to the other.

	//pole one sends as pole two receives the player
	public Transform poleOne;
	public Transform poleTwo;
	public Transform zipUser;

	public float speed;

	private float startTime;
	private float journeyLegth;

	public bool travelling;
	public bool justActivated;

	// Use this for initialization
	void Start () {
		//startTime = Time.time;
		journeyLegth = Vector3.Distance (poleOne.position, poleTwo.position);

	}
	
	// Update is called once per frame
	void Update () {
		
		if(justActivated){
			startTime = Time.time;
			justActivated = false;
		}

		if(travelling && zipUser != null){
			print ("Travelling");
			print (zipUser.position);
			float distCovered = (Time.time - startTime) * speed;
			float fracJourney = distCovered / journeyLegth;
			zipUser.position = Vector3.Lerp (poleOne.position, poleTwo.position, fracJourney);

			if(zipUser.position == poleTwo.position){
				print ("Reached");
				travelling = false;
				CodeControl changeBack = zipUser.GetComponent<CodeControl> ();
				changeBack.controls.enabled = true;
				zipUser = null;
				//Reactivate Movement Code
			}

		}
	}

	public void Traveller(Transform wander){
		
		zipUser = wander;
		//travelling = true;
		//Deactivate Movement Code
	}


}
