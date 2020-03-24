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
        Debug.Log("Awake");
    }

    public void Appear(Color color) {
        Debug.Log("Appear");
        _animator.Play("Appear");
        _animator.ResetTrigger("Dissapear");
        _animator.ResetTrigger("Selected");
        _animator.ResetTrigger("Deselected");
        _filler.color = color;
    }

    public void UpdateProgress(float progress) {
        Debug.Log("Progress: " + progress);
        _filler.fillAmount = progress;
    }

    public void Dissapear() {
        Debug.Log("Dissapear");
        _animator.SetTrigger("Dissapear");
    }

    public void StartSelecting() {
        Debug.Log("Start selecting");
        _animator.SetTrigger("Selected");
    }

    public void StopSelecting() {
        Debug.Log("Stop selecting");
        _animator.SetTrigger("Deselected");
    }
}
