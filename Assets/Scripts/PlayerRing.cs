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

    [Header("How far above the ground the ring floats")]
    public float ringsHeight;

    public void SetColor(Color color)
    {
        color.a = ring.color.a;
        ring.color = color;

        aimingRingFiller.color = color;
    }

    public void UpdateRings(Vector3 position, Vector3 turretDirection, float ammoFillAmount, float percentageFillAmount)
    {
        position.y = ringsHeight;
        this.transform.position = position;
        //ring.transform.rotation = Quaternion.LookRotation(Vector3.up, this.transform.forward);

        if (turretDirection.magnitude > 0) aimingRingObject.transform.rotation = Quaternion.LookRotation(Vector3.up, turretDirection);
        else aimingRingObject.transform.rotation = this.transform.rotation;

        ring.fillAmount = percentageFillAmount;
        aimingRingFiller.fillAmount = ammoFillAmount;
    }
}
