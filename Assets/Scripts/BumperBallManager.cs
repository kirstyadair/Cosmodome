using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperBallManager : MonoBehaviour
{
    [TextArea]
    public string Help = "If you want to change the properties of the bumper ball, such as live time and explosion force, open the bumper ball prefab";

    public BumperBallPipe[] bumperBallPipes;
    public delegate void BumperBallShoot();
    public static event BumperBallShoot OnBumperBallShoot;

    [Header("Should fire a bumper ball every X seconds")]
    public float fireEverySeconds;

    [Header("Chance of firing multiple bumper balls at once")]
    public float chanceOfFiringMultiple;

    [Header("How much warning to give before firing the bumper ball")]
    public float warningTime;

    // how much time has passed before we've fired a bumper ball
    float sinceLastFired = 0;

    void Fire()
    {
        OnBumperBallShoot.Invoke();
        if (Random.Range(0f, 1f) <= chanceOfFiringMultiple) Fire();

        List<BumperBallPipe> pipesReadyToFire = new List<BumperBallPipe>();
        foreach (BumperBallPipe pipe in bumperBallPipes)
        {
            if (!pipe.isFiring) pipesReadyToFire.Add(pipe);
        }

        if (pipesReadyToFire.Count == 0) return;

        BumperBallPipe chosenPipe = pipesReadyToFire[Random.Range(0, pipesReadyToFire.Count)];

        chosenPipe.StartFiring(warningTime);
    }

    void Update()
    {
        sinceLastFired += Time.deltaTime;

        if (sinceLastFired >= fireEverySeconds)
        {
            sinceLastFired = 0;
            Fire();
        }
    }
}
