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
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        targetPoint = new Vector3 (0, 0.5f, 0);
    }

    void Update()
    {
        // Return to centre point if not already there
        if (Vector3.Distance(transform.localPosition, targetPoint) > 0.1f) SeekCentrePoint();
    }

    void SeekCentrePoint()
    {
        desiredVelocity = Vector3.Normalize(targetPoint - transform.localPosition) * 0.2f;
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
            directionOfShip.y = 0.5f;

            sc.PushBack(directionOfShip * 50);
        }
    }
}
