using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Author: Jospeh K.
/// Edited by Nathan Hales
/// Snake Genbu's(Boss 2) AI Brain
/// </summary>

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Boss_Health))]
[RequireComponent(typeof(Target))]
[RequireComponent(typeof(Rigidbody))]
public class Snake_Genbu_AI : MonoBehaviour {

    //===================================================
    // Publics
    public bool _Testing; //Debug Mode?
    [Space(8)]

    [Header("Attacks")]
    [Range(6, 20)]
	public float aggroIntensity = 10.0f; //How often does the AI attack?

    public int burrowDamage = 20; //How much damage does the burrow attack do?

	public float laserTurnSpeed = 10.0f; //How fast does the Snake turn his head during Laser form
	public int laserDamage = 80; //How much damage does the laser every 0.1 seconds?
	
	public int laserCountDown = 5; //How long does the snake wait before shooting the laser?
	public float laserStartSize = 1f; //What size does the laser start at?
	public int laserEnlargeSpeed = 20; //How fast does the laser grow?
	public int laserDuration = 5; //How long does the laser last?

    [Header("Ignore")]
    public GameObject wolf; //Wolf's gameObject
    public GameObject sondra; //Sondra's gameObject
    public GameObject cityWall; //City wall's GameObject
    public Transform[] moveLocations; //Locations for the snake to move to
    public Transform laserSpawner; //Where the Laser Spawns
    public GameObject laserPrefab;
    public Hitbox headHitBox;

    //===================================================
    // Privates
    private NavMeshAgent nav; //Shortcut for the NavMeshAgent Component
    private Boss_Health bH; //Shortcut for Boss_Health script

    private GameObject target; //Who the Snake should target
    private CameraTargetController camTarCon; //Camera Controller (For checking which player character is active)

    private Transform currentMoveLocation;

    private Rigidbody rb;

    private float maxHealth; //What health did this AI start at?
    private float nextWallAttack; //Health Snake needs to be lower than in order to switch targets
    private int wallTargetCount = 3; //How many times should the AI attack the Wall?
    private float nextWallAttackSubstract; //How much health is substracted from "nextWallAttack"

    private float sondraRageIntensity = 0;
    private float wolfRageIntensity = 20;

    private float timer;

    private int burrowUses;

    private bool pauseUpdate = false;   //For deciding whether or not to pause the Update Function. This will pretty much stop
                                        //everything in the script except IEnumerators (which is the idea).

    private bool burrowAttack = false;

    //-----------------------------------------------------------------------------------------------------------------------------------------------------
    // When the Game Starts
    private void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        bH = GetComponent<Boss_Health>();
        camTarCon = GameObject.FindObjectOfType<CameraTargetController>();
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.mass = 9001;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        headHitBox.transform.GetComponent<Collider>().enabled = false;
        headHitBox.damage = burrowDamage;

        maxHealth = bH.health;

