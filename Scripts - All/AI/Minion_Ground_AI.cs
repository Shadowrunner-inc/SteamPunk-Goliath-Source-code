using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Minion_Ground_AI : MonoBehaviour {

    public GameObject target;
    public Hitbox hitbox;
    public float moveSpeed = 5;
    public int damage = 8;

    public float stoppingDistance = 10;

    [HideInInspector]
    public bool canAttack;

    private NavMeshAgent nav;

    private bool pauseUpdate;

    Minion_Manager senpai;

    void Start()
    {
        hitbox.enabled = false;
        nav = GetComponent<NavMeshAgent>();
        target = GameObject.Find("WolfL3");

        senpai = FindObjectOfType<Minion_Manager>();
        senpai.minions.Add(this.GetComponent<Minion_Ground_AI>());//Regikster with the boss

        //Add to Manager
    }

    void FixedUpdate ()
    {
        if (!pauseUpdate)
        {
            if (nav.destination != target.transform.position)
            {
                Chase();
            }

            if (Vector3.Distance(transform.position, target.transform.position) <= stoppingDistance)
            {
                StartCoroutine(Attack());
            }

            transform.LookAt(target.transform.position);
        }
	}

    void Chase()
    {
        nav.speed = moveSpeed;
        nav.destination = target.transform.position;
        nav.stoppingDistance = stoppingDistance;
    }

    IEnumerator Attack()
    {
        pauseUpdate = true;

        if (senpai.NoticeMe(GetComponent<Minion_Ground_AI>(), target))
        {
            hitbox.enabled = true;
            hitbox.transform.GetComponent<Collider>().enabled = true; //Turn the Hitbox on!
            hitbox.damage = damage;
            Debug.Log("Hit! -" + gameObject.name);
            //Play Attack Animation
        }

        yield return new WaitForSeconds(2);

        hitbox.enabled = false;
        hitbox.transform.GetComponent<Collider>().enabled = false; //Turn the Hitbox off!

        pauseUpdate = false;
        yield return null;
    }
}
