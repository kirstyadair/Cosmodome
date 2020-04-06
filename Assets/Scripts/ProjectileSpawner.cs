using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] GameObject projectile;

    private void OnEnable()
    {
        CrowdManager.OnProjectileThrow += FireProjectile;
    }

    private void OnDisable()
    {
        CrowdManager.OnProjectileThrow -= FireProjectile;
    }

    void FireProjectile()
    {
        GameObject newObj = Instantiate(projectile, transform.position, transform.rotation);
    }
}
