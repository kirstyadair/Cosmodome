using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRing : MonoBehaviour
{
    [TextArea]
    public string Help = "Looks after the movement and aiming rings below the player";

    public Image ring;
    public GameObject aimingRingObject;
    public Image aimingRingFiller;

    bool _isCharging = false;
    bool _isFullyCharged = false;
    bool _isChargingWeapon = false;
    Animator _animator;

    [Header("How far above the ground the ring floats")]
    public float ringsHeight;

    public void SetColor(Color color)
    {
        color.a = ring.color.a;
        ring.color = color;

        aimingRingFiller.color = color;
    }


    public void StartCharging()
    {
        _isCharging = true;
        _animator.SetBool("isCharging", true);
    }

    public void StopCharging()
    {
        _isCharging = false;
        _isFullyCharged = false;
        _animator.SetBool("isCharging", false);
    }

    public void FullyCharged()
    {
        _animator.SetBool("isCharged", true);
        _isFullyCharged = true;
        
    }

    public void UpdateCharge(float chargeAmount)
    {
        if (_animator == null) return;

        if (_isFullyCharged) return;
        if (_isCharging || chargeAmount == 0)
        {
            _animator.Play("Weapon ring charge up", 0, chargeAmount);
        }
        else
        {
            _animator.Play("Weapon ring charge down", 0, 1 - chargeAmount);
        }

        aimingRingFiller.fillAmount = chargeAmount;
    }

    public void IsChargingWeapon()
    {
        if (_animator != null) _animator.SetBool("isChargingWeapon", true);
        UpdateCharge(0);
        _isChargingWeapon = true;
    }

    /// <summary>
    /// Sets the alpha of the middle aiming ring
    /// </summary>
    /// <param name="a">New alpha</param>
    void SetAlpha(float a) {
        Color color = aimingRingFiller.color;
        color.a = a;
        aimingRingFiller.color = color;
    }

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Updates the rings every frame
    /// </summary>
    /// <param name="position"></param>
    /// <param name="turretDirection"></param>
    /// <param name="ammoFillAmount"></param>
    /// <param name="percentageFillAmount"></param>
    /// <param name="isDisabled"></param>
    public void UpdateRings(Vector3 position, Vector3 turretDirection, Vector3 pointingDirection, float ammoFillAmount, float percentageFillAmount, bool isDisabled)
    {
        position.y = ringsHeight;
        this.transform.position = position;



        ring.fillAmount = percentageFillAmount;
        
        if (!_isChargingWeapon) aimingRingFiller.fillAmount = ammoFillAmount;

        if (isDisabled) {
            SetAlpha(0.5f);
        } else {
            SetAlpha(1f);
        }

        this.transform.rotation = Quaternion.LookRotation(Vector3.up, pointingDirection);
        if (turretDirection.magnitude > 0) aimingRingObject.transform.rotation = Quaternion.LookRotation(Vector3.up, turretDirection);
        else aimingRingObject.transform.rotation = this.transform.rotation;

    }
}
