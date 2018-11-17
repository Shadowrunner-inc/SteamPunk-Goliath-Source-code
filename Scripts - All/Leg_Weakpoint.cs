using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leg_Weakpoint : MonoBehaviour {
    
    public Genbu_AI genbuAI;

    public GameObject[] shieldsToDestroy;

    [Range(0,3)]
    public int legID;

    public float fadeSpeed = 0.1f;
    public float fadeTimer;

    private float timer;
    private bool beat;
    public bool shot;
    public bool played;
    public Material materialOne;
    public Material materialTwo;

    private int shieldCounter = 0;

    public WolfVoiceCon wolfVoice;


	// Use this for initialization
	void Start () {

        materialOne = gameObject.GetComponent<Renderer>().material;

        if (genbuAI == null)
            genbuAI = GameObject.Find("Genbu").gameObject.GetComponent<Genbu_AI>();
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        if (timer < fadeTimer)
        {
            Color color = GetComponent<Renderer>().material.color;
            if (!beat)
            {   
                if (color.a > 0)
                    color.a -= fadeSpeed;
            }

            else if (beat)
            {
                if (color.a < 1)
                    color.a += fadeSpeed;
            }
            
            GetComponent<Renderer>().material.color = color;
        }

        else if (timer >= fadeTimer)
        {
            if (beat)
            {
                beat = false;
            }

            else
            {
                beat = true;
            }
            timer = 0;
        }

        /*if (gameObject.transform.parent.childCount <= 1 && !shot)
        {
            gameObject.GetComponent<Collider>().enabled = true;
            gameObject.GetComponent<Renderer>().enabled = true;
        }
        */
        for(int i = 0; i < shieldsToDestroy.Length; i++)
        {
            if (shieldsToDestroy[i].transform.childCount <= 0)
            {
                shieldCounter += 1;
            }

            if (i == shieldsToDestroy.Length - 1)
            {
                if (shieldCounter >= 3)
                {
                    gameObject.GetComponent<Collider>().enabled = true;
                    gameObject.GetComponent<Renderer>().enabled = true;
                    if(!played)
                    {
                        wolfVoice.WeakSpotVoice();
                        played = true;
                    }
                }
                else
                {
                    shieldCounter = 0;
                    played = false;
                }
            }
        }

        /*else if (gameObject.transform.parent.childCount > 1 || shot)
        { 
            gameObject.GetComponent<Collider>().enabled = false;
            gameObject.GetComponent<Renderer>().enabled = false;
        }
        */
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<ArrowProjectile>() && !shot)
        {
            StartCoroutine(genbuAI.WeakPointHit("Sondra", gameObject, legID));
            gameObject.GetComponent<Renderer>().material = materialTwo;
            shot = true;
            Destroy(other.gameObject);
        }
    }
}
