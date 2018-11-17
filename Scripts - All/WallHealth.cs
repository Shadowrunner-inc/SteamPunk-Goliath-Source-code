using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Author: Nathan Hales
// Tracks and Triggers the destruction event for the fail state in the Second level(Snake Genbu boss fight.)

public class WallHealth : MonoBehaviour {

	public float wallMaxHealth;
	public BreakWall[] m_BreakWall;
	public GameObject centeralWall;
	public Slider _healthBar;

	public string loseScene;

	void Start(){
		_healthBar.maxValue = wallMaxHealth;
		_healthBar.value = _healthBar.maxValue;
	}

	public	void TakeDamage(int damage){
		_healthBar.value -= damage;//Decrease the slider

		if (_healthBar.value <= 0f){
			for (int i = 0; i < transform.childCount; i++) {
				transform.GetChild (i).gameObject.SetActive (false);
			}

			_healthBar.fillRect.gameObject.SetActive (false);//Turn off the slider gameobject


			foreach (BreakWall wall in m_BreakWall) {
				wall.gameObject.SetActive (true);//Turn on the Breaking model game object
				wall.enabled = true;//Turn on the Destruction!!!
				wall.Wall_break = true;
			}
			StartCoroutine (Lose ());
		}
		return;
	}

	IEnumerator Lose()
	{
		yield return new WaitForSeconds (3);
		SceneManager.LoadScene (loseScene);
	}

}
