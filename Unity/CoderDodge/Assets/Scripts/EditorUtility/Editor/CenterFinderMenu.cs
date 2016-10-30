using UnityEngine;
using System.Collections;
using UnityEditor;

public class CenterFinderMenu : MonoBehaviour {

    protected static void CreateCenterFinderHelper<T>(string finderName, 
        string[] pointNames) where T: CenterFinder
    {
        GameObject centerFinder = new GameObject(
            GameObjectUtility.GetUniqueNameForSibling(null, finderName));
        int n = pointNames.Length;
        for (int i = 0; i < n; i++)
        {
            string pointName = pointNames[i];
            GameObject point = new GameObject(pointName);
            GameObjectUtility.SetParentAndAlign(point, centerFinder);
        }
        GameObject center = new GameObject("Center");
        GameObjectUtility.SetParentAndAlign(center, centerFinder);
        center.AddComponent<T>();
        Undo.RegisterCreatedObjectUndo(centerFinder, "Create center finder");
        Selection.activeGameObject = center;
    } 
}
