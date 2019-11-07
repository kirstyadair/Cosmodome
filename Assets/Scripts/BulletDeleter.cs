using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDeleter : MonoBehaviour
{
    public float timeToDie = 3f;
    public float bulletForce = 1f;
    float timeAlive = 0.0f;
    public GameObject shooter;

    // Update is called once per frame
    void Update()
    {
        timeAlive += Time.deltaTime;
        if (timeAlive >= timeToDie)
        {
            Destroy(this.gameObject);
        }

        this.transform.position += this.transform.forward * bulletForce;
    }

    public void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ship") && other.gameObject != shooter)
        {
            other.GetComponent<ShipController>().HitByBullet(this);
            Destroy(this.gameObject);
        }
    }
}
