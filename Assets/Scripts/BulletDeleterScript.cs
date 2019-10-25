using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDeleterScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "RedBullet" || other.tag == "GreenBullet")
        {
            Destroy(other.gameObject);
        }
    }
}
