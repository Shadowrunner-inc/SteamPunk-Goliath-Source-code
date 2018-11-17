using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActivateHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public RotateMe[] rotator;

	public void OnPointerEnter(PointerEventData eventData)
	{
		for(int i = 0; i < rotator.Length; i++){
			rotator[i].enabled = true;
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		for(int i = 0; i < rotator.Length; i++){
			rotator[i].enabled = false;
		}
	}
}
