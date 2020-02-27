﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDeleter : MonoBehaviour
{
    [SerializeField]
    [Header("The prefab to reparent to the scene (like an explosion PS) on collision")]
    GameObject _collisionExplosion;

    [SerializeField]
    [Header("The amount to push back the hit player by")]
    float _pushbackPower;

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
        GameObject other = collision.gameObject;

        if (collision.gameObject.CompareTag("Ship") && collision.gameObject == shooter) return;

        if (_collisionExplosion != null)
        {
            _collisionExplosion.SetActive(true);
            _collisionExplosion.transform.SetParent(null, true);
        }

        if (other.CompareTag("Ship") && other.gameObject != shooter)
        {
            if (this.gameObject.name == "Laser")
            {
                other.GetComponent<ShipController>().HitByBullet(this);
            }
            else
            {
                other.GetComponent<ShipController>().HitByBullet(this);
                other.GetComponent<ShipController>().PushBack((other.transform.position - this.transform.position) * _pushbackPower);
            }

             Destroy(this.gameObject);
        }

        if (this.gameObject.name != "Laser") Destroy(this.gameObject);
    }
}
