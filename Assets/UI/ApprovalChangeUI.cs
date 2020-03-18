using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ApprovalChangeUI : MonoBehaviour
{

    public Image backingBarL;
    public Image backingBarR;
    public Text approvalText;

    public GameObject player;
    private Animator animator;

    ScoreManager sm;
  
    
    public float approval;
    private float previousApproval;

    public void OnStateChange (GameState newState, GameState oldState)
    {
        // Only show the UI as visible when gamestate is INGAME
        if (newState == GameState.COUNTDOWN) animator.SetBool("Visible", true);
        else if (newState != GameState.INGAME) animator.SetBool("Visible", false);
    }

    private void Start()
    {
        sm = ScoreManager.Instance;
        ScoreManager.OnStateChanged += OnStateChange;
        approval = player.GetComponent<PlayerScript>().approval.percentage;
        previousApproval = approval;
        approvalText.text = previousApproval.ToString() + "%"; 
        animator = GetComponent<Animator>();
    }

    void OnDestroy() {
        ScoreManager.OnStateChanged -= OnStateChange;
    }

    private void OnGUI()
    {
        
    }

    public void Update()
    {
        if (player.activeSelf == false)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
        approval = player.GetComponent<PlayerScript>().approval.percentage;
        if (approval != previousApproval)
        {
            if(approval<previousApproval)
            {
                animator.SetBool("Hit", true);
                StartCoroutine(WaitFlash());
            }
            previousApproval = approval;
            approvalText.text = previousApproval.ToString() + "%";

            
        }
        gameObject.GetComponentInChildren<PercentageBar>().UpdatePercentages();

    }




    IEnumerator WaitFlash()
    {
        yield return new WaitForSeconds(1f);
        animator.SetBool("Hit", false);
    }

}
