using InControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelectionBox : MonoBehaviour
{
    bool readyDebounce = true;
    public bool isReady = false;

    public delegate void SelectionBoxEvent();
    public event SelectionBoxEvent OnReadyUp;
    public event SelectionBoxEvent OnUnReady;

    public InputDevice controller = null;

    public GameObject characterBox;
    public Text characterName;

    public GameObject joinText;
    public GameObject confirmText;
    public GameObject readyText;
    public GameObject alreadyPicked;

    public Image arrowLeft;
    public Image arrowRight;
    public Image xButtonImage;

    public Color xButtonReadyColour;
    public Color xButtonUnReadyColour;
    public Color arrowHighlightColour;
    public Color arrowNormalColour;
    public Color characterNameNormalColour;
    public Color characterNameUsedColour;

    int currentSelectedCharacter = 0;
    ControllerAllocation controllerAllocation;
    CharacterSelectionOption selectedCharacter = null;

    public void AllocateController(InputDevice controller, ControllerAllocation controllerAllocation)
    {
        this.controller = controller;
        this.controllerAllocation = controllerAllocation;
        this.controllerAllocation.OnAnotherBoxChanged += OnAnotherBoxChanged;

        ShowCharacterSelection();
        ChangeSelection(0);
        readyDebounce = true; // to avoid accidentially clicking thru to ready up
    }

    public void ShowCharacterSelection()
    {
        arrowLeft.gameObject.SetActive(true);
        arrowRight.gameObject.SetActive(true);
        joinText.gameObject.SetActive(false);
        characterBox.SetActive(true);
        confirmText.SetActive(true);
        readyText.SetActive(false);
        xButtonImage.color = xButtonUnReadyColour;
    }

    public void ShowReadyUp()
    {
        arrowLeft.gameObject.SetActive(false);
        arrowRight.gameObject.SetActive(false);
        joinText.gameObject.SetActive(false);
        confirmText.SetActive(false);
        characterBox.SetActive(true);
        readyText.SetActive(true);
        xButtonImage.color = xButtonReadyColour;
    }

    public void SelectRight()
    {
        currentSelectedCharacter++;
        if (currentSelectedCharacter >= controllerAllocation.selectableCharacters.Length) currentSelectedCharacter = 0;

        ChangeSelection(currentSelectedCharacter);
    }

    public void OnAnotherBoxChanged()
    {
        ChangeSelection(currentSelectedCharacter);
    }

    public void ChangeSelection(int i)
    {
        selectedCharacter = controllerAllocation.selectableCharacters[i];
        characterName.text = selectedCharacter.characterName;

        if (selectedCharacter.chosenBy != null && selectedCharacter.chosenBy != this)
        {
            characterName.color = characterNameUsedColour;
            alreadyPicked.SetActive(true);
        }
        else
        {
            characterName.color = characterNameNormalColour;
            alreadyPicked.SetActive(false);
        }
    }

    public void SelectLeft()
    {
        currentSelectedCharacter--;
        if (currentSelectedCharacter < 0) currentSelectedCharacter = controllerAllocation.selectableCharacters.Length - 1;

        ChangeSelection(currentSelectedCharacter);
    }

    public void Update()
    {
        if (controller == null) return;


        if (!isReady)
        {
            // Pressed X on selection screen, ready up
            if (controller.Action1.WasPressed && selectedCharacter.chosenBy == null)
            {
                if (readyDebounce)
                {
                    readyDebounce = false;
                    return;
                }

                ShowReadyUp();
                selectedCharacter.chosenBy = this;
                isReady = true;
                OnReadyUp?.Invoke();
            }

            // Highlight the selection arrows when left D pad or left stick is pressed
            if (controller.LeftStickLeft.IsPressed || controller.DPadLeft.IsPressed) arrowLeft.color = arrowHighlightColour;
            else arrowLeft.color = arrowNormalColour;

            if (controller.LeftStickRight.IsPressed || controller.DPadRight.IsPressed) arrowRight.color = arrowHighlightColour;
            else arrowRight.color = arrowNormalColour;

            // once we've done the click, change selection
            if (controller.LeftStickLeft.WasPressed || controller.DPadLeft.WasPressed) SelectLeft();
            if (controller.LeftStickRight.WasPressed || controller.DPadRight.WasPressed) SelectRight();

        } else
        {
            // Pressed X or O while readied up, so unready
            if (controller.Action1.WasPressed || controller.Action2.WasPressed)
            {
                ShowCharacterSelection();
                selectedCharacter.chosenBy = null;
                isReady = false;
                OnUnReady?.Invoke();
            }
        }
    }
}
