using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestTrapController : MonoBehaviour
{
    public Text openButtonText;
    public ArenaCannonScript arenaCannon;
    public GameObject activateButton;

    public float openTime = 5f;
    public void OpenButtonClicked()
    {
        if (arenaCannon.isOpen)
        {
            arenaCannon.Close();
        } else
        {
            arenaCannon.OpenFor(openTime, new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)));
        }
    }

    public void ActivateButtonClicked()
    {
        arenaCannon.Activate(null);
    }

    public void Update()
    {
        if (arenaCannon.isOpen)
        {
            openButtonText.text = "Close";
            activateButton.SetActive(true);
        } else
        {
            openButtonText.text = "Open";
            activateButton.SetActive(false);
        }
    }
}
