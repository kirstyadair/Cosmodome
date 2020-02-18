using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    [SerializeField]
    Text _textComponent;

    string _text;
    Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Change the text shown in the status bar
    /// </summary>
    /// <param name="text">The text to change the status bar to</param>
    public void ChangeText(string text)
    {
        _text = text;
        _animator.Play("change text", -1, 0);
    }

    /// <summary>
    /// Change the text shown in the status bar, with "important" animation
    /// </summary>
    /// <param name="text">The text to change the status bar to</param>
    public void ChangeTextImportant(string text)
    {
        _text = text;
        _animator.Play("change text important", -1, 0);
    }

    /// <summary>
    /// Called by Animator event
    /// </summary>
    public void AnimatorSetStatusBarText()
    {
        _textComponent.text = _text;
    }
}
