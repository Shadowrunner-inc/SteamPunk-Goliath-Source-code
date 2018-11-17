using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlots : MonoBehaviour, IDropHandler{

    public GameObject m_Item ;
    public AudioSource m_audio;

    void Start()
    {
        if (InventoryManager.instance == null)
        {
            InventoryManager.instance = new GameObject().AddComponent<InventoryManager>();
            InventoryManager.instance.gameObject.name = "Inventory Master";
            m_audio = gameObject.GetComponent<AudioSource>();
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        print("OnDrop");
        ItemDragHandler _item = ItemDragHandler.itemBeingDragged.GetComponent<ItemDragHandler>();
        if (_item.price <= GoldCurrency.Gold_Count && _item.itemWorks == true /*&& is not another slot. */) {
            GoldCurrency.Gold_Count -= _item.price;
            InventoryManager.instance.healthPotions++;
            _item.price = 0;
            ItemDragHandler.itemBeingDragged.transform.SetParent(transform);

            m_audio.Play();
        }
    }
}
