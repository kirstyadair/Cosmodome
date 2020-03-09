using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PercentageBar : MonoBehaviour
{
    

    public List<GameObject> percentageColours = new List<GameObject>();
    public List<GameObject> playerArray = new List<GameObject>();



    public void OnStateChange(GameState newState, GameState oldState)
    {
        if (newState == GameState.ROUND_END_CUTSCENE)
        {
            CheckPlayers();
        }
        if (newState == GameState.ROUND_START_CUTSCENE)
        {
            CheckPlayers();
        }
        if (newState == GameState.COUNTDOWN)
        {
            CheckPlayers();

        }



    }


    // Start is called before the first frame update
    void Start()
    {
        ScoreManager.OnStateChanged += OnStateChange;

    }

    void CheckPlayers()
    {
        for (int i = 0; i < playerArray.Count; i++)
        {
            if (!playerArray[i].activeSelf)
            {
                percentageColours[i].SetActive(false);
            }
        }
    }

    public void UpdatePercentages()
    {
        for(int i =0; i<percentageColours.Count;i++)
        {
            percentageColours[i].GetComponent<LayoutElement>().flexibleWidth = playerArray[i].GetComponent<ApprovalChangeUI>().approval / 100;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePercentages();
    }
}
