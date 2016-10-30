using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(RingColliderMaker))]
public class RingColliderMakerEditor : Editor
{
    private RingColliderMaker _script;

    void OnEnable()
    {
        _script = serializedObject.targetObject as
            RingColliderMaker;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawDefaultInspector();
        if (_script == null)
        {
            return;
        }
        if (GUILayout.Button("Make"))
        {
            MakeRingColliders();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void MakeRingColliders()
    {
        Transform center = _script.Center;
        Vector3 localRotationAxis = _script.CenterLocalRotationAxis;
        Transform ringElement = _script.RingColliderElement;
        if (center == null || ringElement == null)
        {
            return;
        }
        int n = _script.ElementNumber;
        if (n > 100)
        {
            n = 100;
        }
        float angle = _script.Angle;
        float angleStep = angle / n;
        float curAngle = angleStep;
        Transform parent = ringElement.parent;
        Vector3 rotationAxisInWorld = center.TransformDirection(localRotationAxis);
        int maxTry = 1000;
        int curTry = 0;
        while (curAngle <= angle && curTry < maxTry)
        {
            Transform newElement = Instantiate(ringElement,
                ringElement.position, ringElement.rotation) as Transform;
            newElement.SetParent(parent);
            newElement.localScale = ringElement.localScale;
            newElement.RotateAround(center.position, 
                rotationAxisInWorld, curAngle);
            Undo.RegisterCreatedObjectUndo(newElement.gameObject, "Create new ring element");
            curAngle += angleStep;
            curTry ++;
        }
        if (curTry == maxTry)
        {
            Debug.LogError("max try is reached.");
        }
    }
}
