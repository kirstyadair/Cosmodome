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
    public List<CrowdMemberScript> crowdMemberScripts = new List<CrowdMemberScript>();


    void OnEnable()
    {
        ScoreManager.OnUpdateScore += UpdateCrowdSupport;
        ExcitementManager.OnResetHype += CrowdBoo;
    }

    void OnDisable() {
        ScoreManager.OnUpdateScore -= UpdateCrowdSupport;
        ExcitementManager.OnResetHype -= CrowdBoo;
    }

    // Start is called before the first frame update
    public void SetUpCrowd()
    {
        sm = ScoreManager.Instance;
        playerCount = sm.players.Count;
        playerScores = new float[playerCount];
        numOfSupporters = new float[playerCount];
        crowdMembers = GameObject.FindGameObjectsWithTag("Crowd");
        foreach (GameObject crowdMember in crowdMembers)
        {
            crowdMemberScripts.Add(crowdMember.GetComponent<CrowdMemberScript>());
        }
        
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
            for (int j = 0; j < crowdMemberScripts.Count; j++)
            {
                if (crowdMemberScripts[j] == null)
                {
                    Debug.Log(j);
                    Debug.Log(crowdMemberScripts.Count);   
                }
                if(j >= minVal && j < (minVal + numOfSupporters[i]))
                {
                    crowdMemberScripts[j].glowstickMR.material.SetColor("_EmissionColor", sm.players[i].playerColor.color * 5);
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

    void CrowdBoo()
    {
        if (Random.value < 0.5f) return;

        Debug.Log("throwing something");
    }
}
