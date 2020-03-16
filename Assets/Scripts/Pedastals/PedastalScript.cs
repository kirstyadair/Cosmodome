using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PedastalScript : MonoBehaviour
{
    Animator _animator;

    [SerializeField]
    Text _playerNameText;

    [SerializeField]
    Text _placedText;


    [SerializeField]
    Light _spotlight1;

    [SerializeField]
    Light _spotlight2;

    [SerializeField]
    Transform _characterModelsContainer;

    int _placed;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();    
    }

    /// <summary>
    /// Sets up the pedastal with the correct detail, model and colour.
    /// </summary>
    /// <param name="playerName">Player name to display in Player N badge</param>
    /// <param name="playerColour">Player colour for lights</param>
    /// <param name="placed">Where they placed, 1 for 1st, 2 for 2nd etc</param>
    /// <param name="approvalPercentage">Approval percentage at time of death or game over</param>
    public void Setup(int playerNumber, Color playerColour, int placed, int approvalPercentage, string characterName)
    {
        _placed = placed;
        _spotlight1.color = playerColour;
        _spotlight2.color = playerColour;

        _playerNameText.text = "PLAYER " + playerNumber;
        _playerNameText.color = playerColour;

        string place = "";
        if (placed == 1) place = "1st";
        if (placed == 2) place = "2nd";
        if (placed == 3) place = "3rd";
        if (placed == 4) place = "4th";

        _placedText.text = place;

        EnableCharacterModel(characterName);
    }

    void EnableCharacterModel(string characterName) {
        foreach (Transform transform in _characterModelsContainer) {
            if (transform.gameObject.name == characterName) transform.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Pops this pedastal up from the ground
    /// </summary>
    public void Show()
    {
        _animator.Play(_placed.ToString());
    }
}
