using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdManager : MonoBehaviour
{
    int playerCount;
    int[] playerScores;
    int[] numOfSupporters;
    
    ScoreManager sm;
    GameObject[] crowdMembers;

    // Start is called before the first frame update
    void Start()
    {
        sm = ScoreManager.Instance;
        playerCount = sm.players.Count;
        playerScores = new int[playerCount];
        numOfSupporters = new int[playerCount];
        crowdMembers = GameObject.FindGameObjectsWithTag("Crowd");
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < playerCount; i++)
        {
            playerScores[i] = sm.players[i].approval.percentage;
            numOfSupporters[i] = crowdMembers.Length * (playerScores[i] / 100);
            Debug.Log(numOfSupporters[i]);
        }

        
    }
}
