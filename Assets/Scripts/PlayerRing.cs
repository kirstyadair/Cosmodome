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

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void UpdateRings(Vector3 position, Vector3 turretDirection, float ammoFillAmount, float percentageFillAmount)
    {
        position.y = ringsHeight;
        this.transform.position = position;
        //ring.transform.rotation = Quaternion.LookRotation(Vector3.up, this.transform.forward);

        if (turretDirection.magnitude > 0) aimingRingObject.transform.rotation = Quaternion.LookRotation(Vector3.up, turretDirection);
        else aimingRingObject.transform.rotation = this.transform.rotation;

        ring.fillAmount = percentageFillAmount;
        
        if (!_isChargingWeapon) aimingRingFiller.fillAmount = ammoFillAmount;
    }
}
