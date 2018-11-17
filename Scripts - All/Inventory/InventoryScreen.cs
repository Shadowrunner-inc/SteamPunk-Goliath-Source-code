using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScreen : MonoBehaviour {
    //Find TeamInventory, Define each slots there, and each Image there

    GameObject inventoryPanel;
    GameObject slotPanel;
    public GameObject inventorySlot;
    public GameObject inventoryItem;

    public List<Item> items = new List<Item>();
    public List<GameObject> slots = new List<GameObject>();

    int slotAmount;

    void Start()
    {
        slotAmount = 28;
        inventoryPanel = GameObject.Find("InventoryScreen");
        slotPanel = inventoryPanel.transform.Find("TeamInventory").gameObject;

      /*  for (int i = 0; i < slotAmount; i++)
        {
            slots.Add(Instantiate(inventorySlot));
            slots[i].transform.SetParent(slotPanel.transform);
        }*/
    }

}
