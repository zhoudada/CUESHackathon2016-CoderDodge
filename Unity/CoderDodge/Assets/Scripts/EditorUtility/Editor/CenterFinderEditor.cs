using UnityEngine;
using System.Collections;
using UnityEditor;

public abstract class CenterFinderEditor : Editor {
    protected CenterFinder Script { get; private set; }

    void OnEnable()
    {
        Script = serializedObject.targetObject
            as CenterFinder;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawDefaultInspector();

        if (Script == null)
        {
            return;
        }
        if (GUILayout.Button("Find Center"))
        {
            FindCenter(Script);
        }
        if (GUILayout.Button("Copy center to target"))
        {
            CopyWorldPositionToTransform(Script.transform.position, Script.WorldPositionCopyTo);
        }
        serializedObject.ApplyModifiedProperties();
    }

    protected abstract void FindCenter(CenterFinder script);

    private void CopyWorldPositionToTransform(Vector3 worldPosition, Transform target)
    {
        if (target == null)
        {
            return;
        }
        Undo.RecordObject(target, "Copy world position to target");
        target.position = worldPosition;
    }

    protected void DrawPositionHandles(Transform[] points)
    {
        int n = points.Length;
        for (int i = 0; i < n; i++)
        {
            Transform point = points[i];
            if (point == null)
            {
                return;
            }
            Undo.RecordObject(point, "Relocate point");
            point.position = Handles.PositionHandle(
                point.position, point.rotation);
        }
    }
}
