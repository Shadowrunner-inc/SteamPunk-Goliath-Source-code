using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hydra_AI : MonoBehaviour {

    //===================================================
    // Publics

    
    public enum SpecialAttack { NeckSlam, FireAOE, Eruption }; //List of Special Attacks

    /*
     * AudioClips
     * 0: Roar
     * 1: Fireball
     * 2: Fire AOE Wind-up
     * 3: Fire AOE BLARG!
     * 4: Neck Slam Startup
     * 5: Neck Slam SLAM*
     * 6: Eruption Roar
     * 7: Screams of Pain as it sinks into the Lava, never to be seen again........ :)
    */

    //public AudioClip[] audioClips;
    [Range(2, 20)]
    public float attackDelay = 10.0f; //How often does the Hydra use their attack
    public float turnSpeed; //How fast does this Hydra turn its head
    
    [Space(12)]
    [Header("Attacks")]
    public SpecialAttack specialAttack; //Special Attack that the user can select in the inspector (Make it visible)
    public float specialAttackDistance; //Distance the player has to be within in order to use Special Attack
    public float moveSpeed = 40.0f;
    public int damageToStun = 30;

    [Range(0,10)]
    public float useFireBallSpeed = 10; //How fast the Fireball travel
    
    [Range(0,100)]
    public int fireBallDamage; //How much damage the Fireball do

    public float afterSpecialMoveDistance = 85.0f;

    [Space(12)]
    [Header("Ignore")]
    public bool aggro; //Is this Hydra angry?
    public bool dead; //Is this Hydra dead?
    public GameObject wolf; //Wolf's gameObject
    public GameObject sondra; //Sondra's gameObject
    public GameObject fireBallPrefab; //Fireball Prefab to be Instantiated
    public GameObject specialAttackEffect; //Special Attack to be turned on and off (Not a prefab)
    public GameObject fireballSpawner;
    public GameObject hydraHead; //Hydra's head for turning
    public GameObject hydraBody; //Hydra's body to turn when head reaches an angle
    public Transform[] fireBreathTurnPoints; //;-------(FireAOE)-------; Positions the Hydra turns to  
    public int remainingHydras;
    //===================================================
    // Privates

    private GameObject target;

    private AudioSource audioSource;

    private float sondraRageIntensity = 0;
    private float wolfRageIntensity = 10;

    private float fireBallTimeToTarget = 4.0f;
    
    private float timer = 0.0f;

    private int turnTarget = 0;

    private bool fireAOE;
    private bool neckSlam;
    private bool eruption;

    private bool sAttack;

    private float fireAOETimer;

    private bool playSound;
    private int soundNo;

    private Vector3 originalPos;
    private Vector3 originalRot;

    private int nextStun;

    private bool stunned;

    private ObjectiveSystem _objectiveSystem; //Current objective system whithin the scene

    // Use this for initialization
    void Start () {
        originalPos = transform.position;

        //Fetch the objective system whith in the scene
        _objectiveSystem = FindObjectOfType<ObjectiveSystem>();
        if (_objectiveSystem == null) //Error checking
        { 
            Debug.LogError( name + " couldn't find the objective system in the current scene and cannot fuction without it.");
            Debug.Break(); //Hault the editor
        }

        //===================================================================================
        // AudioSource
        //If this object does not have an AudioSource, add one
        if (GetComponent<AudioSource>() == null)
        {
            gameObject.AddComponent<AudioSource>();
        }
        
        //Assign the AudioSource on this object to the variable (To make it easier to type)
        audioSource = gameObject.GetComponent<AudioSource>();
        
        //Make sure this object never loops
        audioSource.loop = false;
        //===================================================================================
        
        
        //===================================================================================
        // Special Attack
        //Set the Effect for the Special Attack off
        if (specialAttackEffect != null)
            specialAttackEffect.SetActive(false);
        //===================================================================================
        
        
        //===================================================================================
        // Aggro
        //Set starting target to wolf by default
        target = wolf;
        //===================================================================================

        remainingHydras = 3;
        
    }

    // Update is called once per frame
    void FixedUpdate () {

        Judgement();

        //Reduce the Time it takes for the Fireball to reach the player based on Distance
        fireBallTimeToTarget = Vector3.Distance(target.transform.position, gameObject.transform.position) / (1 + useFireBallSpeed * 100);

        //Do the Fire AOE attack when set
        //if (fireAOE && !dead)
        //{
        //    FireAOE();
        //}
        
        //If Hydra is not doing their Special Attack
        if (!sAttack && !dead)
        {
            //==================================================================================================
            //Rotation

            hydraHead.transform.LookAt(target.transform.position);
            
            //Get the direction of the Target
            Vector3 targetDir = hydraBody.transform.position - target.transform.position;

            // Set step equal to speed times frame time.
            float step = turnSpeed * Time.deltaTime;
            
            //Have object lerp towards the target Direction 
            Vector3 newDir = Vector3.RotateTowards(hydraBody.transform.forward, targetDir, step, 0.0f);

            //Run the 3 combined lines of code
            hydraBody.transform.rotation = Quaternion.LookRotation(newDir);
            //==================================================================================================
            //Run the Aggro System
            Aggro_System();
            //==================================================================================================
        }

        //Play Sound Once
        if (playSound)
        {
            //audioSource.clip = audioClips[soundNo];
            audioSource.Play();
        }

        //If the Hydra runs out of health
        if (GetComponent<Boss_Health>().health <= 0 && !dead)
        {
            StopAllCoroutines();
            StartCoroutine(Die()); //Die
        }
    }

    //-----------------------------------------------------------------------------------------------------------------------------------------------------
    //Function for deciding who the AI should target. Sondra or Wolf?
    void Judgement()
    {
        CameraTargetController camTarCon = GameObject.FindObjectOfType<CameraTargetController>();
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

    //Decide what attack to use based on Time and Distance
    public void Aggro_System()
    {
        //If aggro is set to true
        if (aggro)
        {
            //Check if Wolf and Sondra exist first
            if (sondra == null || wolf == false)
            {
                aggro = false;
                Debug.LogError("Genbu doesn't know where either Sondra or Wolf or both of them are!");
            }

            //Else decide who should the AI should target
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

            //Timer used for Hydra's attack
            timer += Time.deltaTime; 

            // If Timer's more than Attack Delay then choose whether to use a Fireball or Special Attack
            if (timer > attackDelay)
            {
                
                //If the target's out of the Special Attack Distance, shoot a Fireball
                if (Vector3.Distance(gameObject.transform.position, target.transform.position) > specialAttackDistance)
                {
                    StartCoroutine(ArcFireBall(fireBallPrefab, fireBallTimeToTarget, fireBallDamage));
                    timer = 0;
                }

                //Else use a Special Attack based on what was chosen in the Inspector
                else if (Vector3.Distance(gameObject.transform.position, target.transform.position) <= specialAttackDistance)
                {
                    if (specialAttack == SpecialAttack.NeckSlam)
                    {
                        StartCoroutine(NeckSlam());
                    }

                    else if (specialAttack == SpecialAttack.FireAOE)
                    {
                        StartCoroutine(FireAOE());
                        fireAOETimer = 0;
                    }

                    else if (specialAttack == SpecialAttack.Eruption)
                    {
                        StartCoroutine(Eruption());
                    }
                    timer = -10;
                }
            }
        }
    }

    //Function for Instantiating a Fireball
    public IEnumerator ArcFireBall(GameObject firePrefab, float travelTime, int damage)
    {
        playSound = true; //Play Sound
        soundNo = 1; //Fireball Sound

        GameObject fireball = Instantiate(firePrefab, fireballSpawner.transform.position, fireballSpawner.transform.rotation);
        fireball.GetComponent<Rigidbody>().velocity = ParabolicEquation(travelTime); //Value is calculated from the "ParabolicEquation" function
        fireball.GetComponent<Fireball>().damage = damage;
        yield return null;
    }

    //Function for the NeckSlam attack
    private IEnumerator NeckSlam()
    {
        neckSlam = true;
        sAttack = true;

        playSound = true; //Play Sound
        soundNo = 4; //Neck Slam Startup Sound
        Debug.Log("Waiting for Set-Up Animation");
        yield return new WaitForSeconds(1);

        //While the Hydra's not within Sondra's Attack Distance
        while(Vector3.Distance(transform.position, originalPos) < afterSpecialMoveDistance)
        {
            transform.position -= hydraBody.transform.forward * moveSpeed * Time.deltaTime; //Move into Sondra's Attack Distance
            yield return new WaitForEndOfFrame();
        }

        playSound = true; //Play Sound
        soundNo = 5; //Neck Slam SLAM* Sound
        Debug.Log("Waiting for HeadSlam Animation");

        yield return new WaitForSeconds(4);

        while (Vector3.Distance(transform.position, originalPos) > (1 + moveSpeed * Time.deltaTime))
        {
            transform.position += hydraBody.transform.forward * moveSpeed * Time.deltaTime; //Move out of Sondra's Attack Distance
            yield return new WaitForEndOfFrame();
        }

        transform.position = originalPos;
        Debug.Log("Waiting for Recovery Animation");
        neckSlam = false;
        sAttack = false;

        yield return null;
    }

    //Function for doing the Fire AOE attack
    IEnumerator FireAOE()
    {
        sAttack = true;

        float step = turnSpeed * Time.deltaTime;

        Vector3 targetDir;
        Vector3 newDir;
        int point = 0;

        while (fireAOETimer < 1)
        {
            fireAOETimer += Time.deltaTime;
            point = 0;
            //step *= 5;
            targetDir = hydraBody.transform.position - fireBreathTurnPoints[point].transform.position;
            newDir = Vector3.RotateTowards(hydraBody.transform.forward, targetDir, step, 0.0f);
            hydraBody.transform.rotation = Quaternion.LookRotation(newDir);
            yield return new WaitForFixedUpdate();
        }

        fireAOETimer = 0;
        specialAttackEffect.SetActive(true);
        yield return new WaitForSeconds(1);

        while (fireAOETimer < 1)
        {
            fireAOETimer += Time.deltaTime;
            point = 1;
            targetDir = hydraBody.transform.position - fireBreathTurnPoints[point].transform.position;
            newDir = Vector3.RotateTowards(hydraBody.transform.forward, targetDir, step, 0.0f);
            hydraBody.transform.rotation = Quaternion.LookRotation(newDir);
            yield return new WaitForFixedUpdate();
        }

        fireAOETimer = 0;

        while (fireAOETimer < 2)
        {
            point = 2;
            fireAOETimer += Time.deltaTime;
            targetDir = hydraBody.transform.position - fireBreathTurnPoints[point].transform.position;
            newDir = Vector3.RotateTowards(hydraBody.transform.forward, targetDir, step, 0.0f);
            hydraBody.transform.rotation = Quaternion.LookRotation(newDir);
            
            yield return new WaitForFixedUpdate();
        }

        while (Vector3.Distance(transform.position, originalPos) < afterSpecialMoveDistance)
        {
            targetDir = hydraBody.transform.position - fireBreathTurnPoints[point].transform.position;
            specialAttackEffect.SetActive(false); //Turn off Hitbox
            transform.position -= targetDir.normalized * moveSpeed * Time.deltaTime; //Move into Sondra's Attack Distance
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(3);

        fireAOETimer = 0;

        while (Vector3.Distance(transform.position, originalPos) > (1 + moveSpeed * Time.deltaTime)) //&& fireAOETimer < 1.5f)
        {
            fireAOETimer += Time.deltaTime;
            targetDir = hydraBody.transform.position - fireBreathTurnPoints[point].transform.position;
            transform.position += targetDir.normalized * moveSpeed * Time.deltaTime; //Move out of Sondra's Attack Distance
            yield return new WaitForEndOfFrame();
        }
        transform.position = originalPos;
        fireAOE = false;
        sAttack = false;
        specialAttackEffect.SetActive(false);
        yield return null;
    }

    //Function for doing the Eruption Attack
    private IEnumerator Eruption()
    {
        sAttack = true;
        eruption = true;
        Debug.Log("PREPARING TO EXPLODE");
        yield return new WaitForSeconds(5);

        Debug.Log("EXPLODING!");
        specialAttackEffect.SetActive(true);
        yield return new WaitForSeconds(5);
        specialAttackEffect.SetActive(false);

        //While the Hydra's not within Sondra's Attack Distance
        while (Vector3.Distance(transform.position, originalPos) < afterSpecialMoveDistance)
        {
            transform.position -= hydraBody.transform.forward * moveSpeed * Time.deltaTime; //Move into Sondra's Attack Distance
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(5);

        float eruptionTimer = 0;
        while (Vector3.Distance(transform.position, originalPos) > (1 + moveSpeed * Time.deltaTime)) //&& eruptionTimer < 0.5f)
        {
            eruptionTimer += Time.deltaTime;
            transform.position += hydraBody.transform.forward * moveSpeed * Time.deltaTime; //Move out of Sondra's Attack Distance
            yield return new WaitForEndOfFrame();
        }
        
        sAttack = false;
        eruption = false;

        yield return null;
    }

    public IEnumerator Stun()
    {
        stunned = true;
        sAttack = true;
        eruption = false;
        fireAOE = false;
        neckSlam = false;
        
        if (specialAttackEffect != null)
            specialAttackEffect.SetActive(false);

        //While the Hydra's not within Sondra's Attack Distance
        hydraBody.transform.localEulerAngles = new Vector3(-25, hydraBody.transform.localEulerAngles.y, hydraBody.transform.localEulerAngles.z);
        while (Vector3.Distance(transform.position, originalPos) < afterSpecialMoveDistance)
        {
            transform.position -= hydraBody.transform.forward * (moveSpeed) * Time.deltaTime; //Move into Sondra's Attack Distance
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(10);
        
        while (Vector3.Distance(transform.position, originalPos) > (1 + moveSpeed * Time.deltaTime)) //&& eruptionTimer < 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPos, (moveSpeed) * Time.deltaTime); ; //Move out of Sondra's Attack Distance
            yield return new WaitForEndOfFrame();
        }
        nextStun = GetComponent<Boss_Health>().health - damageToStun;
        sAttack = false;
        stunned = false;
        yield return null;
    }

    public IEnumerator Die()
    {
        dead = true;
        print("Dead!!");
        hydraBody.transform.localEulerAngles = new Vector3(0, 0, 0);
        Debug.Log("I'm DEAD, PLAY ANIMATION");

        Hydra_AI[] everyHydra = GameObject.FindObjectsOfType<Hydra_AI>();

        foreach(Hydra_AI hydra in everyHydra)
        {
            hydra.remainingHydras -= 1;
        }

        for (int x = 0; x < _objectiveSystem._Objectives.Count; x++) {
            //If this objective is not complete
            if (_objectiveSystem._Objectives[x].bIsCompleted != true)
            {
                print("Updated objective system");
                _objectiveSystem._Objectives[x].bIsCompleted = true; //Check of the objective
                //_objectiveSystem.UpdateObjectives(); //UPdate the system
                x = _objectiveSystem._Objectives.Count + 1; //Leave this for loop
            }
        }

        yield return new WaitForSeconds(1);

        Destroy(gameObject);
        yield return null;
    }

    //====================================================================================================================================
    // Math Stuff
    //====================================================================================================================================
    Vector3 ParabolicEquation (float timeToTarget)
    {
        // PARABOLIC EQUATION 1
        /*
        //Using a Parabolic Equation, this will calculate the speed the projectile must travel (according to the angle) for it to land on the target

        //Get the direction
        Vector3 direction = target.position - hydraBody.transform.position;

        //Get the height difference
        float height = direction.y;

        //Get horizontal distance
        direction.y = 0;
        float distance = direction.magnitude;

        //Convert angle to radian, then sets direction to the elevation angle
        float a = angle * Mathf.Deg2Rad;
        direction.y = distance * Mathf.Tan(a);

        //Correct height difference
        distance += height / Mathf.Tan(a);

        //And finally! Calculate the velocity magnitude and return it as a value
        float vel = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2 * a));
        return vel * direction.normalized;
        */

        // PARABOLIC EQUATION 2
        Vector3 AtoB = target.transform.position - fireballSpawner.transform.position;
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
}
