using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public class SaveOnRun
{
    static SaveOnRun()
    {
        EditorApplication.playmodeStateChanged += SaveCurrentScene;
    }

    static void SaveCurrentScene()
    {
        if (!EditorApplication.isPlaying
            && EditorApplication.isPlayingOrWillChangePlaymode)
        {
            Scene activeScene = EditorSceneManager.GetActiveScene();
            EditorSceneManager.SaveScene(activeScene);
        }
    }
}