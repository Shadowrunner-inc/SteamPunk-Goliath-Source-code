//Author: Nate Hales
//Simple Script to be attacted to objects that can be destroyed by damaging them.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableScript : MonoBehaviour {

    public int objHealth = 3;
    [SerializeField] GameObject DestructionVFX;

    public void TakeDamage(int damageRecivied) {
        objHealth -= damageRecivied;
        if (objHealth <= 0) { Destroy(gameObject); }
    }

    private void OnDestroy()
    {
        if(DestructionVFX!=null) Instantiate(DestructionVFX);
    }

}
