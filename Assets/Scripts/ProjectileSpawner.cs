using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] GameObject projectile;

    private void OnEnable()
    {
        ExcitementManager.OnResetHype += FireProjectile;
    }

    private void OnDisable()
    {
        ExcitementManager.OnResetHype -= FireProjectile;
    }

    void FireProjectile()
    {
        GameObject newObj = Instantiate(projectile, transform.position, transform.rotation);
    }
}
