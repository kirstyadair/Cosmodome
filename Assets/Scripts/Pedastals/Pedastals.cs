using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedastals : MonoBehaviour
{
    [SerializeField]
    GameObject _pedastalPrefab;

    List<PedastalScript> _pedastals = new List<PedastalScript>();

    public float gapBetweenPedastals;

    ScoreManager _sm;

    /// <summary>
    /// Sets up the pedatals, ready to be shown
    /// </summary>
    public void Setup()
    {
        _sm = ScoreManager.Instance;
        List<PlayerData> playerData = _sm.GetFinalPlayerData();
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
            pedaScript.Setup(data.playerName, data.playerColor, data.placed, data.approvalPercentage);

            _pedastals.Add(pedaScript);
        }
    }

    void Update()
    {
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
