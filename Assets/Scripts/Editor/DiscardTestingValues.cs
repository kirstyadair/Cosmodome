using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Editor script that restores testing values to default when the project is open, to provent test values getting pushed
/// </summary>
[ExecuteInEditMode]
public class DiscardTestingValues: MonoBehaviour
{
    public bool _needsRestore = true;

    void Start() {
        //aa
        if (!_needsRestore) return;
        RestoreValues();

        _needsRestore = false;
    }

    void RestoreValues() {
        Debug.Log("<b>DiscardTestingValues has set these values to defaults, as you probably didn't mean to push them:</b><br/>Round length");
    }
}
