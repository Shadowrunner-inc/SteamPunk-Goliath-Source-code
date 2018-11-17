using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enlargement : MonoBehaviour {

    public float enlargeSpeed;

    [HideInInspector]
    public Transform parent;

    public Vector3 startPoint;
    public Vector3 endPoint;
    public Vector3 centerPoint;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            endPoint = hit.point;
        }

        gameObject.transform.localScale += new Vector3(0, 0, enlargeSpeed * Time.deltaTime);

        if (parent != null)
        {
            gameObject.transform.position = parent.position;
        }
    }
}
