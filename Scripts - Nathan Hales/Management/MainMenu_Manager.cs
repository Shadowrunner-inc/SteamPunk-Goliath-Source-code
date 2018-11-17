using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Author: Nate Hales
/// Manages which sub menus are displayed during the main menu scene. This acts as a tree for other scripts to call fuctions to turn things on and off in the scene.
/// </summary>

public class MainMenu_Manager : MonoBehaviour
{

    public GameObject mainMenuScreen, levelSelectScreen;
	public GameObject wolfControlScreen, sondraControlScreen;

    public string creditstSceneName;


    public void MMStart()
    {
        TurnOffScreens();
        levelSelectScreen.SetActive(true);
    }

    public void Controls()
    {
        TurnOffScreens();
		wolfControlScreen.SetActive(true);
    }

    public void Credits()
    {
        TurnOffScreens();
        UnityEngine.SceneManagement.SceneManager.LoadScene(creditstSceneName);
    }

    public void Back()
    {
        TurnOffScreens();
        mainMenuScreen.SetActive(true);
    }

    void TurnOffScreens()
    {
        levelSelectScreen.SetActive(false);
		wolfControlScreen.SetActive(false);
		sondraControlScreen.SetActive(false);
        mainMenuScreen.SetActive(false);
    }

	public void SwitchControlScreen(){
		if(wolfControlScreen.activeSelf == true){
			wolfControlScreen.SetActive(false);
			sondraControlScreen.SetActive(true);
		}
		else{
			wolfControlScreen.SetActive(true);
			sondraControlScreen.SetActive(false);
		}

	}

    public void QuitGame() { Application.Quit(); }
}
