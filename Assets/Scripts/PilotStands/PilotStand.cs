using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotStand : MonoBehaviour
{
    [SerializeField]
    Transform walkToAndStandPoint;

    [SerializeField]
    Transform seatedPoint;

    [SerializeField]
    Transform characterPoint;

    Animator characterAnimator;

    // Start is called before the first frame update
    void Start()
    {
        characterAnimator = characterPoint.GetComponentInChildren<Animator>();
    }

    /// <summary>
    /// Start the character walking to walkToAndStandPoint and wave at the end
    /// </summary>
    /// <param name="timeToWalk">Time it takes to walk</param>
    public void WalkAndWave(float timeToWalk) {
        if (characterAnimator != null) characterAnimator.Play("Walking");

        StartCoroutine(CharacterWalkAndWave(timeToWalk));
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
        characterPoint.transform.position = seatedPoint.position;
        characterPoint.forward = seatedPoint.forward;
    }

    public void Eliminated() {
        characterAnimator.Play("Eliminated");
    }
}
