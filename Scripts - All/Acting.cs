using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acting : MonoBehaviour {

	public Animator sondraAnim;

	// Use this for initialization
	void Start () {
		StartCoroutine ("SondraRun");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator SondraRun () {
		yield return new WaitForSeconds (10);
		sondraAnim.SetBool ("Running", true);
	}

}
