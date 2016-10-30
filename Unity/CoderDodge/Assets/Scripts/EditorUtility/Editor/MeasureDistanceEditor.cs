using UnityEngine;
using System.Collections;
using System.Text;
using UnityEditor;

[CustomEditor(typeof(MeasureDistance))]
public class MeasureDistanceEditor : Editor {
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawDefaultInspector();
        MeasureDistance script = serializedObject.targetObject as MeasureDistance;
        if (script == null)
        {
            return;
        }
        Transform startPoint = script.StartPoint;
        StringBuilder ss = new StringBuilder("Distance: ");
        if (startPoint != null)
        {
            float distance = Vector3.Distance(startPoint.position, 
                script.transform.position);
            ss.Append(distance);
        }
        EditorGUILayout.LabelField(ss.ToString());
        serializedObject.ApplyModifiedProperties();
    }
}
