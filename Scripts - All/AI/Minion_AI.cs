using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Minion_AI : MonoBehaviour {

    public GameObject target;

    public float hoverDistance;
    public float wallDetectionDistance;
    public float hoverSpeed;

    private bool goUp;

    public GameObject groundTarget;

    void Start()
    {
        GameObject groundTar = Instantiate(groundTarget);
        groundTar.GetComponent<Minion_Ground_AI>().target = target;
        target = groundTar;
    }

    void Update ()
    {
        Vector3 pos = gameObject.transform.parent.transform.position;
        pos.y = gameObject.transform.position.y;

        gameObject.transform.position = pos;

        MoveTurtle();
        gameObject.transform.LookAt(target.transform.position);
	}

    void MoveTurtle()
    {
        RaycastHit hit;
        RaycastHit hit2;
        if (Physics.Raycast(transform.position, transform.forward, out hit, wallDetectionDistance) && (!Physics.Raycast(transform.position, Vector3.up, out hit2, hoverDistance)))
        {
            Debug.Log("A");
            if (hit.transform.gameObject != target && hit.transform.tag != "Minion")
                transform.position += transform.up * hoverSpeed * Time.deltaTime;
            else
            {
                if (Physics.Raycast(transform.position, Vector3.up, out hit, hoverDistance))
                {
                    transform.position += transform.up * hoverSpeed * Time.deltaTime;
                }

                if (!Physics.Raycast(transform.position, -Vector3.up, out hit, hoverDistance))
                {
                    transform.position -= transform.up * hoverSpeed * Time.deltaTime;
                }
            }
        }

        else
        {
            Debug.Log("B");
            if (Physics.Raycast(transform.position, Vector3.up, out hit, hoverDistance))
            {
                transform.position += transform.up * hoverSpeed * Time.deltaTime;
            }

            if (!Physics.Raycast(transform.position, -Vector3.up, out hit, hoverDistance))
            {
                transform.position -= transform.up * hoverSpeed * Time.deltaTime;
            }
        }
    }
}
