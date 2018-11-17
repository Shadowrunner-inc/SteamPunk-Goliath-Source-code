using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThighBlaster : MonoBehaviour {

	//TODO:
	//-needs to spawn sondra teleport UI
	//-Stop Genbu if all 4 legs are blown off, and trigger snake sequence

	public int numBroken = 0;

	public bool LFrontDamaged = false;
	public bool RFrontDamaged = false;
	public bool LBackDamaged = false;
	public bool RBackDamaged = false;
	public bool LFrontBroken = false;
	public bool RFrontBroken = false;
	public bool LBackBroken = false;
	public bool RBackBroken = false;
    public bool voicePlayed;

	public GameObject LFrontOil;
	public GameObject RFrontOil;
	public GameObject LBackOil;
	public GameObject RBackOil;
	public GameObject LFrontExplosion;
	public GameObject RFrontExplosion;
	public GameObject LBackExplosion;
	public GameObject RBackExplosion;

	public GameObject LFrontLeg1;
	public GameObject LFrontLeg2;
	public GameObject LFrontLeg3;
	public GameObject RFrontLeg1;
	public GameObject RFrontLeg2;
	public GameObject RFrontLeg3;
	public GameObject LBackLeg1;
	public GameObject LBackLeg2;
	public GameObject LBackLeg3;
	public GameObject RBackLeg1;
	public GameObject RBackLeg2;
	public GameObject RBackLeg3;

	public Animator genbuAnimator;
    public WolfVoiceCon wolfVoice;

    // Use this for initialization
    void Start () {
		LFrontOil.SetActive (false);
		RFrontOil.SetActive (false);
		LBackOil.SetActive (false);
		RBackOil.SetActive (false);
		LFrontExplosion.SetActive (false);
		RFrontExplosion.SetActive (false);
		LBackExplosion.SetActive (false);
		RBackExplosion.SetActive (false);

        voicePlayed = false;
	}

	void Update () {
		numBroken = 0;

		if (LFrontBroken) {
			LFrontOil.SetActive (false);
			LFrontExplosion.SetActive (true);
			LFrontLeg1.transform.parent = null;
			LFrontLeg2.transform.parent = null;
			LFrontLeg3.transform.parent = null;
			LFrontLeg1.GetComponent<Rigidbody> ().isKinematic = false;
			LFrontLeg2.GetComponent<Rigidbody> ().isKinematic = false;
			LFrontLeg3.GetComponent<Rigidbody> ().isKinematic = false;
			numBroken++;
		} else if (LFrontDamaged) {
			LFrontOil.SetActive (true);
		}
		if (RFrontBroken) {
			RFrontOil.SetActive (false);
			RFrontExplosion.SetActive (true);
			RFrontLeg1.transform.parent = null;
			RFrontLeg2.transform.parent = null;
			RFrontLeg3.transform.parent = null;
			RFrontLeg1.GetComponent<Rigidbody> ().isKinematic = false;
			RFrontLeg2.GetComponent<Rigidbody> ().isKinematic = false;
			RFrontLeg3.GetComponent<Rigidbody> ().isKinematic = false;
			numBroken++;
		} else if (RFrontDamaged) {
			RFrontOil.SetActive (true);
		}
		if (LBackBroken) {
			LBackOil.SetActive (false);
			LBackExplosion.SetActive (true);
			LBackLeg1.transform.parent = null;
			LBackLeg2.transform.parent = null;
			LBackLeg3.transform.parent = null;
			LBackLeg1.GetComponent<Rigidbody> ().isKinematic = false;
			LBackLeg2.GetComponent<Rigidbody> ().isKinematic = false;
			LBackLeg3.GetComponent<Rigidbody> ().isKinematic = false;
			numBroken++;
		} else if (LBackDamaged) {
			LBackOil.SetActive (true);
		}
		if (RBackBroken) {
			RBackOil.SetActive (false);
			RBackExplosion.SetActive (true);
			RBackLeg1.transform.parent = null;
			RBackLeg2.transform.parent = null;
			RBackLeg3.transform.parent = null;
			RBackLeg1.GetComponent<Rigidbody> ().isKinematic = false;
			RBackLeg2.GetComponent<Rigidbody> ().isKinematic = false;
			RBackLeg3.GetComponent<Rigidbody> ().isKinematic = false;
			numBroken++;
		} else if (RBackDamaged) {
			RBackOil.SetActive (true);
		}

		if (numBroken >= 2) {
			genbuAnimator.SetBool ("isCrawling", true);
		}

        if (numBroken >= 4)
        {
            Genbu_AI genbuAI = GetComponent<Genbu_AI>();
            genbuAI.dead = true;
        }

        if(LFrontDamaged && RFrontDamaged && LBackDamaged && RBackDamaged)
        {
            if(!voicePlayed)
            {
                wolfVoice.ReleaseTheGas();
                voicePlayed = true;
            }
        }
	}
}
