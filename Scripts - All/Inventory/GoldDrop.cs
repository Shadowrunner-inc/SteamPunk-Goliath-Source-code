using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldDrop : MonoBehaviour {

    //When certain things happened, drop the gold coin on the position for player to pick it up

    public GameObject Gold_Coin_Prefab;
    private bool ToDropCoin = true;

    public int CoinsDrop_Amount;

    private void Update()
    {
        if (ToDropCoin)
        {
            GenbuRedBoltsCheck();
        }

    }

    //For genbu level, when destory red bolts on genbu's leg, for each red bolts destroyed, drop a coin there.
    public void GenbuRedBoltsCheck()
    {
        if (gameObject.GetComponent<MeshRenderer>().enabled)
        {
            print("MeshRenderer on!");
        }
        else
        {
            print("MeshRenderer off!");
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
        GameObject spawned = Instantiate(Gold_Coin_Prefab, gameObject.transform);
        spawned.transform.position = gameObject.transform.position;
    }

}
