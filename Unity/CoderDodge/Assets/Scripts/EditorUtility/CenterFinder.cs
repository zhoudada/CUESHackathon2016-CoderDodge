using UnityEngine;
using System.Collections;

public abstract class CenterFinder : MonoBehaviour {
    public Transform WorldPositionCopyTo { get { return _worldPositionCopyTo; } }
    [SerializeField]
    [Range(0, 1)]
    private float _gizmoRadius = 0.001f;
    [SerializeField]
    private Color _gizmoColor = Color.yellow;
    [SerializeField]
    private Transform _worldPositionCopyTo;

    protected bool DrawGizmoSphere(Transform[] points)
    {
        int n = points.Length;
        Gizmos.color = _gizmoColor;
        for (int i = 0; i < n; i++)
        {
            Transform point = points[i];
            if (point == null)
            {
                return false;
            }
            Gizmos.DrawWireSphere(point.position, _gizmoRadius);
        }
        return true;
    }
}
