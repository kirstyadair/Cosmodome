using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PercentageBar : MonoBehaviour
{
    public LayoutElement RedPercent;
    public LayoutElement BluePercent;
    public LayoutElement OrangePercent;
    public LayoutElement PurplePercent;

    public GameObject RedPlayer;
    public GameObject BluePlayer;
    public GameObject OrangePlayer;
    public GameObject PurplePlayer;



    // Start is called before the first frame update
    void Start()
    {
        RedPercent.flexibleWidth = RedPlayer.GetComponent<ApprovalChangeUI>().approval / 100;
        BluePercent.flexibleWidth = BluePlayer.GetComponent<ApprovalChangeUI>().approval / 100;
        OrangePercent.flexibleWidth = OrangePlayer.GetComponent<ApprovalChangeUI>().approval / 100;
        PurplePercent.flexibleWidth = PurplePlayer.GetComponent<ApprovalChangeUI>().approval / 100;


    }

    public void UpdatePercentages()
    {
        RedPercent.flexibleWidth = RedPlayer.GetComponent<ApprovalChangeUI>().approval / 100;
        BluePercent.flexibleWidth = BluePlayer.GetComponent<ApprovalChangeUI>().approval / 100;
        OrangePercent.flexibleWidth = OrangePlayer.GetComponent<ApprovalChangeUI>().approval / 100;
        PurplePercent.flexibleWidth = PurplePlayer.GetComponent<ApprovalChangeUI>().approval / 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
