using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaCannonScript : MonoBehaviour
{
    public bool isOpen = false;
    public GameObject turret;
    Coroutine closeAfterTime;
    Animator animator;

    Vector3 direction;
    public ArenaCannonMissile[] missiles;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();    
    }

    public void FireAnimationTrigger()
    {
        foreach (ArenaCannonMissile missile in missiles)
        {
            if (!missile.isFired)
            {
                missile.Fire(direction);
                return;
            }
        }
    }

    public void Activate()
    {
        if (!isOpen) return;

        animator.SetTrigger("Fire");
        if (closeAfterTime != null) StopCoroutine(closeAfterTime);
        closeAfterTime = StartCoroutine(CloseAfterTime(0.5f));

    }

    public void OpenFor(float time, Vector3 direction)
    {
        Open(direction);

        if (closeAfterTime != null) StopCoroutine(closeAfterTime);
        closeAfterTime = StartCoroutine(CloseAfterTime(time));
    }

    public void Open(Vector3 direction)
    {
        //direction.y = 0

        this.direction = direction;
        turret.transform.right = -direction;
        turret.transform.Rotate(new Vector3(-90, 0, 0), Space.Self);
        //turret.transform.forward = Vector3.up;

        if (isOpen) return;

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

        animator.SetBool("Open", false);
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
