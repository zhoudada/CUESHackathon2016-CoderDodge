using UnityEngine;
using System.Collections;

public class RingColliderMaker : MonoBehaviour
{
    public Transform Center { get { return _center; } }
    public Vector3 CenterLocalRotationAxis { get { return _centerLocalRototationAxis;} }
    public Transform RingColliderElement { get { return _ringColliderElement; } }
    public int ElementNumber { get { return _elementNumber; } }
    public float Angle { get { return _angle; } }

    [SerializeField]
    private Transform _center;
    [SerializeField]
    private Vector3 _centerLocalRototationAxis;
    [SerializeField]
    private Transform _ringColliderElement;
    [SerializeField]
    private int _elementNumber;
    [SerializeField]
    private float _angle;
}
