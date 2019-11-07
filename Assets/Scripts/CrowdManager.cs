using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdManager : MonoBehaviour
{
    int playerCount;
    float[] playerScores;
    float[] numOfSupporters;
    
    ScoreManager sm;
    GameObject[] crowdMembers;

    // Start is called before the first frame update
    void Start()
    {
        sm = ScoreManager.Instance;
        playerCount = sm.players.Count;
        playerScores = new float[playerCount];
        numOfSupporters = new float[playerCount];
        crowdMembers = GameObject.FindGameObjectsWithTag("Crowd");
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < playerCount; i++)
        {
            playerScores[i] = sm.players[i].approval.percentage;
            numOfSupporters[i] = crowdMembers.Length * (playerScores[i] / 100);
        }
    }

    void UpdateCrowdSupport()
    {
        int minVal = 0;
        // Array positions 0 - numOfSupporters[0] go red
        for (int i = 0; i < playerCount; i++)
        {
            for (int j = 0; j < crowdMembers.Length; j++)
            {
                if(j >= minVal && j < numOfSupporters[i])
                {
                    Debug.Log();
                }
            }
        }
        // Array positions numOfSupporters[0]+1 to numOfSupporters[1] go blue

        // Array positions numOfSupporters[1]+1 to numOfSupporters[2] go green

        // Array positions numOfSupporters[2]+1 to crowdMembers.length go yellow
    }
}
