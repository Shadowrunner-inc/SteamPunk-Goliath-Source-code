using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Made by Jeffrey

public class BulletImpact : MonoBehaviour {




	public GameObject impactWallSand;
	public GameObject impactWallMetal;


//	public float delMe;

	void Update () {

//		delMe -= Time.deltaTime;
//		if (delMe <= 0) 
//			Destroy (gameObject);
	}


	void OnCollisionEnter(Collision other)
	{
	
	
		if (other.gameObject.tag == "WallSand") 
		{
			Debug.Log ("HIT SAND SAND");
			GameObject iWD = Instantiate(impactWallSand, transform.position, Quaternion.identity) as GameObject;
				Destroy(gameObject);
		}


		if (other.gameObject.tag == "WallMetal") 
		{
			Debug.Log ("HIT METAL METAL");
			GameObject iWM = Instantiate(impactWallMetal, transform.position, Quaternion.identity) as GameObject;
			Destroy(gameObject);
		}



	}

}
