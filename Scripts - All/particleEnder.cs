using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(ParticleSystem))]
public class particleEnder : MonoBehaviour {

	void Update () {
        if (GetComponent<ParticleSystem>().isEmitting == false) { Destroy(gameObject); }
	}
}
