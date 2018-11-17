using UnityEngine;

//By Jeffrey aka Trickzbunny

// Place script on Gameobject (be sure Static is turned off) depending which bool is activated, the gameobject will turn in a different way.

public class FollowTarget : MonoBehaviour 
{

	public Transform target;
	public float speed = 1;

	public bool moveForward = false; // Notes not necessary. 



	void Update () 
	{
		float step = speed * Time.deltaTime;
		transform.LookAt (target);



		if (moveForward)
			transform.position = Vector3.MoveTowards (transform.position, target.position, step);


	}
}
