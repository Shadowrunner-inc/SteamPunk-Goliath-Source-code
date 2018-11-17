using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Nathan Hales
/// This manages spawns in credit objects at a determined rate and point in the credit scene.
/// </summary>

public class Credit_Manager : MonoBehaviour
{
    public CreditData[] _creditInfo;
    public GameObject creditPrefab;

    [SerializeField] private Vector3 spawnPoint; //point at which the criedits will be spawned.
    [SerializeField] private float maxHeight; //this is the y threshold for when this gets destroyed.
    [SerializeField] private float spawnRate; //Spacing out the next time this is called.
    [SerializeField] private float scrollingSpeed; // The rate are which the credits will rise.

   [SerializeField] private int producedAmount = 0;

   [SerializeField] private bool isDone = false;

    float timeKeeper = 0;

	// Use this for initialization
	void Start () {

        MakeNewCredit();
        timeKeeper = spawnRate;

	}

    private void OnEnable()
    {
        producedAmount = 0;
    }


    private void Update()
    {
        timeKeeper -= Time.deltaTime;

        if (timeKeeper <= 0f && isDone == false) {
            MakeNewCredit();
        }
    }


    void MakeNewCredit()
    {

        //If you have produced all the credits stop.
        if (producedAmount >= _creditInfo.Length){
            isDone = true;
            StartCoroutine(RestartCredits());
        }

        else
        {
            GameObject newCredit = Instantiate(creditPrefab); //Spawn our credit

            //Set it to be at the spawnpoint.
            newCredit.transform.position = spawnPoint;

            //Setup the text for the proper information;
            newCredit.GetComponentInChildren<TextMesh>().text =_creditInfo[producedAmount].name + "\n" + _creditInfo[producedAmount].teamRole;

            //Tell it's script what Hieght not to pass.
            newCredit.GetComponent<Credit_AI>().maxHeight = maxHeight;

            //Set the Credit's rising speed.
            newCredit.GetComponent<Credit_AI>().speed = scrollingSpeed;

            producedAmount++; //Increase tick

            timeKeeper = spawnRate;

        }

    }

    IEnumerator RestartCredits() {
        yield return new WaitForSeconds(spawnRate*2);
        isDone = false;
        producedAmount = 0;
        timeKeeper = spawnRate;
    }
}
