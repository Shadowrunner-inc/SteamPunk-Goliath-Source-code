using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Genbu_AI : MonoBehaviour {
	public bool _Testing;
    [Header("Aggro")]
    public GameObject wolf;
    public GameObject sondra;

    public GameObject target;
    [Range(6, 20)]
    public float aggroIntensity = 10.0f;
    
    [Space(8)]
    public float useRocketDistanceMin;
    public float useRocketDistanceMax;
    [Space(8)]
    public float useMinionDistanceMin;
    public float useMinionDistanceMax;
    [Space(8)]
    public float useRepulseDistanceMin;
    public float useRepulseDistanceMax;
    [Space(8)]
    public float useLaserDistanceMin;
    public float useLazerDistanceMax;
    [Space(12)]

    //[Header("Fixed Movement?")]
    //public bool fixedMovement;
    [Space(8)]

    public float moveSpeed;
    public Transform genbuDestination;
    private NavMeshAgent nav;
    
    [Space(12)]

    [Header("Summons")]
    public GameObject[] summon;
    public Transform[] rocketSpawnPositions;

    [Space(12)]
    [Header("Health")]
    public int mainHealth = 1300;
    public int shieldPlateHealth = 50;
    //public int legHealth = 100;
    [Space(12)]

    [Header("Attacks")]
	public GameObject cameraTargetController;
    public GameObject repulsionField;
    public GameObject genbuHead;
    public Transform laserSpawner;
    [Space(8)]
    public int laserDamage = 80;
    public int rocketDamage = 8;
    public int repulseDamage = 40;
    [Space(8)]
    [Range(0.1f, 1)]
    public float rocketSpawnRate = 0.3f;
    public int rocketAmount = 5;
	public float rocketSpeed = 80;

    public int laserCountDown = 5;
    public int laserEnlargeSpeed = 20;
    public int laserDuration = 5;
    [Space(8)]
    public float minionSpawnRate = 0.3f;
    public int minionAmount = 9;
    public Transform minionSpawnPosition;

    [Header("Damaged")]
    public GameObject[] shields;
    public GameObject[] weakpoints;
	//public GameObject[] Legs;
	public int[] shieldsBlah; 
    public bool stunnedWhenDamaged;
    public float damageStun = 4;
    [Range(0, 5)]
    public float speedDecreaseWhenDamaged = 1;
    private float sondraRageAdder = 10;
    private float wolfRageAdder = 10;

    //----------------------------------------------------------------------------
    private bool aggro = true;

    [HideInInspector]public bool dead;

    private float sondraRageIntensity = 0;
    private float wolfRageIntensity = 30;

    private float timer;
    private bool stunned;
    private bool stopMoving;

    private int countDown;
    private bool lookAtPlayer;
    private bool lookAtPlayerPause;

	private bool dummified;
	private float origAggroIntensity;
    
    private float sceneTimer;

	private ObjectiveSystem _objectivesystem;

    void Start () {
		_objectivesystem = GameObject.FindObjectOfType<ObjectiveSystem> ();
        aggro = true;

		origAggroIntensity = aggroIntensity;

        repulsionField.GetComponent<Hitbox>().damage = repulseDamage;

        if (repulsionField != null && repulsionField.GetComponent<Hitbox>())
        {
            repulsionField.GetComponent<Hitbox>().damage = repulseDamage;
            repulsionField.SetActive(false);
        }

        if (!GetComponent<NavMeshAgent>())
        {
            gameObject.AddComponent<NavMeshAgent>();
        }

        if (moveSpeed == 0)
        {
            moveSpeed = 3;
        }
        
        for(int i = 0; i < shields.Length; i++)
        {
            for (int c = 0; c < shields[i].transform.childCount; c++)
            {
                if(shields[i].transform.GetChild(c).GetComponent<Target>())
                {
                    shields[i].transform.GetChild(c).GetComponent<Target>().health = shieldPlateHealth;
                }
            }
        }
			
	}

    void Update () {
        if (dead)
        {/*Scene Changes now handled by the objective system
            sceneTimer += Time.deltaTime;
            if (sceneTimer > 2)
            {
                SceneManager.LoadScene("Level_01_P3_Desert");
            }*/

        }

        if (moveSpeed < 0)
        {
            moveSpeed = 0;
        }
        
		if (cameraTargetController.GetComponent<CameraTargetController> ().controlledChar == 1 && target == wolf
		    || cameraTargetController.GetComponent<CameraTargetController> ().controlledChar == 0 && target == sondra) 
		{
			dummified = true;

            if (target == wolf)
            {
                wolfRageIntensity -= 1 * Time.deltaTime;
                sondraRageIntensity += 1 * Time.deltaTime;
                if (sondraRageIntensity > wolfRageIntensity)
                {
                    sondraRageIntensity += 10;
                    wolfRageIntensity -= 10;
                }
            }

            else if (target == sondra)
            {
                wolfRageIntensity += 1 * Time.deltaTime;
                sondraRageIntensity -= 1 * Time.deltaTime;
                if (sondraRageIntensity < wolfRageIntensity)
                {
                    sondraRageIntensity -= 10;
                    wolfRageIntensity += 10;
                }
            }
		} 

		else 
		{
			dummified = false;
		}

        if (!stopMoving && !stunned)
        {
            FixedMovement();
        }

        if (!stunned)
        {
            Aggro_System();
        }

		if (dummified) 
		{
			aggroIntensity = origAggroIntensity * 2;
		} 

		else 
		{
			aggroIntensity = origAggroIntensity;
		}

		if (lookAtPlayer && !lookAtPlayerPause && !dummified)
        {
            Vector3 targetDir = target.transform.position - genbuHead.transform.position;
            Vector3 newDir = Vector3.RotateTowards(genbuHead.transform.forward, targetDir, Time.deltaTime, 0.0f);
            genbuHead.transform.rotation = Quaternion.LookRotation(newDir);
        }

		else if (lookAtPlayer && !lookAtPlayerPause && dummified)
		{
			Vector3 targetDir = target.transform.position - genbuHead.transform.position;
			Vector3 newDir = Vector3.RotateTowards(genbuHead.transform.forward, targetDir, Time.deltaTime / 15, 0.0f);
			genbuHead.transform.rotation = Quaternion.LookRotation(newDir);
		}

        else if (!lookAtPlayer)
        {
            //genbuHead.transform.eulerAngles = new Vector3(0, 90, 0);
            genbuHead.transform.eulerAngles = new Vector3
               (Mathf.LerpAngle(genbuHead.transform.eulerAngles.x, 0, Time.deltaTime),
                Mathf.LerpAngle(genbuHead.transform.eulerAngles.y, 90, Time.deltaTime),
                Mathf.LerpAngle(genbuHead.transform.eulerAngles.z, 0, Time.deltaTime));
        }
        //-----------------------------------------
        // development Testing Stuff
		if (Input.GetKeyDown(KeyCode.Space) && _Testing)
        {
            int launcher = Random.Range(0, rocketSpawnPositions.Length);
            StartCoroutine(Summon(rocketAmount, rocketSpawnRate, rocketSpawnPositions[launcher], 0));
        }
        //----------------------------------------
    }

    public void Aggro_System()
    {
        if (aggro)
        {
            if (sondra == null || wolf == false)
            {
                aggro = false;
            }

            else
            {
                if (sondraRageIntensity > wolfRageIntensity)
                {
                    target = sondra;
                }

                else if (wolfRageIntensity > sondraRageIntensity)
                {
                    target = wolf;
                }
            }

            timer += Time.deltaTime; // Timer used to delay Genbu's attack

            int randomChance = Random.Range(0, 4); // Decide what attack to use
            if (timer > aggroIntensity) // If Timer's more than the
            {
                //--------------------Use Rockets--------------------//
                if (randomChance == 0)
                {
                    if ((Vector3.Distance(gameObject.transform.position, target.transform.position) > useRocketDistanceMin)
                        && (Vector3.Distance(gameObject.transform.position, target.transform.position) < useRocketDistanceMax))
                    {
                        int launcher = Random.Range(0, rocketSpawnPositions.Length);
                        StartCoroutine(Summon(rocketAmount, rocketSpawnRate, rocketSpawnPositions[launcher], 0));
                        timer = 0;
                    }
                    
                }
                //--------------------Use Minions--------------------//
                if (randomChance == 1)
                {
                    if ((Vector3.Distance(gameObject.transform.position, target.transform.position) > useMinionDistanceMin)
                        && (Vector3.Distance(gameObject.transform.position, target.transform.position) < useMinionDistanceMax))
                    {
                        StartCoroutine(Summon(minionAmount, minionSpawnRate, minionSpawnPosition, 1));
                        timer = 0;
                    }
                }
                //--------------------Use Stomp--------------------//
                if (randomChance == 2)
                {
                    if ((Vector3.Distance(gameObject.transform.position, target.transform.position) > useRepulseDistanceMin)
                        && (Vector3.Distance(gameObject.transform.position, target.transform.position) < useRepulseDistanceMax))
                    {

                        StartCoroutine(Repulse_Attack());
                        timer = -13;
                    }
                }
                //-----------------Use Giant Lazer-----------------//
				if (randomChance == 3 && target != sondra)
                {
                    if ((Vector3.Distance(gameObject.transform.position, target.transform.position) > useLaserDistanceMin)
                        && (Vector3.Distance(gameObject.transform.position, target.transform.position) < useLazerDistanceMax)
                        && (Vector3.Angle((target.transform.position - genbuHead.transform.position), target.transform.position) > 100))
                    {
                        StartCoroutine(Laser(2));
                        timer = 0;
                    }

                    else if ((Vector3.Distance(gameObject.transform.position, target.transform.position) > useRocketDistanceMin)
                            && (Vector3.Distance(gameObject.transform.position, target.transform.position) < useRocketDistanceMax))
                    {
                        int launcher = Random.Range(0, rocketSpawnPositions.Length);
                        StartCoroutine(Summon(rocketAmount, rocketSpawnRate, rocketSpawnPositions[launcher], 0));
                        timer = 0;
                    }
                }
            }
        }
    }

    public void FixedMovement()
    {
        nav = GetComponent<NavMeshAgent>();
        nav.radius = 50.0f;
        nav.height = 100.0f;

        //===========================================================================================

        nav.isStopped = false;
        nav.speed = moveSpeed;
        if (genbuDestination != null)
        {
            nav.SetDestination(genbuDestination.position);

            if (Vector3.Distance(transform.position, genbuDestination.position) < 100)
            {
                SceneManager.LoadScene("Cutscene_Lose");
            }
        }

        else
            Debug.LogError("No destination detected");


        //===========================================================================================
    }

    IEnumerator Repulse_Attack()
    {
        repulsionField.SetActive(true);
        yield return new WaitForSeconds(5);

        int randomRange = Random.Range(0, 2);
        if (randomRange == 0 && (Vector3.Angle((target.transform.position - genbuHead.transform.position), target.transform.position) > 100))
        {
            StartCoroutine(Laser(2));    
        }

        else
        {
            int launcher = Random.Range(0, rocketSpawnPositions.Length);
            StartCoroutine(Summon(rocketAmount, rocketSpawnRate, rocketSpawnPositions[launcher], 0));
        }

        yield return null;
    }

    IEnumerator Summon(int amount, float spawnRate, Transform spawnOffSet, int spawnID)
    {
        for (int i = 0; i < amount; i++)
        {
            timer = amount * -1.5f;
            GameObject spawn = Instantiate(summon[spawnID], spawnOffSet.position, transform.rotation);
            
            if (spawn.GetComponent<Missile_AI>())
            {
                spawn.GetComponent<Missile_AI>().target = target;
                spawn.GetComponent<Missile_AI>().damage = rocketDamage;
				spawn.GetComponent<Missile_AI> ().rocketSpeed = rocketSpeed;
            }

            if (spawn.GetComponent<Minion_Ground_AI>())
            {
                spawn.GetComponent<Minion_Ground_AI>().target = target;
                spawn.transform.GetChild(0).GetComponent<Minion_AI>().target = target;
            }

            yield return new WaitForSeconds(spawnRate);
        }

        yield return null;

    }

    IEnumerator Laser(int summonID)
    {
        countDown = 0;
        lookAtPlayer = true;
        while(countDown < laserCountDown)
        {
            timer = 0;
            yield return new WaitForSeconds(1);
            countDown++;
			if (countDown >= (laserCountDown - 1) && !dummified) 
			{
				lookAtPlayerPause = true;
			} 

			else if (countDown >= (laserCountDown - 2) && dummified) 
			{
				lookAtPlayerPause = true;
			} 

			else 
			{
				lookAtPlayerPause = false;
			}
        }
			

        GameObject laser = Instantiate(summon[summonID], laserSpawner.position, laserSpawner.rotation);
        laser.GetComponent<Enlargement>().enlargeSpeed = laserEnlargeSpeed;
        laser.GetComponent<Enlargement>().parent = laserSpawner;
        laser.transform.GetChild(0).GetComponent<Hitbox>().damage = laserDamage;
        timer = 0;
        yield return new WaitForSeconds(laserDuration);

        timer = 0;
        lookAtPlayerPause = false;
        lookAtPlayer = false;
        yield return null;
    }

    // YOU CAN CALL THIS IN OTHER SCRIPTS WITH '' GetComponent<Genbu_AI>().RegularHit(whoHitMe); ''
    // Preferably the script that deals with the Crossbow
    public IEnumerator DamageShield(string whoHitMe, int shieldNo) //Case sensitive, '' Wolf '' OR '' Sondra ''
    {
        aggro = true;

        shieldsBlah[shieldNo] -= 1;

		//Talk to Objective system bolt deystruction
		for(int x = 0; x < _objectivesystem._Objectives.Count; x++){
			print ("For loop step in");
			if (_objectivesystem._Objectives [x].name == "Shield bolts") {
				print ("Told o system bolt destroy");
				_objectivesystem._Objectives [x].currentCount++; //Increment
				if(_objectivesystem._Objectives [x].currentCount == _objectivesystem._Objectives [x].total){ _objectivesystem._Objectives [x].bIsCompleted = true;} //Check to see if the goal is finished
			}
		}

        if (shieldsBlah[shieldNo] == 0)
        {
            shields[shieldNo].transform.parent = null;
            shields[shieldNo].GetComponent<Rigidbody>().isKinematic = false;
            shields[shieldNo].GetComponent<Rigidbody>().AddForce((shields[shieldNo].transform.position - transform.position) * 30.0f);

			//Talk to Objective system plate has fallen off
			for(int x = 0; x < _objectivesystem._Objectives.Count; x++){
				if (_objectivesystem._Objectives [x].name == "armoured plating") {
					_objectivesystem._Objectives [x].currentCount++; //Increment
					if(_objectivesystem._Objectives [x].currentCount == _objectivesystem._Objectives [x].total){ _objectivesystem._Objectives [x].bIsCompleted = true;} //Check to see if the goal is finished
				}
			}
        }
            if (whoHitMe == "Wolf")
        {
            wolfRageIntensity += wolfRageAdder;
            if (wolfRageIntensity > sondraRageIntensity)
            {
                sondraRageIntensity = sondraRageIntensity / 2;
            }
        }

        else if (whoHitMe == "Sondra")
        {
            sondraRageIntensity += sondraRageAdder;
            if (sondraRageIntensity > wolfRageIntensity)
            {
                wolfRageIntensity = wolfRageIntensity / 2;
            }
        }

        else
        {
            Debug.LogError("Remember, case sensitive. Genbu doesn't recognize: " + whoHitMe);
        }

        yield return null;
    }
		
	/// <summary>
	/// Author: nate hales
	/// USE this to destory the legs when sondra attack from the inside. Also Checks to see if Phase 3 needs to Begin
	/// Hides the legs.
	/// </summary>
	/// <param name="LegIndex">Leg index.</param>
    ///
    /*
	public void HideLegs(int LegIndex)
    {
		Legs [LegIndex].SetActive (false);
		int brokenLegNum = 0;
		foreach (GameObject leg in Legs) {
			if(leg.activeSelf == false){brokenLegNum++;}
		}
		if (brokenLegNum == Legs.Length) {
		//TODO: Begin Phase 3 and Turn this Genbu_AI Off.
		}
    }
    */

    // YOU CAN CALL THIS IN OTHER SCRIPTS WITH '' GetComponent<Genbu_AI>().WeakPointHit(whoHitMe, shieldNo); ''
    // Preferably the script that deals with the Crossbow
    public IEnumerator WeakPointHit(string whoHitMe, GameObject obj, int legID) //Case sensitive, '' Wolf '' OR '' Sondra ''
    {
        ThighBlaster tB = GetComponent<ThighBlaster>();

		// Objective system leg was shot by crossbow
		for(int x = 0; x < _objectivesystem._Objectives.Count; x++){
			if (_objectivesystem._Objectives [x].name == "Puncture legs") {
				_objectivesystem._Objectives [x].currentCount++; //Decrement
				if(_objectivesystem._Objectives [x].currentCount == _objectivesystem._Objectives [x].total){ _objectivesystem._Objectives [x].bIsCompleted = true;} //Check to see if the goal is finished
			}
		}

        if (legID == 0)
        {
            tB.LFrontDamaged = true;
            tB.LFrontBroken = true;
        }

        else if (legID == 1)
        {
            tB.RFrontDamaged = true;
            tB.RFrontBroken = true;
        }

        else if (legID == 2)
        {
            tB.LBackDamaged = true;
            tB.LBackBroken = true;
        }

        else if (legID == 3)
        {
            tB.RBackDamaged = true;
            tB.RBackBroken = true;
        }

        else
        {
            Debug.LogError("Wtf? I thought Genbu only has 4 legs?");
        }

        nav.isStopped = true;
        nav.speed = 0;
        if (!stunnedWhenDamaged)
        {
            stopMoving = true;
        }

        else if (stunnedWhenDamaged)
        {
            stunned = true;
        }

        //
        //Run an animation
        //
        
        yield return new WaitForSeconds(damageStun);
        nav.isStopped = false;
        nav.speed = moveSpeed;

        moveSpeed -= speedDecreaseWhenDamaged; //Lost a leg slow down

        //
        //Return back to Original Animation
        //

        stopMoving = false;
        stunned = false;
        aggro = true;


        if (whoHitMe == "Wolf")
        {
			wolfRageIntensity = sondraRageIntensity + wolfRageAdder;
            if (wolfRageIntensity > sondraRageIntensity)
            {
                sondraRageIntensity = sondraRageIntensity / 2;
            }
        }

        else if (whoHitMe == "Sondra")
        {
			sondraRageIntensity = wolfRageIntensity + sondraRageAdder;
            if (sondraRageIntensity > wolfRageIntensity)
            {
                wolfRageIntensity = wolfRageIntensity / 2;
            }
        }

        else
        {
            Debug.LogError("Remember, case sensitive. Genbu doesn't recognize: " + whoHitMe);
        }
        yield return null;
    }


	/// <summary>
	/// Author: Nate hales
	/// Stuns the genbu. Use this for the Steampipes to stun Genbu
	/// </summary>
	/// <returns>The genbu.</returns>
	/// <param name="stunTime">Stun time.</param>
	public IEnumerator StunGenbu(GameObject toDestroy, float stunTime) 
	{
        ThighBlaster tB = GetComponent<ThighBlaster>();

        if (tB.LFrontDamaged && !tB.LFrontBroken)
        {
            tB.LFrontBroken = true;
        }

        if (tB.RFrontDamaged && !tB.RFrontBroken)
        {
            tB.RFrontBroken = true;
        }

        if (tB.LBackDamaged && !tB.LBackBroken)
        {
            tB.LBackBroken = true;
        }

        if (tB.RBackDamaged && !tB.RBackBroken)
        {
            tB.RBackBroken = true;
        }

        nav.isStopped = true;
		nav.speed = 0;

		stunned = true;
        
        yield return new WaitForSeconds(stunTime);

        Destroy(toDestroy);

		nav.isStopped = false;
		nav.speed = moveSpeed;

		//
		//Return back to Original Animation
		//

		stunned = false;
        stopMoving = false;

		aggro = true;

		moveSpeed -= speedDecreaseWhenDamaged; //Lost a leg slow down

		yield return null;
	}

}
