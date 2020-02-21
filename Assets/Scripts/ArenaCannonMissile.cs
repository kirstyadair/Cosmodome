using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaCannonMissile : MonoBehaviour
{
    Vector3 startPosition;
    Quaternion startRotation;
    Transform startParent;

    public PlayerScript firer;
    public GameObject fireFX;
    public GameObject spawnExplosionGO;
    Vector3 fireDirection;
    public bool isFired = false;
    public float fireSpeed = 1f;
    public float velocity = 0;
    public float lifetime = 5f;
    float timeAlive = 0;
    public float smashForce;

    public delegate void FireCannon();
    public static event FireCannon OnFireCannon;

    public void OnTriggerEnter(Collider other)
    {
        if (!isFired) return;

        if (other.CompareTag("Ship") && other.GetComponent<PlayerScript>() != firer)
        {
            Hit(other.GetComponent<PlayerScript>());
        }
    }

    public void Hit(PlayerScript target)
    {
        target.GetComponent<Rigidbody>().AddForce(fireDirection * smashForce, ForceMode.Impulse);
        GameObject explosion = Instantiate(spawnExplosionGO);
        explosion.transform.position = this.transform.position;
        target.WasHitWithArenaCannon();
        Restore();
    }

    public void Restore()
    {
        firer = null;
        velocity = 0;
        this.transform.SetParent(startParent);
        this.transform.localPosition = startPosition;
        this.transform.localRotation = startRotation;
        isFired = false;
        fireFX.SetActive(false);
    }

    public void Fire(Vector3 direction, PlayerScript firer)
    {
        OnFireCannon.Invoke();
        this.firer = firer;
        direction.y = 0;//this.transform.position.y;
        startParent = this.transform.parent;
        startPosition = this.transform.localPosition;
        startRotation = this.transform.localRotation;

        GameObject explosion = Instantiate(spawnExplosionGO);
        fireFX.SetActive(true);
        explosion.transform.position = this.transform.position;
        isFired = true;
        this.transform.SetParent(null, true);
        fireDirection = direction;
        timeAlive = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFired)
        {
            velocity += fireSpeed;
            this.transform.right = -fireDirection;
            this.transform.Translate(Vector3.left * velocity * Time.deltaTime, Space.Self);
            timeAlive += Time.deltaTime;

            if (timeAlive >= lifetime)
            {
                Restore();
            }
        }
    }
}
