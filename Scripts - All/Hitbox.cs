using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour {

    public bool ignoreCamController;
    public int damage; //How much damage does the Hitbox do?
    public bool disappearAfterWhile; //Will this gameObject disappear after a set amount of time?
    public bool consecutiveDamage; //Does this gameObject deal consecutive damage or just once
    public float disappearTime = 1.5f; //If "disappearAfterWhile" is set to true, how long until it does disappear?

    public bool canStun = true;
    public bool noBlock = false;
    public float stunTime = 1;

    private float timer;
    private bool damagePause;

    private GameObject camHolder;

    private bool stopIt;

    // Use this for initialization
    void Start() {

        //===========================
        canStun = true;
        stunTime = 0.35f;
        //===========================


        //Look for Camera
        GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");

        //Due to how the camera's laid out in the inspector, we have to get its real camHolder
        camHolder = cam.transform.parent.parent.gameObject;
    }

    bool IsThisWolf(string wolf)
    {
        if (wolf == "Wolf" || wolf == "WolfL1" || wolf == "WolfL2" || wolf == "WolfL3")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool IsThisSondra(string sondra)
    {
        if (sondra == "Sondra" || sondra == "SondraL1" || sondra == "SondraL2" || sondra == "SondraL3")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Update()
    {
        //If this boolean is set to true, the gameObject with this script will self destruct from the scene after a set amount of time
        if (disappearAfterWhile)
        {
            timer += Time.deltaTime;
            if (timer > disappearTime)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Used to stop the hitbox from dealing too much damage to each individual Gates
        if (other.gameObject.tag == "Gate" && !stopIt)
        {
            WallHealth _wHp = FindObjectOfType<WallHealth>();
            StartCoroutine(DamageWall(_wHp));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && GetComponent<Hitbox>().isActiveAndEnabled
            && (
                    (
                        ((camHolder.GetComponent<CameraTargetController>().controlledChar == 1
                            && (IsThisSondra(other.transform.parent.parent.name))
                           )
            
                        || (camHolder.GetComponent<CameraTargetController>().controlledChar == 0
                            && (IsThisWolf(other.transform.parent.parent.name)))
                        )
                     ||      ignoreCamController
                    )
                )
           )
        {
            Debug.Log("I hit you");
			if (!damagePause)
            {
                StartCoroutine(DamagePlayer(other.gameObject));
            }
            
            if (!consecutiveDamage)
            {
                gameObject.GetComponent<Collider>().enabled = false;
            }
        }

        else
        {
            Debug.Log(other.name);
        }
    }

    private IEnumerator DamagePlayer(GameObject player)
    {
		Vector3 damageDirection = transform.position - player.transform.position;
		player.transform.parent.parent.transform.GetComponent<Health>().TakeDamage(damage, canStun, stunTime, damageDirection, noBlock);
        damagePause = true;
        yield return new WaitForSeconds(0.05f);
        damagePause = false;

    }

	private IEnumerator DamageWall(WallHealth wall)
	{
		wall.TakeDamage(300);
		damagePause = true;
		yield return new WaitForSeconds(0.05f);
		damagePause = false;
		stopIt = true;
	}

}
