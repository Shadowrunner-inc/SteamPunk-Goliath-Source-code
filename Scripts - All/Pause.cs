using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// Author: Joseph koroma
///	Edited by: Nate hales 7/5/18
///  Pause in-game functionality.


[RequireComponent(typeof(ObjectiveSystem))]
public class Pause : MonoBehaviour {

    public bool pause;
    public CameraTargetController cam;

    private GameObject[] players;

	public GameObject pauseScreen;
   // public Text pauseText;

   // public string ExitScene;

    private float timer = 0;


	private ObjectiveSystem _objectiveSystem;

	// Use this for initialization
	void Start () {
		pauseScreen.SetActive(false);
      //  pauseText.enabled = false;
        players = GameObject.FindGameObjectsWithTag("Player");

		//Get your objective system
		_objectiveSystem = GetComponent<ObjectiveSystem>();
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            timer = 0;
            if (!pause)
            {
				PauseGame ();
            }

            else if (pause)
            {
				ResumeGame ();
            }
        }

        if (pause) {
            Cursor.visible = true; //Show the mouse cursor
            Cursor.lockState = CursorLockMode.None; // Unlock the mouse posistion
        }
        else {
            Cursor.visible = false; //Hide the mouse cursor       
            Cursor.lockState = CursorLockMode.Locked; //Lock it to the center of the screen
        }

    }

	public void PauseGame(){
		pause = true;
		Time.timeScale = 0.0f;
		pauseScreen.SetActive(true);

		_objectiveSystem.UpdateObjectives ();

		//pauseText.enabled = true;

	
		foreach (GameObject p in players)
		{
			if (p.GetComponent<WolfMovement>() && cam.controlledChar == 0)
			{
				p.GetComponent<WolfMovement>().enabled = false;
			}

			if (p.GetComponent<SondraMovement>() && cam.controlledChar == 1)
			{
				p.GetComponent<SondraMovement>().enabled = false;
			}

			if (p.GetComponent<WolfAI>())
			{
				p.GetComponent<WolfAI>().enabled = false;
			}
		}

		if (cam != null)
			cam.enabled = false;

        

    }


    public void ResumeGame(){
		pause = false;
		Time.timeScale = 1.0f;
		pauseScreen.SetActive(false);

		//pauseText.enabled = false;


		foreach (GameObject p in players)
		{
			if (p.GetComponent<WolfMovement>() && cam.controlledChar == 0)
			{
				p.GetComponent<WolfMovement>().enabled = true;
			}

			if (p.GetComponent<SondraMovement>() && cam.controlledChar == 1)
			{
				p.GetComponent<SondraMovement>().enabled = true;
			}

			if (p.GetComponent<WolfAI>() && cam.controlledChar == 1)
			{
				p.GetComponent<WolfAI>().enabled = true;
			}
		}

		if (cam != null)
			cam.enabled = true;

       

	}

    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}
