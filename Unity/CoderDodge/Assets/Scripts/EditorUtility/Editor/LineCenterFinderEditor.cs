using System;
using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.Assertions;

[CustomEditor(typeof(LineCenterFinder))]
public class LineCenterFinderEditor : CenterFinderEditor{
    protected override void FindCenter(CenterFinder script)
    {
        LineCenterFinder finder = script as LineCenterFinder;
        if (finder == null)
        {
            return;
        }
        Transform center = finder.transform;
        Transform startPoint = finder.StartPoint;
        Transform endPoint = finder.EndPoint;
        Undo.RecordObject(center, "Relocate center");
        center.position = (startPoint.position + endPoint.position) / 2;
    }

    void OnSceneGUI()
    {
        LineCenterFinder finder = Script as LineCenterFinder;
        if (finder == null)
        {
            return;
        }
        Transform startPoint = finder.StartPoint;
        Transform endPoint = finder.EndPoint;
        DrawPositionHandles(new [] { startPoint, endPoint });
    }
}
