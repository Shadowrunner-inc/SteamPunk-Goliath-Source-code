using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Author: Nathan Hales
/// This is script gets placed on each credit object and acts as its brain. Adds self culling and movement fuctionality.
/// when the objecte rises to the threshold(Y axis point) it destorys its self.
/// </summary>
public class Credit_AI : MonoBehaviour
{
    [HideInInspector]public float maxHeight = 575f;


    [HideInInspector] public float speed;
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        Rise();
	}

    void Rise()
    {
        //Stored current position
        Vector3 newPosistion = transform.position;

        //Increase the y axis or the stored posistion
        newPosistion.y += speed*Time.deltaTime;

        //Assign to the object
        transform.position = newPosistion;

        HeightChecker();
    }

    /// <summary>
    /// Check the height of the object and destroys it if it goes to high.
    /// </summary>
    void HeightChecker()
    {
        if(transform.position.y >= maxHeight)
            Destroy(gameObject);
    }

}