        if (wallTargetCount > 0)
        {
            //Example: 
            //X = 1000 - (1000 / 3 + 1)
            //X = 1000 - 250
            //X = 750
            nextWallAttackSubstract = (maxHealth / (wallTargetCount + 1));
            nextWallAttack = maxHealth - nextWallAttackSubstract;
        }
    }

    //-----------------------------------------------------------------------------------------------------------------------------------------------------
    // Every Frame
    private void Update()
    {
        if (!pauseUpdate)
        {
            AI();
            if (transform.position.y < -30)
            {
                ReturnToSurface();
            }

            if (timer > aggroIntensity * 2)
            {
                StopAllCoroutines();
                StartCoroutine(BurrowAttack());
            }
        }
    }

    private void FixedUpdate()
    {
        timer += Time.deltaTime;
        
        Judgement();
    }

    //-----------------------------------------------------------------------------------------------------------------------------------------------------
    // AI Stuff
    private void AI()
    {
        if (timer > aggroIntensity && target != cityWall)
        {
            int moveSelector = Random.Range(0, 2);

            if (moveSelector == 1 && transform.position.y < -10)
            {
                moveSelector = 0;
            }

            if (moveSelector == 0)
            {
                if (wallTargetCount >= 2)
                {
                    burrowUses = Random.Range(3, 5);
                }

                else
                {
                    burrowUses = Random.Range(5, 8);
                }
                StartCoroutine(BurrowAttack());
            }

            else
            {
                StartCoroutine(LaserFrenzy());
            }
        }

        else if (moveLocations.Length > 1)
        {
            StartCoroutine(MoveToRandom());
        }

        if (target == cityWall && !burrowAttack)
        {
            int moveSelector = Random.Range(0, 2);
            StopAllCoroutines();
            StartCoroutine(LaserTheWall());
        }
    }

    //-----------------------------------------------------------------------------------------------------------------------------------------------------
    //Function for deciding who the AI should target. Sondra or Wolf?
    void Judgement()
    {
        if (bH.health < nextWallAttack)
        {
            target = cityWall;

            if (cityWall.transform.parent.GetComponent<WallHealth>()._healthBar.value >= 1000)
            {
                bH.immune = true;
            }
        }

        else
        {
            bH.immune = false;
            if (camTarCon.controlledChar == 0 && wolfRageIntensity < (sondraRageIntensity + 200))
            {
                wolfRageIntensity += 10;
            }

            if (camTarCon.controlledChar == 1 && sondraRageIntensity < (wolfRageIntensity + 200))
            {
                sondraRageIntensity += 10;
            }

            //If either of these don't exist
            if (sondra == null || wolf == false)
            {
                Debug.LogError("One of the Main Cast is missing. Script will break without both Sondra and Wolf"); //Show Error in Inspector
            }

            //Else, choose a target based on who the AI's angrier at
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
        }
    }

    //-----------------------------------------------------------------------------------------------------------------------------------------------------
    // Movement
    IEnumerator MoveToRandom()
    {
        pauseUpdate = true; //Stop the Update function from running

        Transform previousMoveLocation = currentMoveLocation; //Get the previous Transform the AI had to move to

        bool newLocation = false; //For deciding whether or not the next location is "new"
        
        while (newLocation == false)
        {
            currentMoveLocation = moveLocations[Random.Range(0, moveLocations.Length)]; //Pick a location at Random

            //If the location is not the same as the previous one
            if (currentMoveLocation != previousMoveLocation)
            {
                nav.SetDestination(currentMoveLocation.position); //Set Destination to this Transform
                nav.isStopped = false; //Ensure that the NavMeshAgent is not paused

                newLocation = true; //And tell the while loop to fuck off
            }
        }

        //If the Snake has not reached its destination
        while (nav.remainingDistance > nav.stoppingDistance)
        {
            //(Do not proceed with the rest of the Coroutine)
            yield return new WaitForFixedUpdate(); //THIS IS REQUIRED SO THAT THE WHILE STATEMENT DOESN'T HAVE AN INFINITE LOOP AND BREAK UNITY
        }

        pauseUpdate = false; //Resume the Update Function
        yield return null;
    }

    //-----------------------------------------------------------------------------------------------------------------------------------------------------
    // Attack 1
    IEnumerator BurrowAttack()
    {
        CamShake cS = GameObject.FindObjectOfType<CamShake>();
        burrowAttack = true;
        while (burrowUses > 0)
        {
            if (target == cityWall)
            {
                bH.immune = false;
            }

            GetComponent<Collider>().enabled = false;
            transform.localEulerAngles = new Vector3(0, 0, 0);
            pauseUpdate = true; //Stop the Update function from running
            nav.enabled = false; //Disable NavMeshAgent so that it doesn't send the Snake back to the surface

            float internalTimer = 0; //timer for the Coroutine

            //While the internal timer is less than 1 second
            while (internalTimer < 1)
            {
                //-----(Burrow downwards)-----//
                transform.Translate(0, -3, 0);
                internalTimer += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }
            cS.shake = true;
            yield return new WaitForSeconds(1); //Pause Underground for 1 second
            cS.shake = false;
            transform.localEulerAngles = new Vector3(-90, 0, 0); //Have the Snake look upwards
            headHitBox.transform.GetComponent<Collider>().enabled = true;

            //int decider = 1;

            //if (decider == 0 || burrowUses == 1)
            //{
            //-----Under player-----//
            transform.position = new Vector3(target.transform.position.x, -10, target.transform.position.z); //Set position directly under the player
            RaycastHit hit;
            while (Physics.Raycast(transform.position, transform.forward, out hit, 10000) && hit.transform.tag != "Skybox")
            {
                transform.Translate(0, 0, 4);
                yield return new WaitForFixedUpdate();
            }

            rb.isKinematic = false;

            int interval = Random.Range(2, 5);

            if (wallTargetCount < 2)
            {
                interval = Random.Range(1, 3);
            }

            rb.velocity = ParabolicEquation(interval, false);

            yield return new WaitForSeconds(interval); 
            //}

            /*else
            {
                //-----To player-----//
                Transform randomPosition = moveLocations[Random.Range(0, moveLocations.Length)];

                transform.position = new Vector3(randomPosition.position.x, -10, randomPosition.position.z);
                RaycastHit hit;
                while (Physics.Raycast(transform.position, transform.forward, out hit, 100) && hit.transform.tag != "Skybox")
                {
                    transform.Translate(0, 0, 4);
                    yield return new WaitForFixedUpdate();
                }

                rb.isKinematic = false;

                rb.velocity = ParabolicEquation(1, true);

                yield return new WaitForSeconds(1);

            }
            */
            transform.localEulerAngles = new Vector3(0, 0, 0);

            burrowUses -= 1;

            if (target == cityWall)
            {
                wallTargetCount -= 1;
                aggroIntensity -= 1.5f;
                nextWallAttack -= nextWallAttackSubstract;
                burrowUses = 0;
            }

            yield return new WaitForFixedUpdate();
        }

        GetComponent<Collider>().enabled = true;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        headHitBox.transform.GetComponent<Collider>().enabled = false;
        timer = 0;
        nav.enabled = true;
        pauseUpdate = false; //Resume the Update Function
        burrowAttack = false;
        yield return null;
    }

    //-----------------------------------------------------------------------------------------------------------------------------------------------------
    // Attack 2
    IEnumerator LaserFrenzy()
    {
        pauseUpdate = true; //Stop the Update function from running
        bH.immune = false;
        Vector3 targetDist = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z); //Ignore the Y differences
        
        //While the player is out of the Snake Head's field of view
        while (CanSeeTarget() == false)
        {
            nav.SetDestination(targetDist); //Set Destination to the target's location (A cheat for getting the Snake to get an angle on the player first before doing anything else)
            nav.isStopped = false; //Ensure that the NavMeshAgent is not paused
            yield return new WaitForFixedUpdate();
        }

        nav.isStopped = true;

        float internalTimer = 0;

        Vector3 targetDir2 = target.transform.position - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir2, Time.deltaTime, 0.0f);

        //---(Charge up the laser)----//
        while (internalTimer < laserCountDown)
        {
            //If the player isn't hidden...
            if (CanSeeTarget())
            {
                nav.enabled = false; //To stop the Nav from changing the rotation
                //Turn towards the player
                targetDir2 = target.transform.position - transform.position;
                newDir = Vector3.RotateTowards(transform.forward, targetDir2, Time.deltaTime, 0.0f);
            }
            transform.rotation = Quaternion.LookRotation(newDir);

            internalTimer += Time.deltaTime; //And count how much time went by
            yield return new WaitForFixedUpdate();
        }

        //---------FIRE THE LASER---------//
        GameObject laser = Instantiate(laserPrefab, laserSpawner.position, laserSpawner.rotation);
        laser.transform.parent = laserSpawner;
        laser.transform.localScale = new Vector3(laserStartSize / transform.localScale.x, laserStartSize / transform.localScale.y, 0.001f);
        laser.GetComponent<Enlargement>().enlargeSpeed = laserEnlargeSpeed / transform.localScale.z;
        laser.GetComponent<Enlargement>().parent = laserSpawner;
        laser.transform.GetChild(0).GetComponent<Hitbox>().damage = laserDamage;
        yield return new WaitForSeconds(laserDuration);
        //--------------------------------//

        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.localEulerAngles.z);
        nav.enabled = true;

        timer = 0; //Reset the timer
        pauseUpdate = false; //Resume the Update Function
        yield return null;
    }

    //-----------------------------------------------------------------------------------------------------------------------------------------------------
    // Attack 3
    IEnumerator LaserTheWall()
    {
        pauseUpdate = true; //Stop the Update function from running

        Vector3 targetDist = new Vector3(cityWall.transform.position.x, transform.position.y, cityWall.transform.position.z); //Ignore the Y differences
        nav.isStopped = true;
        float internalTimer = 0;

        Vector3 targetDir2 = cityWall.transform.position - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir2, Time.deltaTime, 0.0f);

        //---(Charge up the laser)----//
        while (internalTimer < laserCountDown)
        {
            targetDir2 = target.transform.position - transform.position;
            newDir = Vector3.RotateTowards(transform.forward, targetDir2, Time.deltaTime * 3, 0.0f);

            transform.rotation = Quaternion.LookRotation(newDir);

            internalTimer += Time.deltaTime; //And count how much time went by
            yield return new WaitForFixedUpdate();
        }

        //---------FIRE THE LASER---------//
        GameObject laser = Instantiate(laserPrefab, laserSpawner.position, laserSpawner.rotation);
        laser.transform.parent = laserSpawner;
        laser.transform.localScale = new Vector3(laserStartSize / transform.localScale.x, laserStartSize / transform.localScale.y, 0.001f);
        laser.GetComponent<Enlargement>().enlargeSpeed = laserEnlargeSpeed / transform.localScale.z;
        laser.GetComponent<Enlargement>().parent = laserSpawner;
        laser.transform.GetChild(0).GetComponent<Hitbox>().damage = laserDamage;
        yield return new WaitForSeconds(laserDuration);
        //--------------------------------//

        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.localEulerAngles.z);
        nav.enabled = true;

        timer = 0; //Reset the timer
        pauseUpdate = false; //Resume the Update Function
        
        wallTargetCount -= 1;
        aggroIntensity -= 1.5f;
        nextWallAttack -= nextWallAttackSubstract;
        bH.immune = false;
        yield return null;
    }

    IEnumerator ReturnToSurface()
    {
        GetComponent<Collider>().enabled = false;
        rb.velocity = ParabolicEquation(2, false);
        yield return new WaitForSeconds(2);
        GetComponent<Collider>().enabled = true;
        yield return null;
    }

    Vector3 ParabolicEquation(float timeToTarget, bool player)
    {
        Transform previousMoveLocation = currentMoveLocation; //Get the previous Transform the AI had to move to
        Transform randomPosition = moveLocations[Random.Range(0, moveLocations.Length)];



        while (randomPosition == previousMoveLocation)
        {
            randomPosition = moveLocations[Random.Range(0, moveLocations.Length)];
        }
        Vector3 AtoB;
        if (!player)
        {
            AtoB = randomPosition.position - transform.position;
        }

        else
        {
            Vector3 targetDist = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z); //Ignore the Y differences
            AtoB = targetDist - transform.position;
        }
        Vector3 horizontal = GetHorizontalVector(AtoB, Physics.gravity);
        float horizontalDistance = horizontal.magnitude;
        Vector3 vertical = GetVerticalVector(AtoB, Physics.gravity);
        float verticalDistance = vertical.magnitude * Mathf.Sign(Vector3.Dot(vertical, -Physics.gravity));

        float horizontalSpeed = horizontalDistance / timeToTarget;
        float verticalSpeed = (verticalDistance + ((0.5f * Physics.gravity.magnitude) * (timeToTarget * timeToTarget))) / timeToTarget;

        Vector3 launch = (horizontal.normalized * horizontalSpeed) - (Physics.gravity.normalized * verticalSpeed);
        return launch;
    }

    private Vector3 GetHorizontalVector(Vector3 AtoB, Vector3 gravityBase)
    {
        Vector3 output;
        Vector3 perpendicular = Vector3.Cross(AtoB, gravityBase);
        perpendicular = Vector3.Cross(gravityBase, perpendicular);
        output = Vector3.Project(AtoB, perpendicular);
        return output;
    }

    private Vector3 GetVerticalVector(Vector3 AtoB, Vector3 gravityBase)
    {
        Vector3 output;
        output = Vector3.Project(AtoB, gravityBase);
        return output;
    }

    bool CanSeeTarget()
    {
        RaycastHit hit;
        Vector3 rayDirection = target.transform.position - transform.position;

        if (Physics.Raycast(transform.position, rayDirection, out hit))
        {
            if (hit.transform.gameObject == target)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        else
        {
            return false;
        }
    }
} 