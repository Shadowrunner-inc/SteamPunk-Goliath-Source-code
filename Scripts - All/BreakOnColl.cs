using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakOnColl : MonoBehaviour {

	public GameObject PipeBreak;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter (Collision other) {
		if (other.gameObject.CompareTag("GenbuPart")) {
		//	Instantiate (PipeBreak, transform.position, transform.rotation);
		//	Destroy (gameObject);
			other.gameObject.GetComponent<Rigidbody>().isKinematic = false;
		}
	}

}
