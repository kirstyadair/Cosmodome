using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Hacky way to proxy the animation events from the camera Animator to the CutscenesManager
/// </summary>
public class CutsceneAnimationEventProxy : MonoBehaviour
{
    [SerializeField] CutscenesManager _cutscenes;


    /// <summary>
    /// Triggered when characters should sit down intro anims
    /// </summary>
    public void Animator_OnCharactersSitDown() {
        _cutscenes.Animator_OnCharactersSitDown();
    }

}
