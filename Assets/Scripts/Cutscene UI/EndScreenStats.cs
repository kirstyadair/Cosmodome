using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InControl;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenStats : MonoBehaviour
{
    [SerializeField]
    GameObject _daveColumn;

    [SerializeField]
    GameObject _hhhColumn;

    [SerializeField]
    GameObject _moscoColumn;

    [SerializeField]
    GameObject _schlugColumn;

    [SerializeField]
    GameObject _columnContainer;

    Animator _animator;

    bool _isWaitingForStart = false;
    void Update() {
        if (!_isWaitingForStart) return;
        if (InputManager.ActiveDevice.Command.WasPressed) {

            SceneManager.LoadScene("CharacterSelection");
        }
    }

    public void Setup(List<PlayerData> playerData) {
         _animator = GetComponent<Animator>();

        playerData = playerData.OrderBy((a) => a.placed).ToList();

        foreach (PlayerData data in playerData) {
            GameObject column = null;

            // very ugly hacky way of doing this but im in a rush
            switch (data.playerType) {
                case PlayerTypes.BIG_SCHLUG:
                    column = _schlugColumn;
                    break;
                case PlayerTypes.HAMMER:
                    column = _hhhColumn;
                    break;
                case PlayerTypes.EL_MOSCO:
                    column = _moscoColumn;
                    break;
                case PlayerTypes.DAVE:
                    column = _daveColumn;
                    break;


            }

            GameObject newColumn = Instantiate(column, _columnContainer.transform);
            newColumn.GetComponent<EndScreenColumn>().Setup(data, null);
        }

        _animator.Play("Drop in");

        _isWaitingForStart = true;
        DestroyImmediate(GameObject.Find("Character Selection Logic"));
    }
}
