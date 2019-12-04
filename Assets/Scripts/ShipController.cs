using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using UnityEngine.Rendering.PostProcessing;

public class ShipController : MonoBehaviour
{
    public Rigidbody rb;
    public bool aiControl = false;
    public float boostForce;
    public float hoverDistance = 3f;
    public float hoverForce = 1f;
    public float selfRightingTorque = 1f;
    public float turningTorque = 1f;
    public ParticleSystem psLeft;
    public ParticleSystem psRight;
    public MeshRenderer boosterLightPlane;
    public float psEmit = 500f;
    public float psEmitIdle = 5f;
    public Color boosterColour;
    public float lightIntensity = 50f;
    public float lightIntensityIdle = 10f;
    public bool inControl;

    public Transform leftBooster;
    public Transform rightBooster;
    public float maxBoosterDeviance = 15;
    public Vector3 boosterRotateAxis;
    public Vector3 prevVelocity;
    public Vector3 targetDirection;
    public Vector3 turretDirection;
    public Vector3 turretRotateAxis;
    public Vector3 turretRotateOffset;
    public float targetTurretAngle = 0;
    public float currentTurretAngle = 0;
    public float turretLerpSpeed = 0.5f;
    public float thresholdBeforeFiringTurret = 0.2f;

    public float impactThreshold = 1f;
    public float hitPause = 0.01f;
    public float impactMultiplier = 1f;

    public float disabledTime = 1f;
    public float careenTime = 1f;

    public float chromaticAbberation = 1f;
    public float chromaticAbberationTime = 1f;

    public float strafeForce;
    public float strafeCooldown = 0f;
    public float stopStrafeAfter = 1f;
    public float strafeDebounce = 1f;
    public float control = 1f;

    public float vibrationMultiplier = 1f;
    public float vibrationLengthMultiplier = 1f;

    public GameObject[] turretObjects;
    public GameObject bulletSpawnA;
    public GameObject bulletSpawnB;
    public GameObject bulletPrefab;
    public bool firedA = true;
    public float hitByBulletForce = 1f;
    public float firingForcePushback = 1f;
    float fireCooldown;
    float burstCooldown;
    float amountOfBursts = 0;
    public float burstCount = 5;
    public float timeBetweenBursts;
    public float timeBetweenBullets;

    public GameObject[] hitParticleFX;
    public GameObject randomTextFX;
    PostProcessProfile postProcessProfile;
    PlayerScript playerScript;

    public delegate void PlayerShooting();
    public static event PlayerShooting OnPlayerShooting;
    public delegate void PlayerReload();
    public static event PlayerReload OnPlayerReload;

    public void Start()
    {
        playerScript = GetComponent<PlayerScript>();
        postProcessProfile = Camera.main.GetComponent<PostProcessVolume>().profile;
    }

    public IEnumerator StopStrafe(float after)
    {
        yield return new WaitForSeconds(after);

        float relativeXVelocity = transform.InverseTransformVector(rb.velocity).x;

        rb.AddRelativeForce(new Vector3(-relativeXVelocity/2, 0, 0), ForceMode.VelocityChange);
    }

    public void AimTurret()
    {
        targetTurretAngle = Mathf.Atan2(turretDirection.x, turretDirection.z) * Mathf.Rad2Deg;
        if (turretDirection.magnitude < thresholdBeforeFiringTurret) targetTurretAngle = Mathf.Atan2(transform.forward.x, transform.forward.z) * Mathf.Rad2Deg;


        currentTurretAngle = Mathf.LerpAngle(currentTurretAngle, targetTurretAngle, turretLerpSpeed);

        foreach (GameObject turretObject in turretObjects) turretObject.transform.rotation = Quaternion.Euler((turretRotateAxis * currentTurretAngle) + turretRotateOffset);
    }


    public void HitByBullet(BulletDeleter bullet)
    {
        foreach (GameObject particleFx in hitParticleFX)
        {
            Instantiate(particleFx, bullet.transform.position, Quaternion.identity);
        }

        rb.AddForce(bullet.transform.forward * hitByBulletForce, ForceMode.Impulse);

        playerScript.WasHitByBullet(bullet);
    }

    public void Fire()
    {
        
        if (fireCooldown > 0 || burstCooldown > 0) return;
        OnPlayerShooting.Invoke();
        
        Vector3 spawnPosition;
        if (firedA)
        {
            firedA = false;
            spawnPosition = bulletSpawnA.transform.position;
        } else
        {
            firedA = true;
            spawnPosition = bulletSpawnB.transform.position;
        }

        GetComponent<PlayerScript>().Vibrate(1f, 0.1f);
        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
        bullet.transform.rotation = Quaternion.Euler(0, currentTurretAngle, 0);
        bullet.GetComponent<BulletDeleter>().shooter = this.gameObject;

        fireCooldown = timeBetweenBullets;

        rb.AddForce(bullet.transform.forward * -firingForcePushback, ForceMode.Impulse);

        amountOfBursts++;
        if (amountOfBursts >= burstCount)
        {
            GetComponent<PlayerScript>().DisableTurretRing();
            burstCooldown = timeBetweenBursts;
            amountOfBursts = 0;
        }
    }

    public void HoverAndSelfRight()
    {
        float deviance = Mathf.Sin(Time.time * 3) / 7f;

        float amountToHoverBy = hoverDistance - this.transform.position.y + deviance;

        rb.AddForce(Vector3.up * (amountToHoverBy * hoverForce));


        var angle = Vector3.Angle(transform.up, Vector3.up);
        if (angle > 0.001)
        {
            var axis = Vector3.Cross(transform.up, Vector3.up);
            rb.AddTorque(axis * angle * turningTorque * control);
        }
    }

