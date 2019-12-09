using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperBall : MonoBehaviour
{
    Rigidbody rb;

    public delegate void BumperBallEvent();
    public static BumperBallEvent OnBumperBallHitPlayer;
    public static BumperBallEvent OnBumperBallHitWall;
    public static BumperBallEvent OnBumperBallExplode;
    public static BumperBallEvent OnBumperBallFire;

    public GameObject hitWallParticleFX;
   
    [Header("The force the bumper ball should be shot out with")]
    public float spawnForce;

    [Header("How long the ball has to live, if it doesn't bounce long enough")]
    public float timeAliveBeforeExploding;
    float timeAlive;
    
    [Header("How many times the ball will bounce off something before exploding")]
    public int bouncesBeforeExplode;

    int bounces = 0;

    public void Start()
    {
       
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ship"))
        {
            OnBumperBallHitPlayer?.Invoke();
        } else
        {
            OnBumperBallHitWall?.Invoke();
            bounces++;

            if (bounces >= bouncesBeforeExplode)
            {
                Explode();
            }
        }

        GameObject particleFX = Instantiate(hitWallParticleFX);
        particleFX.transform.position = collision.GetContact(0).point;
    }

    public void Spawn(Vector3 position, Vector3 direction)
    {
        OnBumperBallFire?.Invoke();
        this.transform.position = position;
        this.transform.forward = direction;
        rb = GetComponent<Rigidbody>();
        rb.AddForce(this.transform.forward * spawnForce, ForceMode.VelocityChange);
    }

    public void Explode()
    {
        OnBumperBallExplode?.Invoke();
        Destroy(gameObject);
    }

    private void Update()
    {
        timeAlive += Time.deltaTime;
        if (timeAlive >= timeAliveBeforeExploding)
        {
            Explode();
        }
    }
}
