using UnityEngine;
using System.Collections;

public class CircleCenterFinder : MonoBehaviour {
    public Transform PointA { get { return _pointA; } }
    public Transform PointB { get { return _pointB; } }
    public Transform PointC { get { return _pointC; } }
    public Transform WorldPositionCopyTo { get { return _worldPositionCopyTo; } }
    public float ConvergeRate { get { return _convergeRate; } }
    [SerializeField]
    private Transform _pointA;
    [SerializeField]
    private Transform _pointB;
    [SerializeField]
    private Transform _pointC;
    [SerializeField]
    [Range(0, 1)]
    private float _gizmoRadius = 0.001f;
    [SerializeField]
    private Color _gizmoColor = Color.yellow;
    [SerializeField]
    private Transform _worldPositionCopyTo;
    [SerializeField]
    private float _convergeRate;

    void Reset()
    {
        Transform parent = transform.parent;
        if (parent == null)
        {
            return;
        }
        if (parent.childCount < 4)
        {
            return;
        }
        _pointA = parent.GetChild(0);
        _pointB = parent.GetChild(1);
        _pointC = parent.GetChild(2);
    }

    void OnDrawGizmosSelected()
    {
        if (_pointA == null || _pointB == null || _pointC == null)
        {
            return;
        }
        Gizmos.color = _gizmoColor;
        Gizmos.DrawWireSphere(_pointA.position, _gizmoRadius);
        Gizmos.DrawWireSphere(_pointB.position, _gizmoRadius);
        Gizmos.DrawWireSphere(_pointC.position, _gizmoRadius);
        Gizmos.DrawLine(_pointA.position, _pointB.position);
        Gizmos.DrawLine(_pointB.position, _pointC.position);
    }
}
