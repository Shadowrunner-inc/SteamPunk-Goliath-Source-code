using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour,IBeginDragHandler,IDragHandler, IEndDragHandler {

    public static GameObject itemBeingDragged;
    Vector3 StartPosition;
    Transform StartParent;
    public bool isHeld = false;
    public bool itemWorks = false;
    public int price;

    public void OnBeginDrag(PointerEventData eventData)
    {
        isHeld = true;
        itemBeingDragged = gameObject;
        StartPosition = transform.position;
        StartParent = transform.parent;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

  public void OnDrag(PointerEventData eventData)
    {
        
        transform.position = Input.mousePosition;
        print(Input.mousePosition);
        
    }



    public void OnEndDrag(PointerEventData eventData)
    {
        print("OnEndDrag");
        isHeld = false;
       GetComponent<CanvasGroup>().blocksRaycasts = true;

        transform.localPosition = Vector3.zero;

        itemBeingDragged = null;

        if (transform.parent == StartParent)
        {
            transform.position = StartPosition;
        }

        

    }

}
