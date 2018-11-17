using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CamCollisionDetection : MonoBehaviour {

    private CameraTargetController camTarCon;

    public Image zoomCrosshair;

    public float minDistance = 1.0f;
    public float maxDistance = 6.0f;

    private float startingMinDistance;
    private float startingMaxDistance;

    public float smooth = 10.0f;

    Vector3 dollyDir;
    Vector3 targetPos;
    Vector3 camPos;
    float distance;

    public bool zoom;

    void Awake()
    {
        dollyDir = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
    }

    void Start()
    {
        startingMinDistance = minDistance;
        startingMaxDistance = maxDistance;

        zoom = false;
        zoomCrosshair.enabled = false;
        camTarCon = GameObject.FindObjectOfType<CameraTargetController>();
    }

    void Update()
    {
        //Camera Zoom Part 2
        if (zoom)
        {
            minDistance = -2;
            maxDistance = -2;
            if (transform.localPosition.z > 0)
            {
                zoomCrosshair.enabled = true;
            }

            else
            {
                zoomCrosshair.enabled = false;
            }
        }

        else
        {
            minDistance = startingMinDistance;
            maxDistance = startingMaxDistance;
            zoomCrosshair.enabled = false;
        }

        targetPos = camTarCon.cameraTarget.position + new Vector3(0, 0.05f, 0);
        Vector3 desiredCameraPos = transform.parent.TransformPoint(dollyDir * maxDistance);
        RaycastHit hit;

        if (Physics.Linecast(targetPos, desiredCameraPos, out hit) && (hit.transform.GetComponent<Collider>() && !hit.transform.GetComponent<Collider>().isTrigger))
        {
            distance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        }

        else
        {
            distance = maxDistance;
        }

        if (!zoom)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, (dollyDir * distance), Time.deltaTime * smooth);
        }

        else
        {
            if (camTarCon.cameraTarget.parent.GetComponent<WolfFly>() && camTarCon.cameraTarget.parent.GetComponent<WolfFly>().fly)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, (dollyDir * distance) + new Vector3(0.25f, 1.35f, 0), Time.deltaTime * smooth);
            }
            else
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, (dollyDir * distance) + new Vector3(0.25f, 2.25f, 0), Time.deltaTime * smooth);
            }
        }
    }
}
