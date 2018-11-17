using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile_AI : MonoBehaviour {

    public float rocketSpeed;
    public float turnSpeed;
	public bool setMovementOnStart = true;
    public GameObject target;
    public GameObject explosion;

    [HideInInspector]
    public int damage;

    private Rigidbody rb;
    private float heatSeakTime;
	private float comingForThatBootyTimer;

	private float origTS;

    private bool dummy;

    float distance;

    // Use this for initialization
    void Start () {
        
		origTS = turnSpeed;

        //target = GameObject.FindGameObjectWithTag("Player");
        //if (!GetComponent<Rigidbody>())
        //{
        //    gameObject.AddComponent<Rigidbody>();
        //}

        //rb = gameObject.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        distance = Vector3.Distance(transform.position, target.transform.position);
        heatSeakTime += Time.deltaTime;

        if (heatSeakTime > 1 && !dummy)
        {
            Turn();
        }
        Move();

        if (heatSeakTime > 5)
        {
            Explode();
        }
    }

    void Turn()
    {
        float tS = turnSpeed;

        if (distance > 100)
        {
            tS = turnSpeed * (distance / 25);
        }

        Vector3 targetDir = target.transform.position - gameObject.transform.position;
		Vector3 newDir = Vector3.RotateTowards (transform.forward, targetDir, tS * Time.deltaTime, 0.0f);
        
		if (target.name == "SondraL1" && comingForThatBootyTimer < 3 && setMovementOnStart) 
		{
			comingForThatBootyTimer += Time.deltaTime;
			newDir.y += 500;
		} 

		else if ((target.name == "SondraL1" && comingForThatBootyTimer >= 2 && setMovementOnStart) || !setMovementOnStart) 
		{
			rocketSpeed += Time.deltaTime;
			turnSpeed = origTS / 2;
		}

        if (distance < 30)
        {
            dummy = true;
        }

        gameObject.transform.rotation = Quaternion.LookRotation(newDir);
    }

    void Move()
    {
        float rS = rocketSpeed;
        if (heatSeakTime > 1)
        {
            if (distance > 100)
            {
                rS = rocketSpeed * (distance / 100);
            }

            else
            {
                rS = rocketSpeed;
            }
        }
        gameObject.transform.position += transform.forward * rS * Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
		if (other.gameObject.tag != "Projectile" && other.gameObject.tag != "Skybox" && other.gameObject.name != "Colliders" && other.GetComponent<Collider>().isTrigger == false)
        {
            Explode();
        }
    }

    void Explode()
    {
        GameObject explo = Instantiate(explosion, transform.position, transform.rotation);
        explo.GetComponent<Hitbox>().damage = damage;
        Destroy(gameObject);
    }
}
