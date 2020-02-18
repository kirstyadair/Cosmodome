using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdManager : MonoBehaviour
{
    public int playerCount;
    //public int emissionStrength;
    float[] playerScores;
    public float[] numOfSupporters;
    //public List<Material> colors;
    
    
    ScoreManager sm;
    public GameObject[] crowdMembers;


    void OnEnable()
    {
        ScoreManager.OnUpdateScore += UpdateCrowdSupport;
    }

    // Start is called before the first frame update
    public void SetUpCrowd()
    {
        sm = ScoreManager.Instance;
        playerCount = sm.players.Count;
        playerScores = new float[playerCount];
        numOfSupporters = new float[playerCount];
        crowdMembers = GameObject.FindGameObjectsWithTag("Crowd");
        
        for (int i = 0; i < playerCount; i++)
        {
            playerScores[i] = sm.players[i].approval.percentage;
            numOfSupporters[i] = crowdMembers.Length * (playerScores[i] / 100);
        }
        UpdateCrowdSupport();
        
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
                        cms[k].mat.SetColor("_EmissionColor", sm.players[i].playerColor.color);
                        cms[k].bothMats[1] = cms[k].mat;
                        cms[k].mr.materials = cms[k].bothMats;
                        cms[k].gmr1.material.SetColor("_EmissionColor", sm.players[i].playerColor.color);
                        cms[k].gmr2.material.SetColor("_EmissionColor", sm.players[i].playerColor.color);
                        cms[k].gmr3.material.SetColor("_EmissionColor", sm.players[i].playerColor.color);
                    }
                }
            }
            minVal += numOfSupporters[i];
        }
    }

    public void RecalculateCrowd()
    {
        playerCount = sm.players.Count;
        playerScores = new float[playerCount];
        numOfSupporters = new float[playerCount];
    }
}
