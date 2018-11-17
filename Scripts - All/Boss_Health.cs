using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss_Health : MonoBehaviour {

    [HideInInspector]
    public bool immune;
    public int health = 1000;
	public UnityEngine.UI.Slider _healthBar;//healthBar
    //public string nextScene = "Menu_Main";

	void Start(){
		if (_healthBar != null) {
			_healthBar.maxValue = health;
			_healthBar.value = _healthBar.maxValue;
		}

        /*if (gameObject.GetComponent<Hydra_AI>())
        {
            hydraAI = GetComponent<Hydra_AI>();
        }
        */
	}

    void Update()
    {
        /*Handeled by the Objective System Script.
         if (hydraAI != null && hydraAI.remainingHydras == 0 || (hydraAI == null && health <= 0))
        {
            SceneManager.LoadScene(nextScene);
        }*/
    }

    public void Regen(int amount)
    {
        health += amount;

        if (_healthBar != null)
            _healthBar.value = (float)health;
    }

    public void TakeDamage(int amount)
    {
        if (!immune)
        {
            health -= amount;
        }

		if(_healthBar != null)
			_healthBar.value = (float)health;
		
        if (health <= 0)
        {
			if(_healthBar != null)
				_healthBar.fillRect.gameObject.SetActive(false);

           /*Interferrece with the Hydra's Death Logic
            if (gameObject.GetComponent<Hydra_AI>())
                gameObject.GetComponent<Hydra_AI>().enabled = false;
                */
            if (gameObject.GetComponent<Genbu_AI>())
                gameObject.GetComponent<Genbu_AI>().enabled = false;
			if (gameObject.GetComponent<Snake_Genbu_AI>())
				gameObject.GetComponent<Snake_Genbu_AI>().enabled = false;
            //if (gameObject.GetComponent<Angel_AI>())
            //    gameObject.GetComponent<Angel_AI>().enabled = false;
        }
    }
}
