using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion_Target : MonoBehaviour {

    public GameObject target;
    
	// Update is called once per frame
	void Update () {

        RaycastHit hit;

        if (Physics.Raycast(target.transform.position, Vector3.down, out hit, 9999))
        {
            gameObject.transform.position = hit.point;
        }	
	}
}
