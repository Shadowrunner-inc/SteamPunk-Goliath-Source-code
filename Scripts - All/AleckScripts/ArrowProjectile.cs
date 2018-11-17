using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProjectile : MonoBehaviour {

	public Rigidbody arrow;
	public float speed = 20.0f;

	// Use this for initialization
	void Start () {
		arrow = transform.GetComponent<Rigidbody> ();
		arrow.velocity = speed * transform.forward;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.GetComponent<Target>() && other.gameObject.GetComponent<Target> ().objectType != Target.Object_Type.Shield) 
		{
			other.gameObject.GetComponent<Target>().DamageWeakPoint ();
		}
	}
}
