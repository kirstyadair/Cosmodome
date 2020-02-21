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

    public void AnimatorSwapModel()
    {
        Debug.Log("swapping model to " + _modelName);
        foreach (Transform transform in modelContainer)
        {
            if (transform.gameObject.name == _modelName) transform.gameObject.SetActive(true);
            else transform.gameObject.SetActive(false);
        }
    }
}
