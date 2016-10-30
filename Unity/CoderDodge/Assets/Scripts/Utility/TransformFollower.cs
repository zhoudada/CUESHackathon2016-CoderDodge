using UnityEngine;
using System.Collections.Generic;
using Utility;

public class TransformFollower : MonoBehaviour
{
    [SerializeField]
    private Transform _target;
    [SerializeField]
    private bool _followX;
    [SerializeField]
    private bool _followY;
    [SerializeField]
    private bool _followZ;

    void Awake()
    {
        Miscellaneous.CheckNullAndLogError(_target);
    }

    void Update()
    {
        Vector3 newPosition = transform.position;
        if (_followX)
        {
            newPosition.x = _target.position.x;
        }
        else if (_followY)
        {
            newPosition.y = _target.position.y;
        }
        else if (_followZ)
        {
            newPosition.z = _target.position.z;
        }
        transform.position = newPosition;
    }

}
