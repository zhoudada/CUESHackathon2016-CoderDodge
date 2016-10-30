using UnityEngine;
using System.Collections;

public class TransformModifier : MonoBehaviour
{
    public Transform Target { get { return _target; } }
    [SerializeField]
    private Transform _target;
}
