using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperBall : MonoBehaviour
{
    Rigidbody rb;

    public delegate void BumperBallEvent();
    public delegate void BumperBallPlayerEvent(PlayerScript player);
    public static BumperBallPlayerEvent OnBumperBallHitPlayer;
    public static BumperBallEvent OnBumperBallHitWall;
    public static BumperBallEvent OnBumperBallExplode;
    public static BumperBallEvent OnBumperBallFire;
    public static BumperBallPlayerEvent OnBumperBallExplodeOnPlayer;
    public GameObject hitWallParticleFX;
    public GameObject explosionFX;
    public GameObject trail;

    bool _isDestroyed = false;
   
    [Header("The force the bumper ball should be shot out with")]
    public float spawnForce;

    [Header("How long the ball has to live, if it doesn't bounce long enough")]
    public float timeAliveBeforeExploding;
    float timeAlive;
    
    [Header("How many times the ball will bounce off something before exploding")]
    public int bouncesBeforeExplode;

    [Header("How much to fling the ball in an impact so it doesnt get stuck")]
    public float bouncificier;

    [Header("Explosion force")]
    public float explosionForce;

    [Header("Explosion radius")]
    public float explosionRadius;

    int bounces = 0;

    public void OnCollisionEnter(Collision collision)
    {
        if (_isDestroyed) return;
        if (collision.gameObject.CompareTag("Ship"))
        {
            OnBumperBallHitPlayer?.Invoke(collision.gameObject.GetComponent<PlayerScript>());
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

        // to stop it getting stuck in a rut
        rb.AddForce(new Vector3(Random.Range(-bouncificier, bouncificier), 0, Random.Range(-bouncificier, bouncificier)), ForceMode.Impulse);
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
        _isDestroyed = true;

        OnBumperBallExplode?.Invoke();
        GameObject fx = Instantiate(explosionFX);
        fx.transform.position = this.transform.position;
        trail.GetComponent<TrailRenderer>().autodestruct = true;
        trail.transform.SetParent(null);
       

        foreach (PlayerScript player in ScoreManager.Instance.players)
        {
            player.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, this.transform.position, explosionRadius);
            if ((player.transform.position - this.transform.position).magnitude < explosionRadius)
            {
                OnBumperBallExplodeOnPlayer?.Invoke(player);
            }
        }

        Debug.Log("Destroying ya boi " + gameObject.name, gameObject);
        Destroy(gameObject);
    }

    private void Update()
    {
         if (_isDestroyed) return;

        timeAlive += Time.deltaTime;
        if (timeAlive >= timeAliveBeforeExploding)
        {
            Explode();
        }
    }
}
