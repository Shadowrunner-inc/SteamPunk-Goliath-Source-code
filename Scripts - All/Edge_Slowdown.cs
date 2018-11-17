using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge_Slowdown : MonoBehaviour {

    public GameObject player;
    public float lookdownDistance = 10;

    [Space(8)]
    [Range(0,100)]
    public int slowToPercentage = 75;
    [Range(0, 100)]
    public int restoreToPercentage = 100;


    private SondraMovement sondraMov;


    private float sondraOriginalSpeed;
    private float slowSpeed;
    private float restoreSpeed;

	public bool borderOn = true;
	// Use this for initialization
	void Start () {
        //==============================================================================================================
        // Start-Up Stuff
        if (player != null && player.GetComponent<SondraMovement>())
        {
            sondraMov = player.GetComponent<SondraMovement>();
        }

        else if (player != null && !player.GetComponent<SondraMovement>())
        {
            gameObject.SetActive(false);
        }

        else
        {
            gameObject.SetActive(false);
        }
        //==============================================================================================================

        sondraOriginalSpeed = sondraMov.speed;
        slowSpeed = sondraOriginalSpeed * slowToPercentage / 100;
        restoreSpeed = sondraOriginalSpeed * restoreToPercentage / 100;
	}
	
	// Update is called once per frame
	void Update () {

		if (borderOn) {
			RaycastHit hit;

			if (Physics.Raycast (transform.position, Vector3.down, out hit, 10)) {
				if (sondraMov.speed < restoreSpeed && Input.GetKey(KeyCode.W)) {
					sondraMov.speed = restoreSpeed;
				}
			} else {
				if (sondraMov.speed > slowSpeed) {
					sondraMov.speed = slowSpeed;
				}
			}
		}
    }
	void OnTriggerStay(Collider border)
	{
		if(border.gameObject.tag == "NotBorder")
			borderOn = false;
	}

	void OnTriggerExit(Collider border)
	{
		if(border.gameObject.tag == "NotBorder")
			borderOn = true;
	}
}
