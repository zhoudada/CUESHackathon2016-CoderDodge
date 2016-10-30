using UnityEngine;
using System.Collections.Generic;

public class CCTest : MonoBehaviour
{
    private CharacterController _cc;

    void Awake()
    {
        _cc = GetComponent<CharacterController>();
    }

    void FixedUpdate()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        float speed = 5;
        float curSpeed = speed * Input.GetAxis("Vertical");
        _cc.SimpleMove(forward * curSpeed);
    }
}
