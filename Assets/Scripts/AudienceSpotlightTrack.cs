using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudienceSpotlightTrack : MonoBehaviour
{
    public CinemachineSmoothPath track;
    public float speed = 1f;
    public float distance = 0;
    public float invertChance = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0f, 1f) < invertChance) speed = -speed;

        distance += speed * Time.deltaTime;
        Vector3 trackedPosition = track.EvaluatePosition(distance);
        trackedPosition.y = this.transform.position.y;

        this.transform.position = trackedPosition;
    }
}
