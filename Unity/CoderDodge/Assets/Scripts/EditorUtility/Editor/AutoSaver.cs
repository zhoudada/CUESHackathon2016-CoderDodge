using UnityEngine;
using System.Collections;
using System.Diagnostics;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Timers;

[InitializeOnLoad]
public class AutoSaver
{
    private static Timer _savingTimer;
    private static bool _savingRequired;

    static AutoSaver()
    {
        _savingTimer = new Timer(60000);
        _savingTimer.Elapsed += ((obj, args) =>
        {
            _savingRequired = true;
        });
        EditorApplication.update += EditorUpdate;
        _savingTimer.AutoReset = true;
        _savingTimer.Start();
    }

    static void EditorUpdate() {
        if (_savingRequired)
        {
            SaveCurrentScene();
            _savingRequired = false;
        }
    }

    static void SaveCurrentScene()
    {
        if (!EditorApplication.isPlaying)
        {
            UnityEngine.Debug.Log("World saved.");
            Scene activeScene = EditorSceneManager.GetActiveScene();
            EditorSceneManager.SaveScene(activeScene);
        }
    }
}
