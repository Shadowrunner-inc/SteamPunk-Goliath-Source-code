using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duststorm_ChildSupport : MonoBehaviour {

    public GameObject cameraController;

    private CameraTargetController camTarCon;

	// Use this for initialization
	void Start () {
        camTarCon = cameraController.GetComponent<CameraTargetController>();
	}
	
	// Update is called once per frame
	void Update () {
		if (camTarCon.controlledChar == 0)
        {
            transform.parent = camTarCon.wolf.transform;
            transform.position = camTarCon.wolf.transform.position;
        }

        if (camTarCon.controlledChar == 1)
        {
            transform.parent = camTarCon.sondra.transform;
            transform.position = camTarCon.sondra.transform.position;
        }
	}
}
