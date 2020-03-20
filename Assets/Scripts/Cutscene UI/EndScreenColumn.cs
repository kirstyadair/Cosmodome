using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreenColumn : MonoBehaviour
{
    [SerializeField]
    Text playerNText;

    [SerializeField]
    Text placedTextField;

    [SerializeField]
    Text smallPlayerNText;

    [SerializeField]
    Image smallPlayerNBg;

    [SerializeField]
    GameObject badgeGameObject;

    [SerializeField]
    Text badgeTitleText;

    [SerializeField]
    Text badgeBodyText;

    [SerializeField]
    Text ramsCount;

    [SerializeField]
    Text comboCount;

    [SerializeField]
    Text characterNameField;

    Animator _animator;
    /// <summary>
    /// Displays this column with the given PlayerData and BadgeData.
    /// If badge is null, then we just won't show it.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="badge"></param>
    public void Setup(PlayerData data, BadgeData badge) {
        _animator = GetComponent<Animator>();

        playerNText.text = "PLAYER " + data.playerNumber;
        playerNText.color = data.playerColor;
        smallPlayerNText.text = "P" + data.playerNumber;
        smallPlayerNBg.color = data.playerColor;

        ramsCount.text = data.rams.ToString();
        comboCount.text = data.comboHi.ToString();


        string placedText = "ERR";
        Color placedTextColour = data.playerColor;

        switch (data.placed) {
            case 1:
                placedText = "1st!";
                break;

            case 2:
                placedText = "2nd.";
                break;
            
            case 3:
                placedText = "3rd.";
                break;
            
            case 4:
                placedText = "4th.";
                break;
        }

        placedTextField.text = placedText;
        placedTextField.color = placedTextColour;

        if (badge != null) {
            badgeGameObject.SetActive(true);
            badgeTitleText.text = badge.title;
            badgeBodyText.text = badge.body;
        }

        characterNameField.text = "as " + data.characterName;

        //_animator.Play("Drop in");
    }
}
