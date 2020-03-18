using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeWallScript : MonoBehaviour
{
    Vector3 desiredVelocity;
    Vector3 targetPoint;
    Rigidbody rb;
    float magnitudeOfShip;
    Vector3 directionOfShip;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        targetPoint = Vector3.zero;
    }

    void Update()
    {
        // Return to centre point if not already there
        SeekCentrePoint();
    }

    void SeekCentrePoint()
    {
        desiredVelocity = Vector3.Normalize(targetPoint - transform.localPosition) * 0.15f;
        transform.localPosition += desiredVelocity;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ship"))
        {   
            Rigidbody shipRB = collision.gameObject.GetComponent<Rigidbody>();
            ShipController sc = collision.gameObject.GetComponent<ShipController>();
            magnitudeOfShip = shipRB.velocity.magnitude;
            directionOfShip = Vector3.Normalize(collision.gameObject.transform.position - transform.position);
            sc.PushBack(directionOfShip * magnitudeOfShip);
        }
    }
}
