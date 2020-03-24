using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsUIScript : MonoBehaviour
{
    Animator animator;

    public float delayBeforeShowingControls;
    public float timeBeforeHidingControls;

    public void ShowControls()
    {
        animator.SetTrigger("Show"); 
    }

    public void HideControls()
    {
        animator.SetTrigger("Hide");
    }

    public IEnumerator ShowControlsAfterE(float time)
    {
        yield return new WaitForSeconds(time);
        ShowControls();
        StartCoroutine(HideControlsAfter(timeBeforeHidingControls));
    }

    public IEnumerator HideControlsAfter(float time)
    {
        yield return new WaitForSeconds(time);
        HideControls();
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
      
    }

    public void ShowControlsAfter(float time) {
          StartCoroutine(ShowControlsAfterE(delayBeforeShowingControls));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
