using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdManager : MonoBehaviour
{
    int playerCount;
    float[] playerScores;
    public float[] numOfSupporters;
    public Material[] colors;
    
    ScoreManager sm;
    public GameObject[] crowdMembers;

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
        UpdateCrowdSupport();
        
    }

    void UpdateCrowdSupport()
    {
        float minVal = 0;

        for (int i = 0; i < playerCount; i++)
        {
            for (int j = 0; j < crowdMembers.Length; j++)
            {
                if(j >= minVal && j < (minVal + numOfSupporters[i]))
                {
                    CrowdMemberScript[] cms = crowdMembers[j].GetComponentsInChildren<CrowdMemberScript>();
                    for (int k = 0; k < cms.Length; k++)
                    {
                        cms[k].newMat = colors[i];
                    }
                }
            }
            minVal += numOfSupporters[i];
        }
    }
}
