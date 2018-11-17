using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDisplay : MonoBehaviour {

    public ItemBlock blockprefab;

    private void Start()
    {
        Display();
    }

    public void Display()
    {
        //cleanup UI each times when you load the store
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
            


        foreach (ItemEntry item in XMLManager.ins.ItemDB.list)
        {
            ItemBlock newBlock = Instantiate(blockprefab) as ItemBlock;
            newBlock.transform.SetParent(transform,false);
            newBlock.Display(item);
        } 

	}
 
}
