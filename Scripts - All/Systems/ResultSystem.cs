using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Author: Nathan Hales
/// This Tacks the data from the Game Manager that was collected during the fight and grades the players performance compared to the average performance of our playtesters and dev's.
/// </summary>

public class ResultSystem : MonoBehaviour {


    public GameObject ResultScreen;


    public Text compTimeText, Grade, Rewardtext;
    public Slider healthLoseBar;

    ObjectiveSystem._Boss _Boss;

    Button[] buttons;

    void Start()
    {
        if (GameManager.GM == null)
        {
            GameManager.GM = new GameObject().AddComponent<GameManager>();
            GameManager.GM.gameObject.name = "Game Master";

        }

        _Boss = GameManager.GM.defeatedBoss;

        print("" + _Boss);

        ShowResult();

        buttons = FindObjectsOfType<Button>();
        foreach (Button click in buttons) {
            if (GameManager.GM.defeatedBoss == ObjectiveSystem._Boss.Disir)
            {
                if (click.GetComponentInChildren<Text>().text == "Store" || click.GetComponentInChildren<Text>().text == "Main Menu") click.gameObject.SetActive(false);
            }
            else {
                if (click.GetComponentInChildren<Text>().text == "Next") click.gameObject.SetActive(false);
            }
        }
    }


     char CalculateGrade()
    {

        float HPgrade = GameManager.GM._preData.healthLeft / GameManager.GM._preData.maxHP;

        HPgrade = (HPgrade * 100f) * 2;

        print("Health Grade: " + HPgrade);

        float timeGrade = (TimeComparrasion()*100f) *2f;// Calculate the time grade depending on the boss fought

        print("Time Grade: " + timeGrade);

        float gradeNum = (HPgrade + timeGrade);

        print("Grade number: " + gradeNum);

        if (299f >= gradeNum && gradeNum >= 200f) {
            GoldCurrency.Gold_Count += 100;
            Rewardtext.text = Rewardtext.text + 100;
            return 'A'; }
        else if (199f >= gradeNum && gradeNum >= 150f) {
            GoldCurrency.Gold_Count += 75;
            Rewardtext.text = Rewardtext.text + 75;
            return 'B'; }
        else if (149f >= gradeNum && gradeNum >= 100f) {
            GoldCurrency.Gold_Count += 50;
            Rewardtext.text = Rewardtext.text + 50;
            return 'C'; }
        else if (99f >= gradeNum && gradeNum >= 50f) {
            GoldCurrency.Gold_Count += 25;
            Rewardtext.text = Rewardtext.text + 25;
            return 'D'; }
        else if(gradeNum >= 300f) {
            GoldCurrency.Gold_Count += 200;
            Rewardtext.text = Rewardtext.text + 200;
            return 'S'; }
        else {
            GoldCurrency.Gold_Count += 5;
            Rewardtext.text = Rewardtext.text + 5;
            return 'E'; }

    }


    void ShowResult()
    {
   
       compTimeText.text = compTimeText.text + string.Format("{0}:{1:00}", (int)GameManager.GM._preData.completionTime / 60, (int)GameManager.GM._preData.completionTime % 60);

        healthLoseBar.maxValue = GameManager.GM._preData.maxHP;

        healthLoseBar.value = GameManager.GM._preData.healthLeft;
       // print("HP left: " + GameManager.GM._preData.healthLeft);
       

        Grade.text = "" + CalculateGrade();
        ResultScreen.SetActive(true);

    }




    public void CallMainMenu() {
        GameManager.GM.ToMainMenu();
    }

    public void CallStore() { GameManager.GM.ToStore(); }

    public void CallEnding() { GameManager.GM.ToEnding(); }

    float TimeComparrasion() {
        float CalcTime = 0f;

        switch (_Boss)
        {
            case ObjectiveSystem._Boss.Genbu:
                CalcTime = GameManager.GM._preData.completionTime / 300f;
                break;
            case ObjectiveSystem._Boss.Snake_Genbu:
                CalcTime = GameManager.GM._preData.completionTime / 240f;
                break;
            case ObjectiveSystem._Boss.Forge_Master:
                CalcTime = GameManager.GM._preData.completionTime / 300f;
                break;
            case ObjectiveSystem._Boss.Disir:
                CalcTime = GameManager.GM._preData.completionTime / 180f;
                break;
            case ObjectiveSystem._Boss.Default:
                print("you fought no one!");
                break;
        }
        
        return 1f - CalcTime; //Closer to zero the better
    }
}
