using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapActivatorScript : MonoBehaviour
{
    public float activationRadius;
    public Transform activationRadiusCenter;
    public bool isActivatable = false;
    public bool isSelected = false;
    public bool isBeingActivated = false;
    public PlayerScript currentShipActivating = null;
    public delegate void TrapActivationEvent(PlayerScript player);
    public float drawPlayerTowardsForce;
    public event TrapActivationEvent OnSelected;
    public event TrapActivationEvent OnDeselected;
    public event TrapActivationEvent OnActivationStart;
    public event TrapActivationEvent OnActivationEnd;

    public void Update()
    {
        if (isActivatable)
        {
            bool prevIsSelected = isSelected;
            isSelected = false;
            
            if (!isBeingActivated)
            {
                foreach (Collider collider in Physics.OverlapSphere(activationRadiusCenter.position, activationRadius))
                {
                    if (collider.CompareTag("Ship"))
                    {
                        isSelected = true;
                        if (collider.GetComponent<PlayerScript>().isActivatingTrap)
                        {
                            isBeingActivated = true;
                            currentShipActivating = collider.GetComponent<PlayerScript>();
                            OnActivationStart?.Invoke(currentShipActivating);
                        }
                    }
                }
            } else
            {
                // cover for the case that the currently activating ship is destroyed
                if (currentShipActivating == null)
                {
                    isBeingActivated = false;
                    OnActivationEnd?.Invoke(null);
                }

                // keep track of how far this boi is
                if (!currentShipActivating.isActivatingTrap || (currentShipActivating.transform.position - this.transform.position).magnitude > activationRadius)
                {
                    isBeingActivated = false;
                    OnActivationEnd?.Invoke(currentShipActivating);
                    currentShipActivating = null;
                } else
                {
                    currentShipActivating.gameObject.GetComponent<Rigidbody>().AddForce((activationRadiusCenter.position - currentShipActivating.transform.position) * drawPlayerTowardsForce);
                }
            }



            if (!isSelected && prevIsSelected) OnDeselected?.Invoke(null);
            if (isSelected && !prevIsSelected) OnSelected?.Invoke(null);
        } else
        {
            if (isSelected)
            {
                isSelected = false;
                OnDeselected?.Invoke(null);
            }

            if (isBeingActivated)
            {
                isBeingActivated = false;
                OnActivationEnd?.Invoke(currentShipActivating);
                //currentShipActivating = null;
            }
        }
    }
}
