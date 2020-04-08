using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModelDisplay : MonoBehaviour
{
    string _modelName;
    Animator _animator;

    public Transform modelContainer;

    void Awake()
    {
        this._animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Starts the animation to swap out the character model
    /// </summary>
    /// <param name="newModelName">Name of the gameobject which is a child of this to swap out to</param>
    public void SwapModel(string newModelName)
    {
        _modelName = newModelName;
        _animator.SetTrigger("Swap");
    }

    public void ReadyUp() {
        foreach (Transform transform in modelContainer)
        {
            if (transform.gameObject.name == _modelName) {
                try {
                    transform.GetComponentInChildren<Animator>().Play("Ready up");
                } catch (Exception e) {
                    Debug.Log("Error playing Readyup animation");
                }

            }
        }
    }

    public void HoverAnim() {
        foreach (Transform transform in modelContainer)
        {
            if (transform.gameObject.name == _modelName) {
                try {
                    transform.GetComponentInChildren<Animator>().Play("Hover");
                } catch (Exception e) {
                    Debug.Log("Error playing Hover animation");
                }

            }
        }
    }

    public void AnimatorSwapModel()
    {
        foreach (Transform transform in modelContainer)
        {
            if (transform.gameObject.name == _modelName) {
                transform.gameObject.SetActive(true);
                HoverAnim();
            }
            else if (transform.gameObject.GetComponent<Light>() == null) transform.gameObject.SetActive(false);
        }
    }
}
