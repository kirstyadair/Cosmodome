using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RopeWallScript : MonoBehaviour
{
    Vector3 desiredVelocity;
    Vector3 targetPoint;
    Rigidbody rb;
    [SerializeField]
    List<ShipToBounce> ships = new List<ShipToBounce>();

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
        else if (ships.Count > 0) PushShips();
    }

    void SeekCentrePoint()
    {
        foreach (ShipToBounce ship in ships)
        {
            ship.shipRB.mass = 0.5f;
        }

        desiredVelocity = Vector3.Normalize(targetPoint - transform.localPosition) * 0.3f;
        transform.localPosition += desiredVelocity;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ship"))
        {
            ShipToBounce newShip = new ShipToBounce();
            newShip.shipRB = collision.gameObject.GetComponent<Rigidbody>();

            if (newShip.originalMass == 0) newShip.originalMass = newShip.shipRB.mass;
            newShip.timeUntilRevertMass = 1.0f;

            ships.Add(newShip);
        }
    }

    void PushShips()
    {
        foreach (ShipToBounce ship in ships)
        {
            ship.shipRB.mass = ship.originalMass;
            ShipController sc = ship.shipRB.gameObject.GetComponent<ShipController>();
            ship.directionOfShip = Vector3.Normalize(ship.shipRB.gameObject.transform.position - transform.position);
            ship.directionOfShip.y = 0.5f;

            sc.PushBack(ship.directionOfShip * 60);
        }

        ships.Clear();
    }
}

[Serializable]
public class ShipToBounce : MonoBehaviour
{
    public Rigidbody shipRB;
    public float originalMass = 0;
    public float timeUntilRevertMass;
    public int playerNumber;
    public Vector3 directionOfShip;

    private void Update()
    {
        if (timeUntilRevertMass > 0)
        {
            timeUntilRevertMass -= Time.deltaTime;
        }
        else if (timeUntilRevertMass <= 0 && originalMass != 0) shipRB.mass = originalMass;
    }
}
