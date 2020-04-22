using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPoints : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;

    public float speed;
    public float startTime;
    public float moveLenght;

    public bool switchMovement;


    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        switchMovement = false;
        moveLenght = Vector3.Distance(endPoint.position, startPoint.position);
    }

    void LateUpdate()
    {
        if(switchMovement == false)
        {
            float distanceCovered = (Time.time - startTime) * speed;
            float fractionOfJourney = distanceCovered / moveLenght;
            transform.position = Vector3.Lerp(startPoint.position, endPoint.position, fractionOfJourney);
        }
        if (switchMovement == true)
        {
            float distanceCovered = (Time.time - startTime) * speed;
            float fractionOfJourney = distanceCovered / moveLenght;
            transform.position = Vector3.Lerp(endPoint.position,startPoint.position, fractionOfJourney);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
