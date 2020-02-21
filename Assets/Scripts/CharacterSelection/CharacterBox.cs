using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBox : MonoBehaviour
{
    /// <summary>
    /// PlayerType this character box will select
    /// </summary>
    public PlayerTypes type;


    CharacterSelectionOption _option;

    /// <summary>
    /// Set the <see cref="CharacterSelectionOption"/> connected to the <see cref="PlayerTypes"/>
    /// </summary>
    /// <param name="option"><see cref="CharacterSelectionOption"/> to set</param>
    public void SetCharacterOption(CharacterSelectionOption option)
    {
        _option = option;
    }
}
