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
        Debug.Log(sm.players.Count);
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
            Debug.Log(crowdMembers.Length);
        }
    }
}
