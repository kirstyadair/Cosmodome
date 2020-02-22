using InControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script is attached to the PlayerBox prefab, and is in charge of changing the state of the player box.
/// Flow goes like this:    <br/>
/// -> Starts flashing the Press X to join <br/>
/// -> <see cref="PlayerBox.AssignController"/> when <see cref="ControllerAllocation"/> assigns a controller to PlayerBox when the user presses X<br/>
/// -> <see cref="PlayerBox.Selecting"/> when it's this player's turn to select their character<br/>
/// -> <see cref="PlayerBox.Ready"/><br/> when this player has chosen their character and is ready
/// </summary>
public class PlayerBox : MonoBehaviour
{
    public delegate void PlayerBoxEvent();
    public event PlayerBoxEvent OnSelectLeft;
    public event PlayerBoxEvent OnSelectRight;
    public event PlayerBoxEvent OnSelect;

    /// <summary>
    /// The current InControl device for this player box
    /// </summary>
    [HideInInspector]
    public InputDevice controller = null;

    /// <summary>
    /// The character chosen by this player, or null if hasn't
    /// </summary>
    public CharacterSelectionOption chosenCharacter;

    /// <summary>
    /// If false, this player hasn't picked a character yet
    /// </summary>
    public bool hasChosenCharacter = false;

    /// <summary>
    /// Which player # this player box represents
    /// </summary>
    [Header("Which player # this player box represents")]
    [SerializeField]
    public int _playerNumber;

    /// <summary>
    /// Colour of this player box when activated
    /// </summary>
    [Header("Colour of this player box when activated")]
    [SerializeField]
    public Color playerColour;

    [SerializeField]
    Image _assignedBg;

    [SerializeField]
    Image _assignedBgFloater;

    [SerializeField]
    Text _playerNameText;

    [SerializeField]
    Text _playerStatusText;

    [SerializeField]
    GameObject _selectorArrow;

    Animator _animator;

    bool _isSelecting = false;

    /// <summary>
    /// Whether the selector arrow is enabled or not. 
    /// </summary>
    public bool selectorArrowEnabled
    {
        get { return _selectorArrow.activeSelf;  }
        set { _selectorArrow.SetActive(value);  }
    }

    private void Start()
    {
        this._animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (_isSelecting)
        {
            if (controller.DPadLeft.WasPressed || controller.LeftStickLeft.WasPressed) OnSelectLeft?.Invoke();
            if (controller.DPadRight.WasPressed || controller.LeftStickRight.WasPressed) OnSelectRight?.Invoke();
            if (controller.Action1.WasPressed) OnSelect?.Invoke();
        }
    }

    /// <summary>
    /// when <see cref="ControllerAllocation"/> assigns a controller to PlayerBox when the user presses X
    /// </summary>
    public void AssignController(InputDevice controller)
    {
        _animator.SetTrigger("AssignController");
        _playerNameText.text = "PLAYER " + _playerNumber;
        _assignedBg.color = playerColour;
        _assignedBgFloater.color = playerColour;
        _playerStatusText.text = "JOINED!";
        this.controller = controller;
    }

    /// <summary>
    /// when it's this player's turn to select their character
    /// </summary>
    public void Selecting()
    {
        _animator.SetTrigger("Selecting");
        _playerStatusText.text = "CHOOSING...";
        _isSelecting = true;
    }

    /// <summary>
    /// Called when this player has selected a character
    /// </summary>
    public void Selected(CharacterSelectionOption option)
    {
        _playerStatusText.text = "READY!";
        hasChosenCharacter = true;
        chosenCharacter = option;
    }

    /// <summary>
    /// when this player has chosen their character and is ready
    /// </summary>
    public void Ready()
    {

    }

}
