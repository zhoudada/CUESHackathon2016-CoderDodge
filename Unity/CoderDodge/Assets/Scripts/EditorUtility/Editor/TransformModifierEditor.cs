using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(TransformModifier))]
public class TransformModifierEditor : Editor
{
    private TransformModifier _script;

    void OnEnable()
    {
        _script = serializedObject.targetObject as TransformModifier;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawDefaultInspector();
        if (_script == null)
        {
            return;
        }
        if (GUILayout.Button("Copy World Position"))
        {
            CopyWorldPosition();
        }
        if (GUILayout.Button("Copy World Rotation"))
        {
            CopyWorldRotation();
        }
        serializedObject.ApplyModifiedProperties();
    }

    private void CopyWorldPosition()
    {
        Transform transform = _script.transform;
        Undo.RecordObject(transform, "Copy position");
        transform.position = _script.Target.position;
    }

    private void CopyWorldRotation()
    {
        Transform transform = _script.transform;
        Undo.RecordObject(transform, "Copy rotation");
        transform.rotation = _script.Target.rotation;
    }
}
