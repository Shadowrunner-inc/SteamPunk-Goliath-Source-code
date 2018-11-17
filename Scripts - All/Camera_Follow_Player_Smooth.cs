using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow_Player_Smooth : MonoBehaviour {


	public Transform target;
	public float smoothTime = 0.3f;

	public float cameraHeight = 15;
	public float cameraDistance = -8;
	public float xCameraRotation = 75;

	private Vector3 velocity = Vector3.zero;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (target != null)
        {
			Vector3 offset = new Vector3(0, cameraHeight, cameraDistance);
            Vector3 goalPos = target.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, goalPos, ref velocity, smoothTime);

			transform.eulerAngles = new Vector3 (xCameraRotation, 0, 0);
        }

		
	}
}
