using UnityEngine;
using System.Collections;
using UnityEditor;

public class MeasureDistanceMenu : MonoBehaviour {

    [MenuItem("GameObject/Position Tools/Measure Distance Object", false, 10)]
    static void CreateMeasureDistanceObject()
    {
        GameObject startPoint = new GameObject("StartPoint");
        GameObject endPoint = new GameObject("EndPoint");
        GameObject obj = new GameObject("MeasureDistance");
        GameObjectUtility.SetParentAndAlign(startPoint, obj);
        GameObjectUtility.SetParentAndAlign(endPoint, obj);
        endPoint.AddComponent<MeasureDistance>();
        Undo.RegisterCreatedObjectUndo(startPoint, "Create start point");
        Undo.RegisterCreatedObjectUndo(endPoint, "Create end point");
        Undo.RegisterCreatedObjectUndo(obj, "Create measure distance object");
        Selection.activeGameObject = endPoint;
    }
}
