using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicWeaponScript : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject[] bulletSpawns;
    public float maxAmmo;
    [HideInInspector]
    public float ammo;
    public float bulletRegenTime;
    public float bulletCooldown;
    public int damage;

    float timeSinceLastBulletRenewal = 0;
    float fireCooldown;
    Rigidbody rb;
    ShipController shipController;

    void Start()
    {
        ammo = maxAmmo;
        rb = GetComponent<Rigidbody>();
        shipController = GetComponent<ShipController>();
    }

    void FixedUpdate()
    {
        if (ammo < maxAmmo)
        {
            timeSinceLastBulletRenewal += Time.deltaTime;
            if (timeSinceLastBulletRenewal >= bulletRegenTime)
            {
                timeSinceLastBulletRenewal = 0;
                ammo++;
            }
        }

        if (fireCooldown > 0) fireCooldown -= Time.deltaTime;
    }

    public void Shoot()
    {
        if (fireCooldown > 0) return;

        if (ammo <= 0)
        {
            return;
        }

        GetComponent<PlayerScript>().Vibrate(1f, 0.1f);

        foreach (GameObject spawnPoint in bulletSpawns)
        {
            GameObject bullet = Instantiate(bulletPrefab, spawnPoint.transform.position, Quaternion.identity);

            bullet.transform.rotation = Quaternion.Euler(0, shipController.currentTurretAngle, 0);
            bullet.GetComponent<BulletDeleter>().shooter = this.gameObject;

            rb.AddForce(bullet.transform.forward * -shipController.firingForcePushback, ForceMode.Impulse);

            fireCooldown = bulletCooldown;

            ammo--;
            timeSinceLastBulletRenewal = 0; // don't renew bullets whilst firing
        }
    }
}
