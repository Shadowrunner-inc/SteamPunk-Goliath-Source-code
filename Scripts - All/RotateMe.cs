﻿using UnityEngine;

//By Jeffrey aka Trickzbunny

// Place script on Gameobject (be sure Static is turned off) depending which bool is activated, the gameobject will turn in a different way.

public class RotateMe : MonoBehaviour 
{

	public float speed = 1;

	public bool flat_clockwise = false; // Notes not necessary. 
	public bool flat_antiClock = false; // 
	public bool wall_clockwise = false; // 
	public bool wall_antiClock = false; // 
	public bool toMe = false;			// 
	public bool awayFromME = false;		// 

    private void Start()
    {
        speed *= 50f;
    }

    void Update () 
	{
		if (flat_clockwise)
			gameObject.transform.Rotate (Vector3.up, speed * Time.deltaTime);
		else
			flat_clockwise = false;

		if (flat_antiClock)
			gameObject.transform.Rotate (Vector3.down, speed * Time.deltaTime);
		else
			flat_antiClock = false;

		if (wall_antiClock)
			gameObject.transform.Rotate (Vector3.left, speed * Time.deltaTime);
		else
			wall_antiClock = false;

		if (wall_clockwise)
			gameObject.transform.Rotate (Vector3.right, speed * Time.deltaTime);
		else
			wall_clockwise = false;

	
		if (toMe)
			gameObject.transform.Rotate (Vector3.back, speed * Time.deltaTime);
		else
			toMe = false;

		if (awayFromME)
			gameObject.transform.Rotate (Vector3.forward, speed * Time.deltaTime);
		else
			awayFromME = false;


	}
}
