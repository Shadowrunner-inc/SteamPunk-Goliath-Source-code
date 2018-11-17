using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Repulsion_Field : MonoBehaviour {

    public float activationTime = 5;

    public Image inRepulsorWarning;
    public Image inRepulsorDamaged;

    public GameObject sfx;

    private float timer = 0;
    private Hitbox hitbox;

	// Use this for initialization
	void Start () {
        if (!GetComponent<Hitbox>())
        {
            gameObject.AddComponent<Hitbox>();
        }

        hitbox = GetComponent<Hitbox>();
        hitbox.enabled = false;

        gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        
        if (timer <= activationTime)
        {
            GetComponent<Collider>().enabled = true;
            //GetComponent<Renderer>().enabled = true;
            hitbox.enabled = false;
            sfx.SetActive(true);
            sfx.transform.GetChild(0).gameObject.SetActive(false);
        }

        else
        {
            hitbox.enabled = true;
            //GetComponent<Renderer>().enabled = false;
            sfx.SetActive(true);
            sfx.transform.GetChild(0).gameObject.SetActive(true);
            if (timer > (activationTime + hitbox.disappearTime))
            {
                hitbox.enabled = false;
                timer = 0;
                inRepulsorWarning.enabled = false;
                inRepulsorDamaged.enabled = false;
                sfx.SetActive(false);
                gameObject.SetActive(false);
            }
        }
	}
    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && timer <= activationTime)
        {
            inRepulsorWarning.enabled = true;
            inRepulsorDamaged.enabled = false;
        }

        else if (other.gameObject.tag == "Player" && timer > activationTime)
        {
            inRepulsorWarning.enabled = false;
            inRepulsorDamaged.enabled = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inRepulsorWarning.enabled = false;
            inRepulsorDamaged.enabled = false;
        }
    }
    
}
