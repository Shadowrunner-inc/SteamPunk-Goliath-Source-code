using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tanksplosion : MonoBehaviour {

	public bool boiler1Destroyed = false;
	public bool boiler2Destroyed = false;
	public bool boiler3Destroyed = false;
	public bool boiler4Destroyed = false;

	public GameObject boiler1Fire;
	public GameObject boiler2Fire;
	public GameObject boiler3Fire;
	public GameObject boiler4Fire;

	void Start () {
		boiler1Fire.SetActive (false);
		boiler2Fire.SetActive (false);
		boiler3Fire.SetActive (false);
		boiler4Fire.SetActive (false);
	}

	void Update () {
		if (boiler1Destroyed) {
			boiler1Fire.SetActive (true);
		}
		if (boiler2Destroyed) {
			boiler2Fire.SetActive (true);
		}
		if (boiler3Destroyed) {
			boiler3Fire.SetActive (true);
		}
		if (boiler4Destroyed) {
			boiler4Fire.SetActive (true);
		}
	}
}
