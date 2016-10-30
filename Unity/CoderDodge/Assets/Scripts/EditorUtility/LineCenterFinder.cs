using UnityEngine;
using System.Collections;

public class LineCenterFinder : CenterFinder
{
    public Transform StartPoint { get { return _startPoint; } }
    public Transform EndPoint { get { return _endPoint; } }
    [SerializeField]
    private Transform _startPoint;
    [SerializeField]
    private Transform _endPoint;

    void Reset()
    {
        Transform parent = transform.parent;
        if (parent == null)
        {
            return;
        }
        if (parent.childCount < 3)
        {
            return;
        }
        _startPoint = parent.GetChild(0);
        _endPoint = parent.GetChild(1);
    }

    void OnDrawGizmosSelected()
    {
        if (DrawGizmoSphere(new Transform[] { _startPoint, _endPoint }))
        {
            Gizmos.DrawLine(_startPoint.position, _endPoint.position);
        }
    }
}
