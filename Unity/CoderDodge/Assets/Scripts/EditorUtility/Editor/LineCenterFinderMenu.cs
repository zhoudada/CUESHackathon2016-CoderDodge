using UnityEngine;
using System.Collections;
using UnityEditor;

public class LineCenterFinderMenu : CenterFinderMenu {

    [MenuItem("GameObject/Position Tools/Center Finder", false, 10)]
    static void CreateCenterFinder()
    {
        CreateCenterFinderHelper<LineCenterFinder>("LineCenterFinder",
            new[] { "StartPoint", "EndPoint" });
    }
}
