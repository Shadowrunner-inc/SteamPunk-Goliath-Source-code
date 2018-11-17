using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldDrop_GenbuLeg : MonoBehaviour {

    public GameObject Gold_Coin_Prefab;
    private bool ToDropCoin = true;

    public int CoinsDrop_Amount;


    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (ToDropCoin)
        {
            GenbuLegDestoryedCheck();
        }
    }

    public void GenbuLegDestoryedCheck()
    {
        if (!gameObject.GetComponent<SphereCollider>().enabled)
        {
            print("Collider! off");
        }
        else
        {
            print("Collider! on");
            for (int i = 0; i < CoinsDrop_Amount; i++)
            {
                //call dropcoin() function for coins Drop_Amount
                DropCoin();
            }
            ToDropCoin = false;
        }

    }

    public void DropCoin()
    {
        GameObject spawned = Instantiate(Gold_Coin_Prefab);
        spawned.transform.position = gameObject.transform.position;
    }
}
