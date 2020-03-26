using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Editor script that restores testing values to default when the project is open, to provent test values getting pushed
/// </summary>
[ExecuteInEditMode]
public class DiscardTestingValues: MonoBehaviour
{
      #if UNITY_EDITOR
    void Start() {
        EditorSceneManager.sceneOpened  += OnSceneOpened;
    }

    void OnSceneOpened(Scene scene, OpenSceneMode mode) {
        if (EditorApplication.isPlayingOrWillChangePlaymode) return;
        RestoreValues();
    }

    void RestoreValues() {
        List<string> testingValuesChanged = new List<string>();

        void CheckIfValueChanged(string result) {
            if (string.IsNullOrEmpty(result)) return;
            testingValuesChanged.Add(result);
        }
        
        GameObject scoreManager = GameObject.Find("ScoreManager");
        GameObject cutscenesManager = GameObject.Find("CutscenesManager");
        GameObject characterSelectionLogic = GameObject.Find("Character Selection Logic");

        if (scoreManager != null) {
            ScoreManager sm = scoreManager.GetComponent<ScoreManager>();
            CheckIfValueChanged(RestoreRoundTime(sm));
        }

        if (cutscenesManager != null) {
            CutscenesManager cm = cutscenesManager.GetComponent<CutscenesManager>();
            CheckIfValueChanged(RestoreIntroCutscenes(cm));
            CheckIfValueChanged(RestoreEndOfRoundCutscenes(cm));
        }

        if (characterSelectionLogic != null) {
            ControllerAllocation ca = characterSelectionLogic.GetComponent<ControllerAllocation>();
            CheckIfValueChanged(RestoreAllowOnePlayer(ca));
            CheckIfValueChanged(RestoreAllowManyControl(ca));
        }


        if (testingValuesChanged.Count == 0) return; // We didn't need to change anything

        // To allow saving
        EditorSceneManager.MarkAllScenesDirty();

        string log = "<color=lightblue><b>DiscardTestingValues has set these values to defaults because test changes were pushed: </b></color>";

        foreach (string valueChanged in testingValuesChanged) {
            log += "<i> || " + valueChanged + "</i>";
        }

        Debug.Log(log);
    }



    /// VALUE RESTORING METHODS
    /// return string is the friendly name to show in editor, or null if not changed

    string RestoreRoundTime(ScoreManager sm) {
        bool changed = false;

        // to account for float error
        if (Mathf.RoundToInt(sm.timeLeftInRound) != 60) {
            sm.timeLeftInRound = 60;
            changed = true;
        }

        if (Mathf.RoundToInt(sm.roundLength) != 60) {
            sm.roundLength = 60;
            changed = true;
        }

        if (!changed) return null;

        return "<b>Round length</b> changed to <b>60</b>";
    }

    string RestoreIntroCutscenes(CutscenesManager cm) {
        if (!cm.shouldShowIntroCutscenes) {
            cm.shouldShowIntroCutscenes = true;
            return "Set <b>ShouldShowIntroCutScenes</b> to <b>true</b>";
        }

        return null;
    }

    
    string RestoreEndOfRoundCutscenes(CutscenesManager cm) {
        if (!cm.shouldShowEndofRoundCutscenes) {
            cm.shouldShowEndofRoundCutscenes = true;
            return "Set <b>ShouldShowEndofRoundCutscenes</b> to <b>true</b>";
        }

        return null;
    }



    string RestoreAllowOnePlayer(ControllerAllocation ca) {
        if (ca.allowOnePlayer) {
            ca.allowOnePlayer = false;
            return "Set <b>Allow one player</b> to <b>false</b>";
        }

        return null;
    }

    string RestoreAllowManyControl(ControllerAllocation ca) {
        if (ca.allowControlAllWithOne) {
            ca.allowControlAllWithOne = false;
            return "Set <b>Allow control all with one</b> to <b>false</b>";
        }

        return null;
    }

    #endif

}
