using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDeleter : MonoBehaviour
{
    public float timeToDie;
    public float bulletForce = 1f;
    float timeAlive = 0.0f;
    public GameObject shooter;

    // Update is called once per frame
    void Update()
    {
        timeAlive += Time.deltaTime;
        if (timeAlive >= timeToDie)
        {
            if (this.gameObject.name != "Laser") Destroy(this.gameObject);
            else this.gameObject.SetActive(false);
        }

        if (this.gameObject.name != "Laser") this.transform.position += this.transform.forward * bulletForce;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (this.gameObject.name != "Laser") Destroy(this.gameObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ship") && other.gameObject != shooter)
        {
            other.GetComponent<ShipController>().HitByBullet(this);

            if (this.gameObject.name != "Laser") Destroy(this.gameObject);
        }
    }
}
