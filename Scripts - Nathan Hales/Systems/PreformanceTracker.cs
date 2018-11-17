using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Nate Hales
//THis Will keep teack of the players preformance for comparasion later.

public class PreformanceTracker : MonoBehaviour {
    
    private float completionTime = 0;
    private float hpLeft = 0;

   
    [HideInInspector]public float maxHp;

    void CalculatedamageTaken() {
        Health wolf = null;
        Health sondra = null;

        //Find the characters health scripts
        Health[] HealthObjects = FindObjectsOfType<Health>();
        foreach (Health foundHP in HealthObjects) {
            //Run through to grab the right characters.
            if (foundHP.gameObject.CompareTag("Player") && foundHP.gameObject.GetComponent<SondraMovement>() != null)
            {
                sondra = foundHP;
               // print("Sondra hp found");
            }
            else if (foundHP.gameObject.CompareTag("Player") && foundHP.gameObject.GetComponent<WolfMovement>() != null)
            {
                wolf = foundHP;
               // print("Wolf Hp Found");
            }
        }

        if (wolf != null && sondra != null) {
           // print("Both hp found");
            hpLeft = wolf.health + sondra.health;
            maxHp = wolf.maxHealth + sondra.maxHealth;
        }
    }

    private void Update()
    {
        completionTime += Time.deltaTime;
    }

    private void LateUpdate()
    {
        CalculatedamageTaken();
    }

    private void OnDestroy()
    {
        GameManager.GM._preData.healthLeft = hpLeft;
        //print("Hp left: " + hpLeft);
        GameManager.GM._preData.completionTime = completionTime;
        GameManager.GM._preData.maxHP = maxHp;
        //print("Max Hp: " + maxHp);
    }
}
