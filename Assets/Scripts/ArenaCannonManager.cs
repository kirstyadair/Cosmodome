using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaCannonManager : MonoBehaviour
{
    public ArenaCannonScript[] arenaCannons;

    [Header("A random point on this path will be picked to aim at")]
    public CinemachineSmoothPath aimPath;

    [Header("Should open an arena cannon every X seconds")]
    public float openEverySeconds;

    [Header("Chance of opening multiple arena cannons at the same time")]
    public float chanceOfOpeningMultiple;
    float sinceLastOpened = 0;

    [Header("How long a cannon stays open for")]
    public float stayOpenFor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // find a random point on the aim track
    public Vector3 PickRandomPoint()
    {
        return aimPath.EvaluatePosition(Random.Range(aimPath.MinPos, aimPath.MaxPos));
    }

    public void Open()
    {
        if (Random.Range(0f, 1f) <= chanceOfOpeningMultiple) Open();

        List<ArenaCannonScript> closedCannons = new List<ArenaCannonScript>();
        foreach (ArenaCannonScript arenaCannon in arenaCannons)
        {
            if (!arenaCannon.isOpen) closedCannons.Add(arenaCannon);
        }

        if (closedCannons.Count == 0) return;

        ArenaCannonScript chosenCannon = closedCannons[Random.Range(0, closedCannons.Count)];

        chosenCannon.OpenFor(stayOpenFor,  PickRandomPoint() - chosenCannon.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        sinceLastOpened += Time.deltaTime;

        if (sinceLastOpened >= openEverySeconds)
        {
            sinceLastOpened = 0;
            Open();
        }
    }
}
