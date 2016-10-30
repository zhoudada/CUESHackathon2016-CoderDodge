using UnityEngine;
using System.Collections;
using UnityEditor;

public class BoxCenterFinderMenu : CenterFinderMenu {
    [MenuItem("GameObject/Position Tools/Box Center Finder", false, 10)]
    static void CreateCenterFinder()
    {
        CreateCenterFinderHelper<BoxCenterFinder>("BoxCenterFinder",
            new[] { "StartPointOnFace1", "EndPointOnFace1",
                "StartPointOnFace2", "EndPointOnFace2" });
    }

}
