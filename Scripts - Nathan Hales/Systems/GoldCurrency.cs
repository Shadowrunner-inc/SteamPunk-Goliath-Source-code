using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Author: Lance
/// Edited By: Nathan Hales
/// main class for keeping track of player coins
/// </summary>
public class GoldCurrency : MonoBehaviour {

    public Text Gold_Text;

    public static int Gold_Count = 100;


    private void Update()
    {
        ShowGold();
    }

    private void ShowGold()
    {
        Gold_Text.text = "Gold : "+ Gold_Count;
    }
}

