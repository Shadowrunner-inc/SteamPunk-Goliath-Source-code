using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

	public Health sondraHealthRef;
	public Health wolfHealthRef;
	public Energy wolfFlyRef;

	public Image sonHealthBar;
	public Image wolfHealthBar;
	public Image wolfFlyBar;

    public CameraTargetController camTarCon;

	public Image sonIconOne;
	public Image sonIconTwo;
	public Image wolfIconOne;
	public Image wolfIconTwo;

	public bool isSon = false;
	public bool sondraUI = false;
	public bool wolfUI = true;

	public float wolfFlyMeter;
	public float wolfFlyMeterMax;
	public int sondraHealth;
	public int sondraMaxHealth;
	public int wolfHealth;
	public int wolfMaxHealth;


	// Use this for initialization
	void Start () {
		sondraMaxHealth = sondraHealthRef.maxHealth;
		wolfMaxHealth = wolfHealthRef.maxHealth;
		wolfFlyMeterMax = wolfFlyRef.maxEnergy;

	}
	
	// Update is called once per frame
	void Update () {

		sondraHealth = sondraHealthRef.health;
		wolfHealth = wolfHealthRef.health;
		wolfFlyMeter = wolfFlyRef.energy;

        if (camTarCon.controlledChar == 0)
        {
            isSon = false;
        }

        else if (camTarCon.controlledChar == 1)
        {
            isSon = true;
        }

		if (isSon) {
			
			if(!sondraUI){
				wolfHealthBar.enabled = false;
				sonHealthBar.enabled = true;
				wolfUI = false;
				sondraUI = true;
				sonIconOne.enabled = true;
				sonIconTwo.enabled = true;
				wolfIconOne.enabled = false;
				wolfIconTwo.enabled = false;

			}


			sonHealthBar.fillAmount = sondraHealth / (float)sondraMaxHealth;

		} else {
			
			if(!wolfUI){
				
				sonHealthBar.enabled = false;
				wolfHealthBar.enabled = true;
				sondraUI = false;
				wolfUI = true;
				sonIconOne.enabled = false;
				sonIconTwo.enabled = false;
				wolfIconOne.enabled = true;
				wolfIconTwo.enabled = true;
			}


			wolfHealthBar.fillAmount = wolfHealth / (float)wolfMaxHealth;
			wolfFlyBar.fillAmount = wolfFlyMeter / wolfFlyMeterMax;
		}
	}
}
