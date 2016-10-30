using UnityEngine;
using System.Collections;
using UnityEditor;

public class CircleCenterFinderMenu : MonoBehaviour {

    [MenuItem("GameObject/Position Tools/Circle Center Finder", false, 10)]
    static void CreateCenterFinder()
    {
        GameObject centerFinder = new GameObject(
            GameObjectUtility.GetUniqueNameForSibling(null, "CircleCenterFinder"));
        GameObject pointA = new GameObject("PointA");
        GameObject pointB = new GameObject("PointB");
        GameObject pointC = new GameObject("PointC");
        GameObject center = new GameObject("Center");
        GameObjectUtility.SetParentAndAlign(pointA, centerFinder);
        GameObjectUtility.SetParentAndAlign(pointB, centerFinder);
        GameObjectUtility.SetParentAndAlign(pointC, centerFinder);
        GameObjectUtility.SetParentAndAlign(center, centerFinder);
        center.AddComponent<CircleCenterFinder>();
        Undo.RegisterCreatedObjectUndo(centerFinder, "Create center finder");
        Selection.activeGameObject = center;
    }
}
