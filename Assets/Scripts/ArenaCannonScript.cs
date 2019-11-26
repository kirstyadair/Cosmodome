using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArenaCannonScript : MonoBehaviour
{
    public bool isOpen = false;

    TrapActivatorScript activator;

    public GameObject turret;
    Coroutine closeAfterTime;
    float closeAfterSeconds;
    Animator animator;
    public GameObject spotLight;

    Vector3 direction;
    public ArenaCannonMissile[] missiles;

    public Image activationProgressSprite;

    [Header("How long the player needs to hold X button for")]
    public float activationTime;
    float beingActivatedFor = 0;

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
    }

    public void OnActivationStart(PlayerScript playerScript)
    {
        if (closeAfterTime != null)
        {
            StopCoroutine(closeAfterTime);
            closeAfterTime = null;
        }

        animator.SetBool("Activate", true);
    }

    public void OnActivationEnd(PlayerScript playerScript)
    {
        closeAfterTime = StartCoroutine(CloseAfterTime(closeAfterSeconds));

        animator.SetBool("Activate", false);
    }


    public void Activate(PlayerScript player)
    {
        if (!isOpen) return;

        animator.SetTrigger("Fire");

        if (closeAfterTime != null)
        {
            StopCoroutine(closeAfterTime);
            closeAfterTime = null;
        }
       // closeAfterTime = StartCoroutine(CloseAfterTime(0.5f));

    }

    public void Update()
    {
        if (isOpen && activator.isBeingActivated)
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
        }
    }
    public void OpenFor(float time, Vector3 direction)
    {
        Open(direction);

        closeAfterSeconds = time;
        if (closeAfterTime != null) StopCoroutine(closeAfterTime);
        closeAfterTime = StartCoroutine(CloseAfterTime(time));
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

        if (closeAfterTime != null) StopCoroutine(closeAfterTime);
    }


    IEnumerator CloseAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        closeAfterTime = null;
        Close();
    }
}
