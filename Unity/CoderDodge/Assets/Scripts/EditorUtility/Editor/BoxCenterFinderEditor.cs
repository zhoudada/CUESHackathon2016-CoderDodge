using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(BoxCenterFinder))]
public class BoxCenterFinderEditor : CenterFinderEditor {

    protected override void FindCenter(CenterFinder script)
    {
        BoxCenterFinder finder = script as BoxCenterFinder;
        if (finder == null)
        {
            return;
        }
        Transform startPointOnFace1 = finder.StartPointOnFace1;
        Transform endPointOnFace1 = finder.EndPointOnFace1;
        Transform startPointOnFace2 = finder.StartPointOnFace2;
        Transform endPointOnFace2 = finder.EndPointOnFace2;
        Transform center = finder.transform;
        Undo.RecordObject(center, "Relocate center");
        Vector3 centerOfFace1 = (startPointOnFace1.position + endPointOnFace1.position) / 2;
        Vector3 centerOfFace2 = (startPointOnFace2.position + endPointOnFace2.position) / 2;
        center.position = (centerOfFace1 + centerOfFace2) / 2;
    }

    void OnSceneGUI()
    {
        BoxCenterFinder finder = Script as BoxCenterFinder;
        if (finder == null)
        {
            return;
        }
        Transform startPointOnFace1 = finder.StartPointOnFace1;
        Transform endPointOnFace1 = finder.EndPointOnFace1;
        Transform startPointOnFace2 = finder.StartPointOnFace2;
        Transform endPointOnFace2 = finder.EndPointOnFace2;
        DrawPositionHandles(new[] {startPointOnFace1, endPointOnFace1,
            startPointOnFace2, endPointOnFace2});
    }
}
