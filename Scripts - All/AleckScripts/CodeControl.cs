using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeControl : MonoBehaviour {

	[SerializeField] Rect textRect;
	string displayText = "Press F to use";
	[SerializeField] GUIStyle displayStyle;

	public MonoBehaviour controls;
	public Transform cameraCam;
	public GiantCrossbow crossbow;
    public SonVoiceCon sonVoice;
	public bool canSee = false;

	// Use this for initialization
	void Start ()
    {
        sonVoice = GetComponent<SonVoiceCon>();
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit interactable;

		if(Physics.Raycast(cameraCam.position, cameraCam.forward, out interactable, 30.0f)){
			
			if (interactable.transform.GetComponent<GiantCrossbow> ()) {
				canSee = true;

			} else if (interactable.transform.GetComponent<ZipLine> ()) {
				canSee = true;

			} else {
				canSee = false;
			}
		}

		if (Input.GetKeyUp (KeyCode.F)){
			if (controls.enabled && crossbow == null) {
				
				RaycastHit hit;
					
				if (Physics.Raycast (cameraCam.position, cameraCam.forward, out hit, 30.0f)) {

					crossbow = hit.transform.GetComponent<GiantCrossbow> ();

					if (crossbow != null) {
						crossbow.madeActive = controls;
						crossbow.isUsed = true;
						controls.enabled = false;
						crossbow.cam = hit.transform.GetComponentInChildren<Camera>();

					}
					ZipLine zippy = hit.transform.GetComponent<ZipLine>();
					if (zippy != null)
					{
						zippy.Traveller(this.transform);
						zippy.travelling = true;
						controls.enabled = false;
						zippy.justActivated = true;
					}

					BoulderInteract bold = hit.transform.GetComponent<BoulderInteract> ();

					if(bold != null){
						bold.Launch();
					}


					GasValve gassy = hit.transform.GetComponent<GasValve> ();

					if(gassy != null){
						gassy.ActivateGas ();
                        sonVoice.GasVoice();
                        print("Something");
                    }

				}

            }

            
            else {
				crossbow = null;
				controls.enabled = true;
			}

            

        }


		
	}//end of Update

	private void OnGUI()
	{
		//if the player is near the valve display the text
		if (canSee) {
			Rect calcRect = new Rect();//placeholder 

			//Make our calculations to be a proportion of the screen
			calcRect.x = textRect.x * Screen.width;
			calcRect.width = textRect.width * Screen.width;
			calcRect.y = textRect.y * Screen.height;
			calcRect.height = textRect.height * Screen.height;

			GUI.Label(calcRect, displayText, displayStyle);//Assign and draw

		}
	}

	public void ReturnControl(){
		
		controls.enabled = true;
	}
}
