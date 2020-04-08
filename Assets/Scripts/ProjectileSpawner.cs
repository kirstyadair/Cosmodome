using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    ScoreManager scoreManager;

    private void Start()
    {
        scoreManager = ScoreManager.Instance;
    }

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
        if (scoreManager.gameState != GameState.INGAME) return;

        GameObject newObj = Instantiate(projectile, transform.position, transform.rotation);
    }
}
