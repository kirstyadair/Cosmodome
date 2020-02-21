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
        if (timeAlive >= timeToDie)
        {
            if (this.gameObject.name != "Laser") Destroy(this.gameObject);
            else if (this.gameObject.GetComponent<Animator>() != null)
            {
                
                this.gameObject.GetComponent<Animator>().SetBool("LaserOn", false);
                timeAlive = 0;
                this.enabled = false;
            } 
        }
        else timeAlive += Time.deltaTime;

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
            if (this.gameObject.name != "Laser") other.GetComponent<ShipController>().HitByBullet(this);
            else if (this.gameObject.GetComponent<Animator>().GetBool("LaserOn"))
            {
                other.GetComponent<ShipController>().HitByBullet(this);
                Debug.Log("hit");
            }

            if (this.gameObject.name != "Laser") Destroy(this.gameObject);
        }
    }
}
