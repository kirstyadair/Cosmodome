using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaSpotlightFollow : MonoBehaviour
{
   // public CinemachineSmoothPath track;
    public float speed = 1f;
    public float distance = 0;
    public float invertChance = 0.01f;
    ScoreManager scoreManager;

    // Start is called before the first frame update
    void Start()
    {
        scoreManager = ScoreManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (scoreManager.winningPlayer == null) return;

        distance += speed * Time.deltaTime;
        this.transform.up = this.transform.position - scoreManager.winningPlayer.transform.position;
    }
}
