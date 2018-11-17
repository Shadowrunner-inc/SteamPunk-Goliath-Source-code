using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Nathan Hales
//*Editied By Aleck J.
//Tracks and compares objects that enter its area. If Genbu enters and the trap has been armed its stuns him.

public class SteamPipes : MonoBehaviour {

	[SerializeField]GameObject steamExplosion;
	[SerializeField]float StunningTime = 3f;

	public bool bIsArmed = false;
	private bool bWasUsed = false;
    public bool shot = false;

    private void Update()
    {
        if (shot)
        {
            GetComponent<BoxCollider>().size = new Vector3(40, 30, 76);
        }
    }

    void OnTriggerEnter(Collider other){
        // && bWasUsed == false
        if (other.tag == "GenbuPart" && bIsArmed && shot){
            Debug.Log("I hit Genbu");
            GameObject genbu = GameObject.FindGameObjectWithTag("Genbu");
            Debug.Log(genbu.name);
            StartCoroutine(genbu.GetComponent<Genbu_AI>().StunGenbu(gameObject, StunningTime));
            bWasUsed = true;
            GetComponent<Collider>().enabled = false;
            //Destroy (gameObject);
		}
	}
    
	/*	
	void OnTriggerStay(Collider other){
        //&& bWasUsed == false
        if (other.tag == "GenbuPart" && bIsArmed)
        {
            Debug.Log("I hit Genbu");
            GameObject genbu = GameObject.FindGameObjectWithTag("Genbu");
            Debug.Log(genbu.name);
            StartCoroutine(genbu.GetComponent<Genbu_AI>().StunGenbu(StunningTime));
            bWasUsed = true;
            Destroy (gameObject);
        }
    }
    */

	void OnDestroy(){
		if(steamExplosion != null){
		GameObject _Explosion = Instantiate (steamExplosion);
			_Explosion.transform.position = transform.position;}
	}

}
