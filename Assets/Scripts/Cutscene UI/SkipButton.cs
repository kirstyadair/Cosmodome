using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkipButton : MonoBehaviour
{
    [SerializeField]
    Image _filler;
    
    Animator _animator;

    void Awake() {
        _animator = GetComponent<Animator>();
    }

    public void Appear(Color color) {
        _animator.Play("Appear");
        _animator.ResetTrigger("Dissapear");
        _animator.ResetTrigger("Selected");
        _animator.ResetTrigger("Deselected");
        _filler.color = color;
    }

    public void UpdateProgress(float progress) {
        _filler.fillAmount = progress;
    }

    public void Dissapear() {
        _animator.SetTrigger("Dissapear");
    }

    public void StartSelecting() {
        _animator.SetTrigger("Selected");
    }

    public void StopSelecting() {
        _animator.SetTrigger("Deselected");
    }
}
