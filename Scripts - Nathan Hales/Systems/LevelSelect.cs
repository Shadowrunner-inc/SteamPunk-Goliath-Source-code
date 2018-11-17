using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*Author: Nathan Hales
Holds an array of the levels for the level selection submenu. Adds resizing fuctionality of the level buttons for additional visual feedback to the players.
 */

public class LevelSelect : MonoBehaviour {

    public float newWidth, newHeight; //decleare new hight and width.
	public string[] levelNames; // Level list


    public Button[] levelButtons; // UI level button list
	public Texture[] backgroundImages; // Level art list for menu background
    public GameObject ImageOnPanel; // GUI panel 

	//public GameObject mM, levelMenu, startBtn, quitBtn, rtnBtn;
	public float fadeDur, fadeAlp;
	public bool timeScale;

	//public GameObject loadingScreen;
	//public Slider progressBar;

    RawImage setImage; // selected backgorund art
    int selectedLevel; // level numbers
    float oldWidth, oldHeight; //decleare old hight and width.

    public bool[] unlockedLevel; //the levels that are accessable currently.

    void Start (){
        setImage = (RawImage)ImageOnPanel.GetComponent<RawImage>(); // Sets up Raw image texture for targeting.
       
        // Grab the original sizes of the first level button for resizing later
        oldHeight = levelButtons[0].GetComponent<LayoutElement>().preferredHeight;  
        oldWidth = levelButtons[0].GetComponent<LayoutElement>().preferredWidth;

        //Set level 1 as the default
        onlevelSelect(1);

    }

	bool intheArray(Button testing){
		foreach (Button button in levelButtons) {
			if (button == testing) {
				return true;
			}
		}
		return false;
	}

	public void onlevelSelect(int level){

        //Chek to make sure the selection is not out of range.
	    if (levelNames[level-1] == null || levelNames[level-1] == "")
	    {
            Debug.LogError("selection of level: " + level + " Is not unavailable");
	        return;
	    }

	    foreach (Button button in levelButtons) {
			LayoutElement element = button.GetComponent<LayoutElement> ();
			element.preferredHeight = oldHeight; // changes height of target button.
			element.preferredWidth = oldWidth; // changes width of target button.
			selectedLevel = level;
			backgroundChanger ();
			//Debug.Log("Selected Level: " + levelNames[selectedLevel-1]);
		}
		LayoutElement element2 = levelButtons [selectedLevel-1].GetComponent<LayoutElement> ();
		element2.preferredHeight = newHeight; // changes height of target button.
		element2.preferredWidth = newWidth; // changes width of target button.
		selectedLevel = level;
	}

	public void launchLevel(){
		//Debug.Log("Loading Level: " + levelNames[selectedLevel-1]);
		
		//StartCoroutine(loadingLevel(selectedLevel)); // loads selected level
        LoadLevel(selectedLevel);
	}


	void backgroundChanger(){
        setImage.texture = backgroundImages[selectedLevel-1]; // Changes panel's raw image.source image to selected level.
		//Debug.Log ("Background Changed.");
	}

/*
IEnumerator fadeTimer(){
	//Debug.Log ("fading.");
	yield return new WaitForSeconds (fadeDur);
		rtnBtn.SetActive (false);
		startBtn.SetActive(true);
	}
    */
    /*
	IEnumerator loadingLevel(int level){
		AsyncOperation async = SceneManager.LoadSceneAsync (level);

		while (!async.isDone) {
			progressBar.value = async.progress / 0.9f ;
			yield return new WaitForEndOfFrame();
		}
	}*/

    void LoadLevel(int level)
    {
        
        SceneManager.LoadScene(levelNames[level-1]);
    }


    public void ChangeLevelAccess(int levelNum, bool bIsUnlocked) {
        
        //TODO: add Error logging

        levelButtons[levelNum - 1].gameObject.SetActive(bIsUnlocked);   }

}
