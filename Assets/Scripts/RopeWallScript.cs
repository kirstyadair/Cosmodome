using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeWallScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<Rigidbody>().useGravity = false;   
        //GetComponent<Rigidbody>().isKinematic = true;   
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
    }
}
