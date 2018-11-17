using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scroling : MonoBehaviour {

	public float speed1 = 0.9f;
	public float speed2 = 0.9f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		float offsetX = Time.time * speed1;
		float offsetY = Time.time * speed2;

		GetComponent<Renderer> ().material.mainTextureOffset = new Vector2 (offsetX, offsetY);
	}
}
