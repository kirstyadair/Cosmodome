using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaSpotlightFollow : MonoBehaviour
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

        if (distance >= track.MaxPos) speed = -Mathf.Abs(speed);
        if (distance <= track.MinPos) speed = Mathf.Abs(speed);

        distance += speed * Time.deltaTime;
        Vector3 trackedPosition = track.EvaluatePosition(distance);


        this.transform.up = this.transform.position - trackedPosition;
    }
}
