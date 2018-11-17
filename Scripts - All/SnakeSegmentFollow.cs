using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeSegmentFollow : MonoBehaviour {

	public Transform toFollow;
	public float offset = 1f;
	public bool noParent = false;

	void Start () {
		
	}

	void Update () {
		if (!noParent) {
			transform.position = offset * Vector3.Normalize (transform.position - toFollow.position) + toFollow.position;
			transform.LookAt (toFollow);
		}
	}
}