    public IEnumerator Careen(float disabledTime, float time)
    {
        if (control < 1f) yield break;

        float prevAngularDrag = rb.angularDrag;
        rb.angularDrag = 0;
        control = 0;

        yield return new WaitForSeconds(disabledTime);
        float t = 0;    


        while (t < time)
        {
            rb.angularDrag = Mathf.Lerp(0f, prevAngularDrag, t / time);
            control = Mathf.Lerp(0f, 1f, t / time);
            t += Time.deltaTime;

            yield return null;
        }

        rb.angularDrag = prevAngularDrag;
        control = 1f;
    }

    public IEnumerator CameraFX(float chromaticAbberation, float time)
    {
        chromaticAbberation = Mathf.Min(1f, chromaticAbberation);
        postProcessProfile.GetSetting<ChromaticAberration>().intensity.Override(chromaticAbberation);
        float t = 0;

        while (t < time)
        {
            postProcessProfile.GetSetting<ChromaticAberration>().intensity.Override(Mathf.Lerp(chromaticAbberation, 0, t / time));
            t += Time.deltaTime;

            yield return null;
        }

        postProcessProfile.GetSetting<ChromaticAberration>().intensity.Override(0);
    }


    public void FixedUpdate()
    {
        HoverAndSelfRight();
        AimTurret();

       
        var emissionLeft = psLeft.emission;
        var emissionRight = psRight.emission;

        int emissionAmount = (int)Mathf.Max(psEmitIdle, psEmit * (targetDirection.magnitude));
        Color finalColor = boosterColour * Mathf.LinearToGammaSpace(Mathf.Max(lightIntensityIdle, lightIntensity * emissionAmount));

        boosterLightPlane.material.SetColor("_EmissionColor", finalColor);

        emissionLeft.rateOverTime = emissionRight.rateOverTime = emissionAmount;
        rb.AddForce(targetDirection * boostForce * control);

        float angle = Vector3.Angle(transform.forward, targetDirection);

        if (angle > 0.001)
        {
            var axis = Vector3.Cross(transform.forward, targetDirection);
            axis.z = 0;
            rb.AddTorque(axis * angle * selfRightingTorque * control);

            float moveBoostersBy = axis.y * maxBoosterDeviance;
            leftBooster.transform.localRotation = Quaternion.Euler(boosterRotateAxis * moveBoostersBy);
            rightBooster.transform.localRotation = Quaternion.Euler(boosterRotateAxis * moveBoostersBy);
        } else
        {
            leftBooster.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            rightBooster.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }

        if (fireCooldown > 0) fireCooldown -= Time.deltaTime;
        if (strafeCooldown > 0) strafeCooldown -= Time.deltaTime;
        if (burstCooldown > 0)
        {
            burstCooldown -= Time.deltaTime;
            if (burstCooldown <= 0) 
            {
                GetComponent<PlayerScript>().EnableTurretRing();
                OnPlayerReload.Invoke();
            }
        }

        this.prevVelocity = rb.velocity;
    }

    public void OnCollisionEnter(Collision collision)
    {

        if (!collision.transform.CompareTag("Ship")) return;
       
        if (collision.relativeVelocity.magnitude < impactThreshold) return;

        Vector3 shipAVelocity = prevVelocity;
        Vector3 shipBVelocity = collision.gameObject.GetComponent<ShipController>().prevVelocity;

        if (shipAVelocity.magnitude < shipBVelocity.magnitude) return;

        if (control < 1 || collision.gameObject.GetComponent<ShipController>().control < 1) return;

        this.GetComponent<PlayerScript>().Vibrate((collision.impulse.magnitude * vibrationMultiplier), (collision.impulse.magnitude * vibrationLengthMultiplier));
        collision.gameObject.GetComponent<PlayerScript>().Vibrate((collision.impulse.magnitude * vibrationMultiplier), (collision.impulse.magnitude * vibrationLengthMultiplier));

        StartCoroutine(HitPause(collision.rigidbody, Mathf.Min(hitPause, (collision.impulse.magnitude / 15) * hitPause), -collision.relativeVelocity * impactMultiplier));
        StartCoroutine(collision.gameObject.GetComponent<ShipController>().Careen(disabledTime, careenTime));
        StartCoroutine(CameraFX(collision.impulse.magnitude * chromaticAbberation, collision.impulse.magnitude * chromaticAbberationTime));

        collision.transform.gameObject.GetComponent<PlayerScript>().WasCollidedWith();

        Vector3 spawnPos = collision.GetContact(0).point;
        foreach (GameObject particleFx in hitParticleFX)
        {

            Instantiate(particleFx, spawnPos, Quaternion.identity);
        }

        Instantiate(randomTextFX, spawnPos + Vector3.up, Quaternion.identity);
       
        //collision.rigidbody.AddForce(collision.impulse * impactMultiplier);
    }

    public IEnumerator HitPause(Rigidbody other, float seconds, Vector3 forceAfter)
    {
        Vector3 thisLastVel = rb.velocity;
        Vector3 thisLastAngular = rb.angularVelocity;
        rb.isKinematic = true;

        Vector3 otherVel = other.velocity;
        Vector3 otherAngular = other.angularVelocity;
        other.isKinematic = true;

        yield return new WaitForSeconds(seconds);

        rb.isKinematic = false;
        rb.velocity = thisLastVel;
        rb.angularVelocity = thisLastAngular;

        other.isKinematic = false;
        other.velocity = otherVel;
        other.angularVelocity = otherAngular;

        other.AddForce(forceAfter, ForceMode.Impulse);
    }

    public void Boost()
    {
        if (strafeCooldown <= 0)
        {
            //InputManager.ActiveDevice.LeftStickButton.WasPressed
            rb.AddForce(targetDirection * strafeForce, ForceMode.Impulse);
            strafeCooldown = strafeDebounce;
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
