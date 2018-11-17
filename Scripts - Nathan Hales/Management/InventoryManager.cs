using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Author: Nathan Hales
/// Tracks and stored data for the players inventory. 
/// adds store fuctionality, and calls the game manager when the player disres to change to another scene.
/// </summary>

public class InventoryManager : MonoBehaviour {


    public static InventoryManager instance = null;
    private int PotionNum = 3;
    public int healthPotions { get { return PotionNum; } set {
             PotionNum = value; } }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void CallMainMenu() {
        if (GameManager.GM == null) { GameManager.GM = new GameObject().AddComponent<GameManager>();
            GameManager.GM.gameObject.name = "Game Master";
        }
        
            GameManager.GM.ToMainMenu(); 
    }

    public void CallNextLvl() {
        if (GameManager.GM == null)
        {
            GameManager.GM = new GameObject().AddComponent<GameManager>();
            GameManager.GM.gameObject.name = "Game Master";
        }

        GameManager.GM.NextBoss();
    }
}



public enum Items
{
    Genbu_Gear,
    Lava_Crystal,
    Power_Battery,
    
    Healing_Potion,
    Defense_Potion,
    Attack_Potion,
    Fireball_Scroll,
}

[System.Serializable]
public class ItemProperty
{
    public string itemName;
    public int itemCode;
    public int goldValue;

    public Image itemIcon;
    public string itemDescription;
}

