using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderInteract : MonoBehaviour {
	//Author: Alecksandar Jackowicz
	//When activated the boulder will launch according to an arc.
	//If it hits the boss' shell then it will cause damage and spawn a particle effect

	public Rigidbody boulder;
	public int boldDmg = 50;

	public bool isHit = false;

	public float launchForce = 200f;
	//Health code Variable


	// Use this for initialization
	void Start () {
		boulder = this.GetComponent<Rigidbody> ();;
	}
	
	// Update is called once per frame
	/*
	void Update () {
		if (Input.GetKeyDown (KeyCode.O)) {
			Launch ();
		}
	}
*/

	void OnCollisionEnter(Collision other){
		if(other.gameObject.CompareTag("Genbu")){
			
			//FakeGenbu fakeGen = other.transform.GetComponent<FakeGenbu>();
			//fakeGen.fakeHealth -= boldDmg;

			Genbu_AI genAi = other.transform.GetComponent<Genbu_AI>();
			genAi.mainHealth -= boldDmg;
			Destroy (this.gameObject);
		}
	}

	public void Launch(){

		boulder.constraints = RigidbodyConstraints.None;
		boulder.velocity += transform.forward * launchForce;

	}


	//Health damage function
}
