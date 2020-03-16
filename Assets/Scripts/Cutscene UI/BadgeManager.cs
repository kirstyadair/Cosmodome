using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BadgeData {
    public string title;
    [TextArea]
    public string body;
}

public class BadgeManager : MonoBehaviour
{
    [Header("Potential badges name and bodies")]
    [Space(30)]

    [SerializeField]
    BadgeData badgeBully;

    [SerializeField]
    BadgeData badgeVictim;

    [SerializeField]
    BadgeData badgeJericho;

    [SerializeField]
    BadgeData badgeFirstBlood;

    [SerializeField]
    BadgeData badgeKnockedIntoWall;

    [SerializeField]
    BadgeData badgeFirstEliminated;

    [SerializeField]
    BadgeData badgeTonneInChamber;

    [SerializeField]
    BadgeData badgeNoneInChamber;

    [SerializeField]
    BadgeData badgeMostMovement;
}
