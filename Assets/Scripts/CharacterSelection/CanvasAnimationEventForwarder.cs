using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A hacky solution to just pass animation events onto <see cref="PlayerSelection"/> which is in another GameObject
/// </summary>
public class CanvasAnimationEventForwarder : MonoBehaviour
{
    public PlayerSelection playerSelection;

    public void AnimatorUpdatePlayerBoxRow()
    {
        playerSelection.AnimatorUpdatePlayerBoxRow();
    }

    public void Animator_FinishedCharacterControllerTransition()
    {
        playerSelection.Animator_FinishedCharacterControllerTransition();
    }
}
