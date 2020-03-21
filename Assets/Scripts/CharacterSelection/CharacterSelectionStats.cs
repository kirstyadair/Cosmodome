using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionStats : MonoBehaviour
{
    [SerializeField]
    Text _bioText;

    [SerializeField]
    Text _weaponText;

    [SerializeField]
    CharacterSelectionDotter _speedDotter;

    [SerializeField]
    CharacterSelectionDotter _sizeDotter;

    [SerializeField]
    Text _characterText;

    Animator _animator;

    CharacterSelectionOption _option;

    /// <summary>
    /// Update the stats displayed by the stat box (also triggering an animation)
    /// </summary>
    /// <param name="option"><see cref="CharacterSelectionOption"/> to display</param>
    public void ChangeStats(CharacterSelectionOption option)
    {
        this._option = option;
        _animator.Play("Flip", -1, 0);
    }

    /// <summary>
    ///  Called by the animator when we can switch out the stats
    /// </summary>
    public void AnimationChangeStats()
    {
        _speedDotter.SetDots(_option.speedStat);
        _sizeDotter.SetDots(_option.sizeStat);
        _bioText.text = _option.infoStat;
        _weaponText.text = _option.weaponStat;
        _characterText.text = _option.characterName;
        _characterText.color = _option.characterColor;
    }

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }
}
