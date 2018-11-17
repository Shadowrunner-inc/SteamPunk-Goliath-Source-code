using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Nathan Hales
/// Edited by Jospeh K.
/// Attached to trigger boxs hazard in the  clock tower level this script sends its data to the Angel boss and gets turned on during one of its attacks. 
/// Its a timer based activation that waits to be called by the boss.
/// </summary>


public class ElectricPlatform : MonoBehaviour
{


    [HideInInspector] public bool canStun = true;
    [HideInInspector] public bool noBlock = false;
    [HideInInspector] public float stunTime = 1;

    [HideInInspector]public float timer = 1.5f, DamageTimer = 0.05f;
    [HideInInspector] public int _damage;

    private float timeKeeper;//Ticks up with time for threshold checks.
    private bool damagePause = false;//Pauses the damage 
    private CameraTargetController _cameraTargetController;

	void Start () {
        HelloAngel();

	    _cameraTargetController = GameObject.FindObjectOfType<CameraTargetController>();
        
        //Turn your self off.
        gameObject.SetActive(false);
	}
	
	void Update ()
	{
	   if(timeKeeper > 0f) {timeKeeper -= Time.deltaTime;} //count Down
	   else{ gameObject.SetActive(false); }//Turn your gameobject off.
    }

    void OnDisable()
    {
        //reset time keeper to the amount of time in the timer;
        timeKeeper = timer;
        damagePause = false;
        StopAllCoroutines();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!damagePause && IsPC(other))
            {
                Health _health = other.transform.parent.parent.GetComponent<Health>();

                StartCoroutine(dealDamage(_health, other.transform.position));
            }
        }
    }

    IEnumerator dealDamage(Health playerHealth, Vector3 playerPos)
    {
        Vector3 dmgDir = transform.position - playerPos;
        playerHealth.TakeDamage( _damage, canStun, stunTime, dmgDir, false);
        damagePause = true;
        yield return new WaitForSeconds(DamageTimer);
        damagePause = false;
    }

    /// <summary>
    /// Checks to see if the collider belongs to a character currently controlled by the player.
    /// </summary>
    /// <param name="characteCollider"></param>
    /// <returns></returns>
    bool IsPC(Collider characteCollider)
    {
        if(_cameraTargetController.controlledChar == 0 && characteCollider.transform.parent.parent.gameObject.name == "Sondra") 
            return false;
        else if (_cameraTargetController.controlledChar == 1 && characteCollider.transform.parent.parent.gameObject.name == "Wolf")
            return false;
        else return true;

    }

    void HelloAngel()
    {
       Angel_AI myBoss = GameObject.FindObjectOfType<Angel_AI>(); //find the angel's AI
        myBoss._electricPlatforms.Add(this.GetComponent<ElectricPlatform>()); //store a refference to yourself with the boss AI
        
        //Get your stats from the boss
        canStun = myBoss.ePlatformCanStun;
        stunTime = myBoss.ePlatformStunTime;
        noBlock = myBoss.ePlatformNoBlock;
        timer = myBoss.ePlatformTimer;
        DamageTimer = myBoss.ePlatformDamageTimer;
        _damage = myBoss.ePlatformDamage;
        timeKeeper = timer;
    }

}
