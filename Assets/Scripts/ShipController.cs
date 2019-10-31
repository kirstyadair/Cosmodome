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
    public Vector3 prevVelocity;
    public Vector3 targetDirection;

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

    public GameObject[] hitParticleFX;
    public GameObject randomTextFX;
    PostProcessProfile postProcessProfile;

    public void Start()
    {
        postProcessProfile = Camera.main.GetComponent<PostProcessVolume>().profile;
    }

    public IEnumerator StopStrafe(float after)
    {
        yield return new WaitForSeconds(after);

        float relativeXVelocity = transform.InverseTransformVector(rb.velocity).x;

        rb.AddRelativeForce(new Vector3(-relativeXVelocity/2, 0, 0), ForceMode.VelocityChange);
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

        Debug.Log("Careening for " + time);
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

        Debug.Log("Careened");
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

        rb.AddForce(targetDirection * boostForce * control);

        float angle = Vector3.Angle(transform.forward, targetDirection);

        if (angle > 0.001)
        {
            var axis = Vector3.Cross(transform.forward, targetDirection);
            axis.z = 0;
            rb.AddTorque(axis * angle * selfRightingTorque * control);

            float moveBoostersBy = axis.y * maxBoosterDeviance;
            leftBooster.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, moveBoostersBy));
            rightBooster.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, moveBoostersBy));
        } else
        {
            leftBooster.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            rightBooster.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
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
        Debug.Log("BOOM!");

        StartCoroutine(HitPause(collision.rigidbody, Mathf.Min(hitPause, (collision.impulse.magnitude / 15) * hitPause), -collision.impulse * impactMultiplier));
        StartCoroutine(collision.gameObject.GetComponent<ShipController>().Careen(disabledTime, careenTime));
        StartCoroutine(CameraFX(collision.impulse.magnitude * chromaticAbberation, collision.impulse.magnitude * chromaticAbberationTime));

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

    // Update is called once per frame
    void Update()
    {
        if (inControl)
        {
            targetDirection = new Vector3(InputManager.ActiveDevice.LeftStick.Value.x, 0, InputManager.ActiveDevice.LeftStick.Value.y);
            targetDirection.Normalize();
            targetDirection *= InputManager.ActiveDevice.LeftStick.Value.magnitude;

            var emissionLeft = psLeft.emission;
            var emissionRight = psRight.emission;

            int emissionAmount = (int)Mathf.Max(psEmitIdle, psEmit * InputManager.ActiveDevice.LeftStick.Value.magnitude);
            emissionLeft.rateOverTime = emissionRight.rateOverTime = emissionAmount;

            Color finalColor = boosterColour * Mathf.LinearToGammaSpace(Mathf.Max(lightIntensityIdle, lightIntensity * InputManager.ActiveDevice.LeftStick.Value.magnitude));

            boosterLightPlane.material.SetColor("_EmissionColor", finalColor);

            if (strafeCooldown <= 0)
            {
                if (InputManager.ActiveDevice.LeftTrigger.WasPressed)
                {
                    rb.AddRelativeForce(Vector3.left * strafeForce, ForceMode.Impulse);
                    StartCoroutine(StopStrafe(stopStrafeAfter));
                    strafeCooldown = strafeDebounce;
                }

                if (InputManager.ActiveDevice.RightTrigger.WasPressed)
                {
                    rb.AddRelativeForce(Vector3.right * strafeForce, ForceMode.Impulse);
                    StartCoroutine(StopStrafe(stopStrafeAfter));
                    strafeCooldown = strafeDebounce;
                }
            } else
            {
                strafeCooldown -= Time.deltaTime;
            }
        } else
        {
            targetDirection = GameObject.Find("Center").transform.position - this.transform.position;
            if (targetDirection.magnitude < 1f) targetDirection = Vector3.zero;
            else targetDirection.Normalize();
        }
    }
}
