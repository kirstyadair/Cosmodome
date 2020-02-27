using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Here, "bullet" refers to a GameObject spawned at the specified bulletSpawn according to these parameters. It can represent the machine gun, lasers & cannonball
/// </summary>
public class BasicWeaponScript : MonoBehaviour
{
    [SerializeField]
    [Header("The prefab that's spawned to represent the bullet")]
    GameObject _bulletPrefab;

    [SerializeField]
    [Header("List of places for the bullets to spawn from (like turrets)")]
    GameObject[] _bulletSpawns;




    [HideInInspector]
    public float bulletsCurrentlyInClip;

    [SerializeField]
    [Header("When x seconds pass, restore 1 bullet")]
    float _bulletRegenTime;

    [SerializeField]
    [Header("How long between bullet shots")]
    float _bulletCooldown;

    /// <summary>
    /// public so <see cref="ScoreManager"/> can access to calculate damage. 
    /// TODO: Should have this stored in a data gameobject so we're not retrieving every shot
    /// </summary>
    [Header("How much dmg this bullet causes when it collides with an enemy")]
    public int damage;

    /// <summary>
    /// public so 
    /// </summary>
    [Header("How many bullets make up one clip")]
    public float clipSize;

    float _timeSinceLastBulletRenewal = 0;
    float _fireCooldown;
    Rigidbody _rb;
    ShipController _shipController;

    void Start()
    {
        bulletsCurrentlyInClip = clipSize;
        _rb = GetComponent<Rigidbody>();
        _shipController = GetComponent<ShipController>();
    }

    void FixedUpdate()
    {
        if (bulletsCurrentlyInClip < clipSize)
        {
            _timeSinceLastBulletRenewal += Time.deltaTime;
            if (_timeSinceLastBulletRenewal >= _bulletRegenTime)
            {
                _timeSinceLastBulletRenewal = 0;
                bulletsCurrentlyInClip++;
            }
        }

        if (_fireCooldown > 0) _fireCooldown -= Time.deltaTime;
    }

    public void Shoot()
    {
        if (_fireCooldown > 0) return;

        if (bulletsCurrentlyInClip <= 0)
        {
            return;
        }

        GetComponent<PlayerScript>().Vibrate(1f, 0.1f);

        foreach (GameObject spawnPoint in _bulletSpawns)
        {
            GameObject bullet = Instantiate(_bulletPrefab, spawnPoint.transform.position, Quaternion.identity);

            bullet.transform.rotation = Quaternion.Euler(0, _shipController.currentTurretAngle, 0);
            bullet.GetComponent<BulletDeleter>().shooter = this.gameObject;

            _rb.AddForce(bullet.transform.forward * -_shipController.firingForcePushback, ForceMode.Impulse);

            _fireCooldown = _bulletCooldown;

            bulletsCurrentlyInClip--;
            _timeSinceLastBulletRenewal = 0; // don't renew bullets whilst firing
        }
    }
}
