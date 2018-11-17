using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamShake : MonoBehaviour {

    public bool shake;
    [Range(0,9)]
    public int magnitude;
    [Range(0,1)]
    public float freq;
    [Range(0,6)]
    public float ampl;
    private Vector3 localStartPos;

    private bool active;

	// Use this for initialization
	void Start () {
        localStartPos = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {

        if (shake && !active)
        {
            StartCoroutine(Shake());
        }

	}

    IEnumerator Shake()
    {
        active = true;
        while (shake)
        {
            Vector3 shak = new Vector3(localStartPos.x + Random.Range(-magnitude, magnitude), localStartPos.y + Random.Range(-magnitude, magnitude), localStartPos.z);
            transform.localPosition = Vector3.Lerp(transform.localPosition, shak, ampl);
            
            yield return new WaitForSeconds(freq / 2);
        }
        shake = false;
        active = false;
        transform.localPosition = localStartPos;
        yield return null;
    }
}
