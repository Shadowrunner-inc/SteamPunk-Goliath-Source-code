using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Author: Nathan Hales
/// This stores and tracks the players progression throught the level and is used for when they check the pause menu for which objectives they still need to complete.
/// </summary>
public class ObjectiveSystem : MonoBehaviour {

	public enum _Boss
	{
		Genbu,
		Snake_Genbu,
		Forge_Master, // lava hydra
		Disir, // clockwork angel
        Default
	}

	public _Boss currentBoss;

	[Header("Place links to the Pause Screen>Scroll View>Content> Objectives 'X' ")]
	public Toggle[] objectiveDisplay;

	[HideInInspector]
	public List<ObjectiveData> _Objectives;

  /*too much of a hassle just hard code this
   *[Header("0 = genbu, 1 = snake genbu, 2 = Forgemaster, 3 = Disir")]
    public string[] nameOfVictoryScenes;*/

    /// <summary>
    /// Runs through all avalible objectives and updates weather or not they are completed.
    /// </summary>
	public void UpdateObjectives() {

        int numCompleted = 0; //Talles for checking if all objectives are finished

        for (int x = 0; x < _Objectives.Count; x++){
			//Set the toggel to the same as the objectives complete bool.
			objectiveDisplay[x].isOn = _Objectives[x].bIsCompleted;

			//storage string
			string displayText;

			if(_Objectives[x].total > 0){
				//Display the count and total.
				displayText = _Objectives[x].description + _Objectives[x].currentCount + "/" + _Objectives[x].total;
			}
			else{
				//Just show the objective.
				displayText = _Objectives[x].description;
			}

			//Assign the Display string for all to read.
			objectiveDisplay[x].GetComponentInChildren<Text>().text = displayText;
		}

        if (currentBoss == _Boss.Genbu)
        {
            //Talk to Objective system plate has fallen off
            for (int x = 0; x < _Objectives.Count; x++)
            {
                if (_Objectives[x].name == "broken legs")
                {
                    ThighBlaster _blaster = GameObject.FindObjectOfType<ThighBlaster>();
                    _Objectives[x].currentCount = _blaster.numBroken; //set the count to that of the number of broken legs.

                   // print("Numbroken: " + _Objectives[x].currentCount);
                    if (_Objectives[x].currentCount == _Objectives[x].total) { _Objectives[x].bIsCompleted = true; } //Check to see if the goal is finished
                }
                else if (_Objectives[x].name == "armoured plating")
                    {
                        if (_Objectives[x].currentCount == _Objectives[x].total) { _Objectives[x].bIsCompleted = true; } //Check to see if the goal is finished
                    }
            }
        }

        if (currentBoss == _Boss.Snake_Genbu) {
            //find the snake boss's health class
            Boss_Health _HP = FindObjectOfType<Boss_Health>();
            //If the snake is dead set the objective completion to true
            if (_HP.health <= 0f) { _Objectives[0].bIsCompleted = true; }
        }



         foreach (ObjectiveData objective in _Objectives)
         {
                if (objective.bIsCompleted == true)
                {
                    numCompleted++; //add a tick
                }
         }
        
        //when all the obectives are complete. 
        if (numCompleted >= _Objectives.Count)
        {
            //load the next scene.
            //UnityEngine.SceneManagement.SceneManager.LoadScene(GetNextScene());
            GameManager.GM.PlayerWon(currentBoss);

        }
    }

    private void Awake()
    {

        if (GameManager.GM == null)
        {
            GameManager.GM = new GameObject().AddComponent<GameManager>();
            GameManager.GM.gameObject.name = "Game Master";

        }

        GameManager.GM.BossFightStarted(); // tell the GM the fight is being built
    }

    void Start(){
		_Objectives = BuildObjectiveList ();
	}
    
	List<ObjectiveData> BuildObjectiveList(){

		List<ObjectiveData> objectives = new List<ObjectiveData>();

		if(currentBoss == _Boss.Genbu){
			MakeGenbuList(out objectives); 
		}

		else if(currentBoss == _Boss.Snake_Genbu){
			MakeSnakeList(out objectives); 
		}

		else if(currentBoss == _Boss.Forge_Master){
			MakeHydraList(out objectives); 
		}

		else if(currentBoss == _Boss.Disir){
			MakeAngelList(out objectives);
		}


		return objectives;
	}

	void FixedUpdate(){
			UpdateObjectives ();
		
         if(currentBoss == _Boss.Disir){}
	}



