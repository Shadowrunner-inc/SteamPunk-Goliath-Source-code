using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Console_Interactiable : MonoBehaviour {

    float stopWatch, timeLimit;

    [HideInInspector]public bool bIsBeingSabatoged = false;

    public GameObject radialProgressBar, interactiableIndicator;

    ObjectiveSystem _objectiveSystem;

    Toggle myGoaldisplay;

    // Use this for initialization
    void Start() {
        HelloAngel();
        stopWatch = 0f;

        //find the objective manager
        _objectiveSystem = FindObjectOfType<ObjectiveSystem>();
        foreach (Toggle goal in _objectiveSystem.objectiveDisplay) {
            if (goal.GetComponentInChildren<Text>().text == "Stop Disir for Regenerating.") {
                myGoaldisplay = goal;
            }
        }

        gameObject.SetActive(false);//turn this object off
    }



    // Update is called once per frame
    void Update() {

        if (stopWatch >= timeLimit && bIsBeingSabatoged) {
    
            ForcePlayerSwitch();

            interactiableIndicator.SetActive(false);

            gameObject.SetActive(false);
        }

        //tick up the timer while its being worked on.
        else if (bIsBeingSabatoged && stopWatch < timeLimit) {
            stopWatch += Time.deltaTime;

            //math to convert to a precentage
            float percentamount = stopWatch / timeLimit;
           // print("precent Done: " + percentamount);

            //Grab any of the images children
            Image[] childImages = radialProgressBar.GetComponentsInChildren<Image>();
            //run through and find the ones with a image type of filled
            for (int x = 0; x < childImages.Length; x++) {
                if(childImages[x].type == Image.Type.Filled)
                    //print("found:" + childImages[x].name);

                //Assign the amount to their fill amount
                childImages[x].GetComponent<Image>().fillAmount = percentamount; }
        }


    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) {
           // print("YO playa");
            //if your playing as wolf
            if (FindObjectOfType<SwitchCharacters>().charNum == 0) {
                interactiableIndicator.SetActive(true);
               // print("Push the button");
                if (Input.GetKeyDown(KeyCode.F)) {
                    workTime();
                    interactiableIndicator.SetActive(false);
                }
            }
            //otherwise if your playing as sondra
            else if (FindObjectOfType<SwitchCharacters>().charNum == 1) { print("Not wolf"); }


        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) interactiableIndicator.SetActive(false);
    }

    void HelloAngel() {
        //If the angel Ai is not found in the scene send out an error and puase the editor.
        if (FindObjectOfType<Angel_AI>() == null) { Debug.LogError("Angel_AI not found " + name); Debug.Break(); }
        
        Angel_AI my_Boss = FindObjectOfType<Angel_AI>();

       
        my_Boss._consoles.Add(this.GetComponent<Console_Interactiable>());//Regester with the boss
        timeLimit = my_Boss.regenTimeAmount; //Get the amount of time needed to disable the console and store it.
    }

    void ForcePlayerSwitch() {
        //switch the player to wolf and the players ability to switch.
        SwitchCharacters _charChanger = FindObjectOfType<SwitchCharacters>();

        _charChanger.CharacterLock = !_charChanger.CharacterLock; //Toggle the lock.
       // print("Boot");
        _charChanger.ChangeCharacters(); //Switch the characters.

    }

    private void OnDisable()
    {
        radialProgressBar.SetActive(false); //Turn off the display
        bIsBeingSabatoged = false;
        interactiableIndicator.SetActive(false);
        //If you know what your objective display text/Toggle is
        if (myGoaldisplay != null) myGoaldisplay.gameObject.SetActive(false); //Turn off the displayed text objective

        //clean the progress bar fill
        Image[] childImages = radialProgressBar.GetComponentsInChildren<Image>();
        //run through and find the ones with a image type of filled
        for (int x = 0; x < childImages.Length; x++)
        {
            if (childImages[x].type == Image.Type.Filled)
                //print("found:" + childImages[x].name);
                //Assign the amount to their fill amount
                childImages[x].GetComponent<Image>().fillAmount = 0f;
        }
    }

    private void OnEnable()
    {
        //If you know what your objective display text/Toggle is
        if(myGoaldisplay != null)
        myGoaldisplay.gameObject.SetActive(true); //Turn on the objective text display 
        stopWatch = 0f;
    }

    void workTime() {

        ForcePlayerSwitch();
        //Disable wolf's AI
        if (FindObjectOfType<WolfAI>() != null) FindObjectOfType<WolfAI>().enabled = false;

        if (FindObjectOfType<WolfAI2>() != null) FindObjectOfType<WolfAI2>().enabled = false;

        if (FindObjectOfType<WolfAI3>() != null) FindObjectOfType<WolfAI3>().enabled = false;

        bIsBeingSabatoged = true;
        radialProgressBar.SetActive(true); //Turn on the display
        interactiableIndicator.SetActive(false);

    }

}
