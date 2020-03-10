using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PercentageBar : MonoBehaviour
{
    

    public List<GameObject> percentageColours = new List<GameObject>();
    public List<GameObject> playerArray = new List<GameObject>();

    public ApprovalChangeUI thisPlayer;

    public Text statusText;

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

    string GetStatusText(float percentage, int numberOfPlayers)
    {
        // for example, 0.25 or above means they are the top player in a 4 player game
        float best = (float)1 / numberOfPlayers;

        // 0.5 is halfway to the best player for example
        float percentageOfBest = percentage / best;

        string result = "LOSER";

        if (percentageOfBest >= 0.50f) result = "UP N COMING";
        if (percentageOfBest >= 0.70f) result = "NECK N NECK";
        if (percentageOfBest >= 1f) result = "FAVOURITE";

        return result;
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

        statusText.text = GetStatusText(thisPlayer.approval / 100, percentageColours.Count);
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePercentages();
    }
}
