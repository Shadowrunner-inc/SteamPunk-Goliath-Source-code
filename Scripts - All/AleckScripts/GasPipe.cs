using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasPipe : MonoBehaviour {
	//Author: Alecksandar Jackowicz
	//To have the gas pipes explode when shot at.
	//Detects if genbu is close to the explosion

	//Shows when the gas vein is active to be shot at
	public bool hasGas;
	public bool usedUp;

	public float expRange = 30f;
	public int expDamage = 100;

	// Use this for initialization
	void Start () {
		usedUp = false;

	}
	
	// Update is called once per frame
	/*
	void Update () {
		if (Input.GetKeyDown (KeyCode.K)) {
			Explode ();

		}
	}

*/

	public void Explode(){
		//if the gas pipe is not used up
		if(!usedUp){

			//if hasgas is active then explode and detect colliders for damage
			if(hasGas){
				
				Collider[] hitColliders = Physics.OverlapSphere (this.transform.position, expRange);

				int i = 0;

				while (i < hitColliders.Length){
					
					if(hitColliders[i].CompareTag("Genbu")){
						//access genbu AI and damage him
						//FakeGenbu fakeGen = hitColliders[i].GetComponent<FakeGenbu>();
						//fakeGen.fakeHealth -= expDamage;
						//Instantiate Explosion

						Genbu_AI genAi = hitColliders[i].GetComponent<Genbu_AI>();
						genAi.mainHealth -= expDamage;
					}
					//end if
					i++;
				}
				//end while

				hasGas = false;
			}
			//end if

			usedUp = true;
		}
		//end if
	}
	//end explode

	void OnDrawGizmosSelected(){
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (transform.position, expRange);
	}

}
