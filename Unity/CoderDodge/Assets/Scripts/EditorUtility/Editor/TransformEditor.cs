using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Transform))]
public class TransformEditor : Editor {
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        Transform transform = (Transform)target;
        EditorGUI.BeginChangeCheck();
        Vector3 position = EditorGUILayout.Vector3Field("World Position", transform.position);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Changed transform.");
            transform.position = position;
            EditorGUILayout.Vector3Field("Local position", transform.localPosition);
            EditorGUILayout.Vector3Field("Local rotation", transform.localEulerAngles);
            EditorGUILayout.Vector3Field("Local Scale", transform.localScale);
        }
        else
        {
            EditorGUI.BeginChangeCheck();
            Vector3 localPosition = EditorGUILayout.Vector3Field("Local position", transform.localPosition);
            Vector3 localEulerAngles = EditorGUILayout.Vector3Field("Local rotation", transform.localEulerAngles);
            Vector3 localScale = EditorGUILayout.Vector3Field("Local Scale", transform.localScale);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Changed transform.");
                transform.position = position;
                transform.localPosition = localPosition;
                transform.localEulerAngles = localEulerAngles;
                transform.localScale = localScale;
            }
        }
        
        serializedObject.ApplyModifiedProperties();
    }

}
