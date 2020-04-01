using System.Collections;
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
    Animator animator;

    void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeAlive >= timeToDie)
        {
            if (this.gameObject.name != "Laser") Destroy(this.gameObject);
            else if (animator != null)
            {
                
                animator.SetBool("LaserOn", false);
                timeAlive = 0;
                this.enabled = false;
            } 
        }
        else timeAlive += Time.deltaTime;

        if (this.gameObject.name != "Laser") this.transform.position += this.transform.forward * bulletForce;
    }

    void OnBulletCollidedWithSomething(GameObject other)
    {
        if (other.CompareTag("Bullet")) return;
        if (other.gameObject.CompareTag("Ship") && other.gameObject == shooter) return;

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
                other.GetComponent<ShipController>().PushBack((other.transform.position - this.transform.position) * _pushbackPower);
            }
            else
            {
                Debug.Log("A");
                other.GetComponent<ShipController>().HitByBullet(this);
                other.GetComponent<ShipController>().PushBack((other.transform.position - this.transform.position) * _pushbackPower);
            }

            if (this.gameObject.name != "Laser") Destroy(this.gameObject);
        }

        if (this.gameObject.name != "Laser") Destroy(this.gameObject);
    }

    public void OnCollisionEnter(Collision collision)
    {
        OnBulletCollidedWithSomething(collision.gameObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        OnBulletCollidedWithSomething(other.gameObject);
    }
}
