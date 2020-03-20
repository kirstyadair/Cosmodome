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

    Animator _animator;

    [Header("How far above the ground the ring floats")]
    public float ringsHeight;

    public void SetColor(Color color)
    {
        color.a = ring.color.a;
        ring.color = color;

        aimingRingFiller.color = color;
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


        aimingRingFiller.fillAmount = ammoFillAmount;
        ring.fillAmount = percentageFillAmount;

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
