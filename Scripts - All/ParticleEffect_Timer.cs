using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect_Timer : MonoBehaviour {

	ParticleSystem _particlesystem; //Storing the attached Particle system
	public bool BInfinite = false;//Is this Endless

	void Start(){
		_particlesystem = GetComponent<ParticleSystem> ();

		var loopMain = _particlesystem.main;
		loopMain.loop = BInfinite;
	}


	void Update(){
		if (_particlesystem.isStopped)
			gameObject.SetActive (false);
	
	}

}
