using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceholderMovementScript : MonoBehaviour
{
    public float force;
    public float bulletSpeed;
    public GameObject bulletPrefab;
    public GameObject bulletSpawn;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        // Shoot
        if (Input.GetButton("Fire1"))
        {
            // Find mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);

            Vector3 mousePos = hit.point;
            Vector3 mouseDirection = mousePos - transform.position;
            mouseDirection.y = 0.0f;
            mouseDirection.Normalize();

            // Spawn in "bullet"
            GameObject bullet = GameObject.Instantiate(bulletPrefab, bulletSpawn.transform.position, Quaternion.identity);

            // Fire in direction of mouse pointer
            bullet.GetComponent<Rigidbody>().AddForce(mouseDirection * bulletSpeed);
        }
    }


    void Update()
    {
        // Player 1 movement
        if (name == "Player1")
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                rb.AddForce(Vector3.left * force);
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                rb.AddForce(Vector3.right * force);
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                rb.AddForce(Vector3.forward * force);
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                rb.AddForce(Vector3.back * force);
            }
        }
        // Player 2 movement
        else if (name == "Player2")
        {
            if (Input.GetKey(KeyCode.A))
            {
                rb.AddForce(Vector3.left * force);
            }

            if (Input.GetKey(KeyCode.D))
            {
                rb.AddForce(Vector3.right * force);
            }

            if (Input.GetKey(KeyCode.W))
            {
                rb.AddForce(Vector3.forward * force);
            }

            if (Input.GetKey(KeyCode.S))
            {
                rb.AddForce(Vector3.back * force);
            }
        }

        

    }
}
