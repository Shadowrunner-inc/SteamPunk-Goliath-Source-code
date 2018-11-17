using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GiantCrossbow : MonoBehaviour {
	//Author: Alecksandar Jackowicz
	//When it is used, disables the player controller and takes the camera.
	//Using the mouse horizontally will rotate the base of the mounted crossbow left and right 
	//Using the mouse Vertically will pivot the limb of the crossbow up and down
	public MonoBehaviour madeActive;

	public Transform horizontalPivot;
	public Transform verticalPivot;

	public float horizontalSpeed = 20.0f;
	public float verticalSpeed = 20.0f;
	public float reloadTime = 2.0f;
	private float reloadTick = 0.0f;

	public bool isUsed = false;
	public bool readyToShoot;

	public int arrowCount;

	//public Text ammoText;

	public GameObject arrow;
	public Camera cam;
	public Camera playerCamera;
	// Use this for initialization
	void Start () {
		readyToShoot = true;
		CameraOff ();
	}
	
	// Update is called once per frame
	void Update () {

		//When the ballista is being used do the following
		if(isUsed){
			//Turn on ammo ui text and change it to ammo count

			//tick down the timer
			if(reloadTick > 0){
				reloadTick -= Time.deltaTime;
			}
			else if(reloadTick < 0){
				//Ready to shoot when the timer runs out
				readyToShoot = true;
			}

			//Make a horizontal and vertical variable that will change based on the speed and mouse input
			float h = horizontalSpeed * Input.GetAxis ("Mouse X") * Time.deltaTime;
			float v = verticalSpeed * Input.GetAxis ("Mouse Y") * Time.deltaTime;

			//Rotate the assigned pivots based on the inputs previously created
			//Notice: The imported models have weird rotations that require different movement
			//print("z: " + horizontalPivot.localRotation.eulerAngles.z);
			horizontalPivot.Rotate (0, 0, h);

			//print("z: " + horizontalPivot.localRotation.eulerAngles.z + ", y: " + verticalPivot.localRotation.eulerAngles.y);
			verticalPivot.Rotate (0, -v, 0);


			//On Left Click and arrow prepared, Shoot Ballista
			if(Input.GetMouseButtonDown(0) && readyToShoot && arrowCount > 0){
				//Shoot Arrow
				//print("shot");
				Transform shot = Instantiate (arrow, verticalPivot.position + verticalPivot.right * 2, verticalPivot.rotation).transform;
				shot.forward = verticalPivot.right;
				//Remove arrow from arrow count
				arrowCount -= 1;
				//not ready to shoot and activate Timer
				readyToShoot = false;
				reloadTick = reloadTime; 
			}

			if(Input.GetKeyDown (KeyCode.F)){
				isUsed = false;
			}
			if (isUsed) 
			{
				madeActive.enabled = true;
				cam.enabled = true;
				playerCamera.enabled = false;
			}

			if (isUsed == false) 
			{
				CameraOff ();
				cam.enabled = false;
				playerCamera.enabled = true;
			}
		}
	}

	public void CameraOn(){
		cam.enabled = true;
	}

	public void CameraOff(){
		cam.enabled = false;
	}


}
