using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionDotter : MonoBehaviour
{
    [SerializeField]
    GameObject dotter1;

    [SerializeField]
    GameObject dotter2;

    [SerializeField]
    GameObject dotter3;

    
    /// <summary>
    /// Set the amount of highlighted dots
    /// </summary>
    /// <param name="dots">0 - 3, amount of highlighted dots</param>
    public void SetDots(int dots)
    {
        dotter1.SetActive(false);
        dotter2.SetActive(false);
        dotter3.SetActive(false);

        if (dots > 0) dotter1.SetActive(true);
        if (dots > 1) dotter2.SetActive(true);
        if (dots > 2) dotter3.SetActive(true);
    }
}
