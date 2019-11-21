﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudienceDiversifiier : MonoBehaviour
{
    public GameObject[] alienSprites;
    public Animator animator;
    public Sprite[] possibleAlienSprites;

    public Vector2 animatorSpeedRange;
    public Vector2 alienScaleRange;
    public Vector2 alienAnimateRange;

    // Start is called before the first frame update
    void Start()
    {
        Randomize();
        //FaceTowardCamera(); nah
    }

    public void FaceTowardCamera()
    {
        foreach (GameObject alienSprite in alienSprites)
        {
            alienSprite.transform.forward = -Camera.main.transform.forward;
        }
    }

    public void Randomize()
    {
        float speed = Random.Range(animatorSpeedRange.x, animatorSpeedRange.y);
        if (Random.Range(0f, 1f) > 0.5f) speed *= -1;

        animator.SetFloat("Speed", speed);

        foreach (GameObject alienSprite in alienSprites)
        {
            alienSprite.GetComponent<SpriteRenderer>().sprite = possibleAlienSprites[Random.Range(0, possibleAlienSprites.Length)];
            alienSprite.GetComponent<Animator>().speed = Random.Range(alienAnimateRange.x, alienAnimateRange.y);
            alienSprite.transform.localScale *= Random.Range(alienScaleRange.x, alienScaleRange.y);
        }
    }
}
