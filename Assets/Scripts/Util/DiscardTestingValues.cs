using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Editor script that restores testing values to default when the project is open, to provent test values getting pushed
/// </summary>
[ExecuteInEditMode]
public class DiscardTestingValues: MonoBehaviour
{
    [SerializeField]
    [HideInInspector]
    bool _needsRestore = true;

    void Start() {
        if (EditorApplication.isPlayingOrWillChangePlaymode) return;
        if (!_needsRestore) return;
        RestoreValues();

        EditorSceneManager.sceneClosing  += OnSceneClosing;
        //_needsRestore = false;
    }

    void OnSceneClosing(Scene scene, bool removingScene) {
        if (!removingScene) return;
        _needsRestore = true;
    }

    void RestoreValues() {
        List<string> testingValuesChanged = new List<string>();

        void CheckIfValueChanged(string result) {
            if (result == null) return;
            testingValuesChanged.Add(result);
        }

        ScoreManager sm = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();

        if (sm != null) {
            CheckIfValueChanged(RestoreRoundTime(sm));
        }

        CutscenesManager cm = GameObject.Find("CutscenesManager").GetComponent<CutscenesManager>();
        if (cm != null) {
            CheckIfValueChanged(RestoreIntroCutscenes(cm));
        }

        if (testingValuesChanged.Count == 0) return; // We didn't need to change anything

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

        if (sm.timeLeftInRound != 60) {
            sm.timeLeftInRound = 60;
            changed = true;
        }

        if (sm.roundLength != 60) {
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


}
