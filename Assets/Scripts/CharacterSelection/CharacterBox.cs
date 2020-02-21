using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBox : MonoBehaviour
{
    /// <summary>
    /// PlayerType this character box will select
    /// </summary>
    public PlayerTypes type;

    [SerializeField]
    Image _selectorImage;

   public CharacterSelectionOption option;
    Animator _animator;

    public bool isHovered = false;

    /// <summary>
    /// Set the <see cref="CharacterSelectionOption"/> connected to the <see cref="PlayerTypes"/>
    /// </summary>
    /// <param name="option"><see cref="CharacterSelectionOption"/> to set</param>
    public void SetCharacterOption(CharacterSelectionOption option)
    {
        this.option = option;
    }

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Hover(PlayerBox hoveredPlayer)
    {
        _selectorImage.color = hoveredPlayer.playerColour;
        _animator.SetBool("isHovered", true);
        isHovered = true;
    }

    public void Unhover()
    {
        isHovered = false;
        _animator.SetBool("isHovered", false);
    }
}
