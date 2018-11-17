using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Author: Nathan Hales
// Communitacte with the inventory manger to track the number of potions the player has.

[RequireComponent(typeof(Text))]
public class Potion_Counter : MonoBehaviour {



    private void Start()
    {
        if (InventoryManager.instance == null)
        {
            InventoryManager.instance = new GameObject().AddComponent<InventoryManager>();
            InventoryManager.instance.gameObject.name = "Inventory Master";

        }

    }

    void LateUpdate()
    {
        ChangePotionCount();
    }


    void ChangePotionCount()
    {
         Text potionText = GetComponent<Text>();
        potionText.text = "x" + InventoryManager.instance.healthPotions;
    }


}