	void MakeGenbuList( out List<ObjectiveData> ob_list){
		ob_list = new List<ObjectiveData> ();

		//get genbu's AI
		Genbu_AI _gBoss = GameObject.FindObjectOfType<Genbu_AI>();

		//List<ObjectiveData> objectives = new List<ObjectiveData>();

		//Bolts objective
		ObjectiveData boltObjective = new ObjectiveData();

		boltObjective.name = "Shield bolts";// Use this name to find this later

		boltObjective.description = "Shoot off the bolts holding the armoured plates.";

		int numofBolts = 0;//to stor the sum of the bolts in the scene

		//run through the array and give us the sum of the number of bolts on each plate	
		foreach (int bolt in _gBoss.shieldsBlah){ numofBolts += bolt; }

		boltObjective.total = numofBolts;//Assign

		boltObjective.currentCount = 0;//Start at zero

		boltObjective.bIsCompleted = false;



		//plate objective
		ObjectiveData plateObjective = new ObjectiveData();
		plateObjective.name = "armoured plating";// Use this name to find this later

		plateObjective.description = "Armoured plates Destroyed: ";

		int numofPlates = 0;//to stor the sum of the bolts in the scene

		//run through the array and give us the sum of the number of plate prent in the scen
		foreach (GameObject _plate in _gBoss.shields){ numofPlates++; }

		plateObjective.total = numofPlates;//Assign

		plateObjective.currentCount = 0;//Start at Zero

		plateObjective.bIsCompleted = false;


		//puncture objective
		ObjectiveData punctureObjective = new ObjectiveData();
		punctureObjective.name = "Puncture legs";// Use this name to find this later

		punctureObjective.description = "legs damaged: ";

		punctureObjective.total = 4;//Assign

		punctureObjective.currentCount = 0;//Start at zero

		punctureObjective.bIsCompleted = false;

		// break Legs objective
		ObjectiveData legObjective = new ObjectiveData();

		legObjective.name = "broken legs";// Use this name to find this later

		legObjective.description = "Number of legs destroyed: ";

		legObjective.total = 4;//Assign

		legObjective.currentCount = 0;//Start at zero

		legObjective.bIsCompleted = false;


		//Add our abjectives to the list
		ob_list.Add(boltObjective);
		ob_list.Add(plateObjective);
		ob_list.Add(punctureObjective);
		ob_list.Add(legObjective);
	
		//return ob_list; //Send out the list
	}

	void MakeSnakeList( out List<ObjectiveData> ob_list){
		ob_list = new List<ObjectiveData> ();

		//fight objective
		ObjectiveData fightObjective = new ObjectiveData();

		fightObjective.name = "Defend";// Use this name to find this later

		fightObjective.description = "Defeat the machine.";

		fightObjective.total = 0;//Assign

		fightObjective.currentCount = 0;//Start at zero

		fightObjective.bIsCompleted = false;

		//Add our abjectives to the list
		ob_list.Add(fightObjective);

	}
		
	void MakeHydraList( out List<ObjectiveData> ob_list){
		ob_list = new List<ObjectiveData> ();

		//Head objective
		ObjectiveData headObjective = new ObjectiveData();
		headObjective.name = "Head 1";// Use this name to find this later

		headObjective.description = "Defeat head 1 ";

		headObjective.total = 0;//Assign

		headObjective.currentCount = 0;//Start at 0

		headObjective.bIsCompleted = false;

		//Head2 objective
		ObjectiveData head2Objective = new ObjectiveData();
		head2Objective.name = "Head 2";// Use this name to find this later

		head2Objective.description = "Defeat head 2 ";

		head2Objective.total = 0;//Assign

		head2Objective.currentCount = 0;//Start at 0

		head2Objective.bIsCompleted = false;


		//Head 3 objective
		ObjectiveData headThreebjective = new ObjectiveData();
		headThreebjective.name = "Head 3";// Use this name to find this later

		headThreebjective.description = "Defeat head 3 ";

		headThreebjective.total = 0;//Assign

		headThreebjective.currentCount = 0;//Start at zero

		headThreebjective.bIsCompleted = false;

		//Add our abjectives to the list
		ob_list.Add(headObjective);
		ob_list.Add(head2Objective);
		ob_list.Add(headThreebjective);
         
        print("hydra listed");
	}

	void MakeAngelList( out List<ObjectiveData> ob_list){
		ob_list = new List<ObjectiveData> ();

		//get Angel's AI
		// _aBoss = GameObject.FindObjectOfType<Genbu_AI>();

		//Regen objective
		ObjectiveData regenObjective = new ObjectiveData();

		regenObjective.name = "Regen";// Use this name to find this later

		regenObjective.description = "Stop Disir from Regenerating.";

		regenObjective.total = 0;//Assign

		regenObjective.currentCount = 0;//Start at zero

		regenObjective.bIsCompleted = false;

		//Fight objective
		ObjectiveData fightObjective = new ObjectiveData();
		fightObjective.name = "Fight";// Use this name to find this later

		fightObjective.description = "Defeat Disir.";

		fightObjective.total = 0;//Assign

		fightObjective.currentCount = 0;//Start at 0

		fightObjective.bIsCompleted = false;

		//Add our abjectives to the list
		ob_list.Add(regenObjective);
		ob_list.Add(fightObjective);

	}

    /*
    string GetNextScene()
    {
        string scenename = ""; //Storage

        switch (currentBoss) //Spit out th next scene name
        {
            case _Boss.Genbu:
                scenename = "Load4_P3_Desert";
                break;

            case _Boss.Snake_Genbu:
                scenename = "Load5_Lava";
                print("Next level: " + scenename);
                break;

            case _Boss.Forge_Master:
                scenename = "Level_03_Clockwork_Tower";
                break;

            case _Boss.Disir:
                scenename = "Credits_Screen";
                break;
        }

        return scenename; //Rturn the name for loading
    }*/

}
