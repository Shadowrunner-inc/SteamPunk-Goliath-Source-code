using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldPickup : MonoBehaviour {


    private void Update()
    {
        CoinRotate();
    }

    void CoinRotate()

    {
        gameObject.transform.Rotate(0,0,1 * Time.deltaTime*100);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            AddGold();
            gameObject.SetActive(false);
        }
    }

    public void AddGold()
    {
        GoldCurrency.Gold_Count += 5;
        Debug.Log("AddCoin");
    }
}
