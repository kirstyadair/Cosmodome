using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExcitementMeterScript : MonoBehaviour
{
    public int comboScore;

    // Start is called before the first frame update
    void OnEnable()
    {
        PlayerScript.OnPlayerCollision += UpdateCombo;
    }

    void UpdateCombo(GameObject playerHit, GameObject playerAttacking)
    {
        if (playerHit == this.gameObject)
        {
            comboScore = 0;
        }
        else if (playerAttacking == this.gameObject)
        {
            comboScore++;
        }
    }

}
