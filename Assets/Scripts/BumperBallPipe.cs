using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperBallPipe : MonoBehaviour
{
    public bool isFiring = false;
    Animator animator;
    public GameObject bumperBallPrefab;
    public Transform bumperBallSpawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // play the warning animation first, then shoot the ball
    // time is how long before firing the bumper ball
    public void StartFiring(float timeToWarnFor)
    {
        isFiring = true;
        animator.SetTrigger("Warning");
        StartCoroutine(StartFiringAfter(timeToWarnFor));
    }

    IEnumerator StartFiringAfter(float time)
    {
        yield return new WaitForSeconds(time);
        animator.SetTrigger("Fire");
    }

    void SpawnBumperBall()
    {
        isFiring = false;

        GameObject bb = Instantiate(bumperBallPrefab);
        bb.GetComponent<BumperBall>().Spawn(bumperBallSpawnPoint.position, bumperBallSpawnPoint.forward);
    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
