using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaveWeaponScript : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject bulletSpawnA;
    public GameObject bulletSpawnB;
    public float maxAmmo;
    [HideInInspector]
    public float ammo;
    public float timeBetweenBulletRenewals;
    public float timeBetweenBullets;

    float timeSinceLastBulletRenewal = 0;
    float fireCooldown;
    Rigidbody rb;
    ShipController shipController;

    void OnEnable()
    {
        ShipController.OnDaveShooting += Shoot;
    }

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
            if (timeSinceLastBulletRenewal >= timeBetweenBulletRenewals)
            {
                timeSinceLastBulletRenewal = 0;
                ammo++;
            }
        }

        if (fireCooldown > 0) fireCooldown -= Time.deltaTime;
    }

    void Shoot()
    {
        if (fireCooldown > 0) return;

        if (ammo <= 0)
        {
            return;
        }

        GetComponent<PlayerScript>().Vibrate(1f, 0.1f);

        GameObject bullet1 = Instantiate(bulletPrefab, bulletSpawnA.transform.position, Quaternion.identity);
        GameObject bullet2 = Instantiate(bulletPrefab, bulletSpawnB.transform.position, Quaternion.identity);

        bullet1.transform.rotation = Quaternion.Euler(0, shipController.currentTurretAngle, 0);
        bullet1.GetComponent<BulletDeleter>().shooter = this.gameObject;

        bullet2.transform.rotation = Quaternion.Euler(0, shipController.currentTurretAngle, 0);
        bullet2.GetComponent<BulletDeleter>().shooter = this.gameObject;
       
        rb.AddForce(bullet1.transform.forward * -shipController.firingForcePushback, ForceMode.Impulse);
        rb.AddForce(bullet2.transform.forward * -shipController.firingForcePushback, ForceMode.Impulse);

        fireCooldown = timeBetweenBullets;

        ammo--;
        timeSinceLastBulletRenewal = 0; // don't renew bullets whilst firing
    }
}
