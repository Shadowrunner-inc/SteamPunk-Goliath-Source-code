using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Nathan Hales
/// Acts as a simple destructable. This turns of the original wall object and turns on a replacement mesh that has beeen segemented in away that allows for a physics explosion to simulate the destruction.
/// </summary>

public class BreakWall : MonoBehaviour {

	public bool Wall_break = false;

	public GameObject Wall_Origin;
	public GameObject Wall_Breaking;

	void Start () {
		//Wall_break = false;
	}

	void Update(){
		BreakingWall ();
	}

	void BreakingWall(){
		if(Wall_break){
			Wall_Origin.SetActive (false);
			Wall_Breaking.SetActive (true);
		}

		if(!Wall_break){
			Wall_Origin.SetActive (true);
			Wall_Breaking.SetActive (false);
		}
	}
}
