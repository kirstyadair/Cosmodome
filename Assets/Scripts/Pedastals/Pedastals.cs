using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedastals : MonoBehaviour
{
    [SerializeField]
    GameObject _pedastalPrefab;

    List<PedastalScript> _pedastals = new List<PedastalScript>();

    public float gapBetweenPedastals;

    PedastalScript _winningPedastal;

    /// <summary>
    /// Sets up the pedatals, ready to be shown
    /// </summary>
    public void Setup(List<PlayerData> playerData)
    {
        int amountOfPedastals = playerData.Count;
        // negative offset to center
        float offset = -((gapBetweenPedastals * amountOfPedastals) / 2);

        for (int i = 0; i < amountOfPedastals; i++)
        {
            GameObject pedastal = Instantiate(_pedastalPrefab, this.transform);

            float xPosition = offset + (gapBetweenPedastals * i);

            pedastal.transform.localPosition = new Vector3(xPosition, 0, 0);

            PedastalScript pedaScript = pedastal.GetComponent<PedastalScript>();

            PlayerData data = playerData[i];
            pedaScript.Setup(data.playerNumber, data.playerColor, data.placed, data.approvalPercentage, data.characterName);
            
            if (data.placed == 1) _winningPedastal = pedaScript;
            _pedastals.Add(pedaScript);
        }
    }

    
    /// <summary>
    /// Gets the general position of the winning player, used by CutscenesManager to zoom the camera in on it
    /// </summary>
    /// <returns>Winning player pedastal position</returns>
    public Vector3 GetWinnerPosition() {
        return _winningPedastal.characterModel.transform.Find("FaceLocation").position;
    }

    /// <summary>
    /// Shows all the pedastals
    /// </summary>
    public void Show()
    {
        foreach (PedastalScript pedastal in _pedastals)
        {
            pedastal.Show();
        }
    }
}
