using UnityEngine;
using System.Collections;

public class BoxCenterFinder : CenterFinder {
    public Transform StartPointOnFace1 { get { return _startPointOnFace1; } }
    public Transform EndPointOnFace1 { get { return _endPointOnFace1; } }
    public Transform StartPointOnFace2 { get { return _startPointOnFace2; } }
    public Transform EndPointOnFace2 { get { return _endPointOnFace2; } }
    [SerializeField]
    private Transform _startPointOnFace1;
    [SerializeField]
    private Transform _endPointOnFace1;
    [SerializeField]
    private Transform _startPointOnFace2;
    [SerializeField]
    private Transform _endPointOnFace2;

    void Reset()
    {
        Transform parent = transform.parent;
        if (parent == null)
        {
            return;
        }
        if (parent.childCount < 5)
        {
            return;
        }
        _startPointOnFace1 = parent.GetChild(0);
        _endPointOnFace1 = parent.GetChild(1);
        _startPointOnFace2 = parent.GetChild(2);
        _endPointOnFace2 = parent.GetChild(3);
    }

    void OnDrawGizmosSelected()
    {
        if (DrawGizmoSphere(new[] {_startPointOnFace1, _startPointOnFace2,
                _endPointOnFace1, _endPointOnFace2}))
        {
            Gizmos.DrawLine(_startPointOnFace1.position, _endPointOnFace1.position);
            Gizmos.DrawLine(_startPointOnFace2.position, _endPointOnFace2.position);
        }
    }
}
