//Author: Nate Hales
//Builds a gui Display when the player stays with in the trigger, watch for a button press, and stops drawing the Gui when the player leaves the space.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureValve : MonoBehaviour {
    [SerializeField] Rect textRect;
    string displayText = "Press 'F'";
    [SerializeField] GUIStyle displayStyle;
	[SerializeField] SteamPipes _steamPipe;

    public bool bIsNear = false;

    private void Start() {
        bIsNear = false;
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Player")) {
            bIsNear = true;
        }
    }
    private void OnTriggerExit(Collider other){
        if (other.CompareTag("Player")){
            bIsNear = false;
        }
    }

    private void OnGUI()
    {
        //if the player is near the valve display the text
        if (bIsNear) {
            Rect calcRect = new Rect();//placeholder 

            //Make our calculations to be a proportion of the screen
            calcRect.x = textRect.x * Screen.width;
            calcRect.width = textRect.width * Screen.width;
            calcRect.y = textRect.y * Screen.height;
            calcRect.height = textRect.height * Screen.height;

            GUI.Label(calcRect, displayText, displayStyle);//Assign and draw
			if (Input.GetKeyDown(KeyCode.F)) {
				_steamPipe.bIsArmed = true; // arm the assign Steam pipe
				enabled = false; //Disable yourself 
            }
        }
    }
}
