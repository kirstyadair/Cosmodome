using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperBall : MonoBehaviour
{
    Rigidbody rb;

    [Header("The force the bumper ball should be shot out with")]
    public float spawnForce;

    public void Start()
    {
       
    }

    public void Spawn(Vector3 position, Vector3 direction)
    {
        this.transform.position = position;
        this.transform.forward = direction;
        rb = GetComponent<Rigidbody>();
        rb.AddForce(this.transform.forward * spawnForce, ForceMode.VelocityChange);
    }
}
