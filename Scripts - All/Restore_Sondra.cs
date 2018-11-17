using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restore_Sondra : MonoBehaviour {
    public GameObject sondra;
    public GameObject wolf;

    private Vector3 sondraStartLoc;
    private Vector3 wolfStartLoc;
	// Use this for initialization
	void Start () {
        sondraStartLoc = sondra.transform.position;

        if (wolf != null)
        {
            wolfStartLoc = wolf.transform.position;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject == sondra)
        {
            sondra.transform.parent.parent.position = sondraStartLoc;
        }

        if (wolf != null && other.gameObject == wolf)
        {
            wolf.transform.parent.parent.position = wolfStartLoc;
        }
    }
}
