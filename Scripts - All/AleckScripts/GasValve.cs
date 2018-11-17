using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasValve : MonoBehaviour {
	//Author: Alecksandar Jackowicz
	//To activate the gas pipe assigned to this gameobject when triggered

	public GasPipe gasPipe;


	public void ActivateGas(){
		gasPipe.hasGas = true;
	}

}
