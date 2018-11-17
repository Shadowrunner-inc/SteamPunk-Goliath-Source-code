using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*Author: Nathan Hales
 This Is the master Commander. The Main Manager and the one that exists in every scene. It is used to capture and transmite data to scripts scene to scene. 
 It is central to loading tracking which levels the player has completed and it gets passed the data for tracking the players performance when they finish a level.
 */

public class GameManager : MonoBehaviour{

    public static GameManager GM = null;
    ObjectiveSystem._Boss lastBoss = ObjectiveSystem._Boss.Default;

    public Preformance_Data _preData;

    string[] LevelNames;

    public ObjectiveSystem._Boss defeatedBoss { get { return lastBoss; } }

    private void Awake()
    {
        //If the You are the only GM assign you self as the one to rule all.
        if (GM == null)
        {
            GM = this;
        }
        //If your a copy... Die
        else if(GM != this.gameObject) Destroy(gameObject);

        //Only fools get destroyed beween loads
        DontDestroyOnLoad(this);
    }

    public void PlayerWon(ObjectiveSystem._Boss _currentBoss) {
        lastBoss = _currentBoss;
        resulttime();
    }

 
   public void NextBoss()
    {
        
        string scenename = ""; //Storage

        switch (defeatedBoss) //Spit out th next scene name
        {
            case ObjectiveSystem._Boss.Genbu:
                scenename = "Load4_P3_Desert";
                break;

            case ObjectiveSystem._Boss.Snake_Genbu:
                scenename = "Load5_Lava";
                print("Next level: " + scenename);
                break;

            case ObjectiveSystem._Boss.Forge_Master:
                scenename = "Level_03_Clockwork_Tower";
                break;

            case ObjectiveSystem._Boss.Disir:
                scenename = "Credits_Screen_Jeff_2D";
                break;
            case ObjectiveSystem._Boss.Default:
                scenename = "Menu_Main";
                break;

        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(scenename);
    }

    


    public void resulttime() {
        //Go to Results
        UnityEngine.SceneManagement.SceneManager.LoadScene("ResultScreen");
    }

    public void BossFightStarted()
    {
            GameObject  myTracker = new GameObject();
            myTracker.AddComponent<PreformanceTracker>();
            myTracker.name = "Preformance_Tracking_System";
    }

    public  void ToStore()
    {
        Time.timeScale = 1f;
            UnityEngine.SceneManagement.SceneManager.LoadScene("Store_Menu"); 
    }
    public  void ToMainMenu()
    {
        Time.timeScale = 1f;
        lastBoss = ObjectiveSystem._Boss.Default;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu_Main");
    }

    public void ToEnding() {

        Time.timeScale = 1f;
        NextBoss();
        lastBoss = ObjectiveSystem._Boss.Default;
    }

}
