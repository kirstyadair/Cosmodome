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
    Rigidbody shipRB;
    bool shipHit = false;
    // This stores the mass prior to the change being made so it can be reset
    public float prevShipMass;

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
        if (Vector3.Distance(transform.localPosition, targetPoint) > 0.2f) SeekCentrePoint();
        else if (shipHit) PushShip();
    }

    void SeekCentrePoint()
    {
        if (shipHit)
        {
            shipRB.mass = 0.5f;
        }
        desiredVelocity = Vector3.Normalize(targetPoint - transform.localPosition) * 0.3f;
        transform.localPosition += desiredVelocity;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ship"))
        {   
            shipRB = collision.gameObject.GetComponent<Rigidbody>();
            if (shipRB.velocity.magnitude > 10)
            {
                if (!shipHit) prevShipMass = shipRB.mass;
                shipHit = true;
            }
        }
    }

    void PushShip()
    {
        shipHit = false;
        shipRB.mass = prevShipMass;
        ShipController sc = shipRB.gameObject.GetComponent<ShipController>();
        magnitudeOfShip = shipRB.velocity.magnitude;
        directionOfShip = Vector3.Normalize(shipRB.gameObject.transform.position - transform.position);
        directionOfShip.y = 0.5f;

        sc.PushBack(directionOfShip * 60);
    }
}
