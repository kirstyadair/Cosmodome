﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PilotStand : MonoBehaviour
{
    [SerializeField]
    Transform walkToAndStandPoint;

    [SerializeField]
    Transform seatedPoint;

    [SerializeField]
    Transform characterPoint;
    
    [SerializeField]
    Light chairLight;

    [SerializeField]
    Light characterLight;

    Animator characterAnimator;

    Coroutine _walkAndWaveCoroutine = null;

    // Start is called before the first frame update
    void Awake()
    {
        characterAnimator = characterPoint.GetComponentInChildren<Animator>();
         if (!ShouldTest()) Disable();

        
    }

    void Start() {
        if (ShouldTest()) {
            SitDown();
            Eliminated();//WalkAndWave(2f);
        }
    }

    /// <summary>
    /// If we're in the PilotStandsDev scene, then we should automatically walk to test animations
    /// </summary>
    /// <returns></returns>
    bool ShouldTest() {
        return SceneManager.GetActiveScene().name == "PilotStandsDev";
    }

    public void Enable() {
        characterLight.enabled = true;
        chairLight.enabled = true;
        characterPoint.gameObject.SetActive(true);
    }

    public void Disable() {
        characterLight.enabled = false;
        chairLight.enabled = false;
        characterPoint.gameObject.SetActive(false);
    }

    /// <summary>
    /// Start the character walking to walkToAndStandPoint and wave at the end
    /// </summary>
    /// <param name="timeToWalk">Time it takes to walk</param>
    public void WalkAndWave(float timeToWalk) {
        characterAnimator = characterPoint.GetComponentInChildren<Animator>();

        if (characterAnimator != null) characterAnimator.Play("Walking");

        _walkAndWaveCoroutine = StartCoroutine(CharacterWalkAndWave(timeToWalk));
    }

    IEnumerator CharacterWalkAndWave(float time) {
        Vector3 start = characterPoint.transform.position;
        Vector3 end = walkToAndStandPoint.position;

        // point the character towards the end point
        characterPoint.transform.forward = end - start;

        float t = 0;

        while (t < time) {
            characterPoint.transform.position = Vector3.Lerp(start, end, t / time);
            t += Time.deltaTime;
            yield return null;
        }

        characterPoint.transform.position = end;

        if (characterAnimator != null) characterAnimator.SetBool("finishedWalking", true);
    }

    public void SitDown() {
        if (_walkAndWaveCoroutine != null) StopCoroutine(_walkAndWaveCoroutine);
        
        characterPoint.transform.position = seatedPoint.position;
        characterPoint.forward = seatedPoint.forward;
        if (characterAnimator != null) characterAnimator.Play("Idle seated");
    }

    public void Eliminated() {
        if (characterAnimator != null) characterAnimator.Play("Eliminated");
    }
}
