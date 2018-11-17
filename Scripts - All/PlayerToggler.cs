using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerToggler : MonoBehaviour 
{


	public GameObject player_Sondra;
	public GameObject player_Wolf;

	// ####>> Instead of two Cameras, change this into the Camera Script and have the camera switch "target" ?
	public GameObject player_Camera_Sondra;
	public GameObject player_Camera_Wolf;


	public bool playing_Sondra;
	public bool playing_Wolf;



	// Use this for initialization
	void Start ()
	{
//		player_Sondra.SetActive (true);
//		player_Wolf.SetActive (false);
		player_Camera_Sondra.SetActive (true);
		player_Camera_Wolf.SetActive (false);

		playing_Sondra = true;
		playing_Wolf = false;
	}

	// Update is called once per frame
	void Update () 
	{


		if (playing_Sondra) 
		{
			playing_Wolf = false;
			player_Camera_Wolf.SetActive (false);
			player_Wolf.GetComponent<Invector.CharacterController.vThirdPersonController>().enabled = false;
			player_Wolf.GetComponent<Invector.CharacterController.vThirdPersonInput>().enabled = false;

//			player_Sondra.SetActive (true);
//			player_Wolf.SetActive (false);

			player_Camera_Sondra.SetActive (true);
			player_Sondra.GetComponent<Invector.CharacterController.vThirdPersonController>().enabled = true;
			player_Sondra.GetComponent<Invector.CharacterController.vThirdPersonInput>().enabled = true;
		}
			
		if (playing_Wolf) 
		{
			playing_Sondra = false;
			player_Camera_Sondra.SetActive (false);
			player_Sondra.GetComponent<Invector.CharacterController.vThirdPersonController>().enabled = false;
			player_Sondra.GetComponent<Invector.CharacterController.vThirdPersonInput>().enabled = false;

//			player_Wolf.SetActive (true);
//			player_Sondra.SetActive (false);

			player_Camera_Wolf.SetActive (true);
			player_Wolf.GetComponent<Invector.CharacterController.vThirdPersonController>().enabled = true;
			player_Wolf.GetComponent<Invector.CharacterController.vThirdPersonInput>().enabled = true;
		}
			


		if (Input.GetKeyDown (KeyCode.T)) 
		{
			
			if (playing_Sondra) {
				Debug.Log ("I am Playing Sondra");
				playing_Sondra = false;
				playing_Wolf = true;
			}
			else if (playing_Wolf) {
				Debug.Log ("I am Playing Wolf");
				playing_Wolf = false;
				playing_Sondra = true;
			}
		//	PlayerTop.SetActive (false);
		//	PlayerIntro.SetActive (false);
		//	PlayerBottom.SetActive (true);
		}

	}
}
