using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArenaCannonScript : MonoBehaviour
{
    public bool isOpen = false;

    TrapActivatorScript activator;

    public GameObject turret;
    Animator animator;
    public GameObject spotLight;

    public float rotationAmount = 50f;
    Vector3 direction;
    public ArenaCannonMissile[] missiles;

    public delegate void TrapActivate();
    public static event TrapActivate OnTrapActivate;

    public Image activationProgressSprite;

    [Header("How long the player needs to hold X button for")]
    public float activationTime;
    float beingActivatedFor = 0;

    public float timeUntilClose;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        activator = GetComponent<TrapActivatorScript>();

        activator.OnSelected += OnSelected;
        activator.OnDeselected += OnDeselected;

        activator.OnActivationStart += OnActivationStart;
        activator.OnActivationEnd += OnActivationEnd;
    }

    public void FireAnimationTrigger()
    {
        turret.transform.Rotate(new Vector3(0, 0, 1), rotationAmount);
        direction = Quaternion.Euler(0, rotationAmount, 0) * direction;

        foreach (ArenaCannonMissile missile in missiles)
        {
            if (!missile.isFired)
            {
                missile.Fire(direction, activator.currentShipActivating);
                return;
            }
        }


    }

    public void OnSelected(PlayerScript player)
    {
        animator.SetBool("Selection", true);
    }

    public void OnDeselected(PlayerScript player)
    {
        animator.SetBool("Selection", false);
        beingActivatedFor = 0;
    }

    public void OnActivationStart(PlayerScript playerScript)
    {
        animator.SetBool("Activate", true);
    }

    public void OnActivationEnd(PlayerScript playerScript)
    {
        animator.SetBool("Activate", false);
    }


    public void Activate()
    {
        //if (!isOpen) return;
        OnTrapActivate.Invoke();
        animator.SetTrigger("Fire");
    }

    public void Update()
    {
        if (isOpen)
        {
            /*if (activator.isBeingActivated)
            {
                beingActivatedFor += Time.deltaTime;
                activationProgressSprite.fillAmount = beingActivatedFor / activationTime;

                if (beingActivatedFor >= activationTime)
                {
                    activationProgressSprite.fillAmount = 0;
                    beingActivatedFor = 0;
                    activator.isActivatable = false;
                    Activate(activator.currentShipActivating);
                }
            } else
            {*/
                //timeUntilClose -= Time.deltaTime;
                //if (timeUntilClose <= 0)
                //{
                    //Close();
               // }
            //}
        }
    }
    public void OpenFor(float time, Vector3 direction)
    {
        Open(direction);

        timeUntilClose = time;
    }

    public void Open(Vector3 direction)
    {
        direction.y = turret.transform.position.y;
 
        this.direction = direction;
        turret.transform.rotation = Quaternion.LookRotation(Vector3.up, direction);//Quaternion.LookRotation(direction, Vector3.up);
        turret.transform.Rotate(Vector3.up, -90, Space.World);
        activator.isActivatable = true;

        if (isOpen) return;

        spotLight.SetActive(true);
        foreach (ArenaCannonMissile missile in missiles)
        {
            if (missile.isFired) missile.Restore();
        }

        animator.SetBool("Open", true);
        isOpen = true;
    }

    public void Close()
    {
        if (!isOpen) return;

        activator.isActivatable = false;
        spotLight.SetActive(false);
        animator.SetBool("Open", false);
        animator.SetBool("Selection", false);
        isOpen = false;
    }

}
