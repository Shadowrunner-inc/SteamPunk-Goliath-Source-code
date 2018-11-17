using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsMovement : MonoBehaviour {

    public GameObject player;

    private Vector3 curLoc = Vector3.zero;
    private Vector3 prevLoc = Vector3.zero;

    private Vector3 movement;


    // Use this for initialization
    void Start () {
        curLoc = player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
			float moveHorizontal = Input.GetAxis ("Horizontal");
			float moveVertical = Input.GetAxis ("Vertical");

			movement = (moveVertical * player.transform.forward * 10) + (moveHorizontal * player.transform.right * 10);

			gameObject.transform.position = player.transform.position;
        
			if (Input.anyKey) {
				transform.rotation = Quaternion.RotateTowards (transform.rotation, Quaternion.LookRotation (new Vector3 (movement.x * 10, 0, movement.z * 10)), 99999);
			}
	}
}
