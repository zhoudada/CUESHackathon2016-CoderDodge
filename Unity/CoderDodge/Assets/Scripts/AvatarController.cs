using UnityEngine;
using System.Collections;
using Utility;

public class AvatarController : MonoBehaviour
{
    private enum AvatarPositionState
    {
        Left,
        Center,
        Right
    }

    [SerializeField]
    private Transform _rightDirectionReference;
    [SerializeField]
    private Transform _upwardDirectionReference;
    [SerializeField]
    private Transform _forwardDirectionReference;
    [SerializeField]
    private Transform _centerReference;
    [SerializeField]
    private Rigidbody _rb;
    [SerializeField]
    private UnityChanControlScriptWithRgidBody _unityChanController;

    private AvatarPositionState _avatarPositionState;
    private Vector3 _targetPositionWrtCenter;
    private float _coolDownTime = 1f;
    private float _coolDownStart;

    void Awake()
    {
        Miscellaneous.CheckNullAndLogError(_rb);
        Miscellaneous.CheckNullAndLogError(_rightDirectionReference);
        Miscellaneous.CheckNullAndLogError(_upwardDirectionReference);
        Miscellaneous.CheckNullAndLogError(_forwardDirectionReference);
        Miscellaneous.CheckNullAndLogError(_centerReference);
        Miscellaneous.CheckNullAndLogError(_unityChanController);
        _avatarPositionState = AvatarPositionState.Center;
        _coolDownStart = -_coolDownTime;
    }

    public void Move(Vector3 change)
    {
        _rb.position += change;
    }

    private bool CheckCoolDownFinished()
    {
        return Time.time > _coolDownTime + _coolDownStart;
    }

    private void StartCoolDown()
    {
        _coolDownStart = Time.time;
        SoundManager.Instance.PlayShiftSound();
    }

    public void MoveHorizontal(int direction)
    {
        float extentToRight = 4;
        Vector3 localRight = new Vector3(1, 0, 0);
        //Debug.LogFormat("move horizontal with {0}", direction);
        if (CheckCoolDownFinished())
        {
            switch (_avatarPositionState)
            {
                case AvatarPositionState.Center:
                    if (direction > 0)
                    {
                        _avatarPositionState = AvatarPositionState.Right;
                        _targetPositionWrtCenter = extentToRight * localRight;
                        StartCoolDown();
                    }
                    else if (direction < 0)
                    {
                        _avatarPositionState = AvatarPositionState.Left;
                        _targetPositionWrtCenter = -extentToRight * localRight;
                        StartCoolDown();
                    }
                    break;

                case AvatarPositionState.Left:
                    if (direction > 0)
                    {
                        _avatarPositionState = AvatarPositionState.Center;
                        _targetPositionWrtCenter = Vector3.zero;
                        StartCoolDown();
                    }
                    break;

                case AvatarPositionState.Right:
                    if (direction < 0)
                    {
                        _avatarPositionState = AvatarPositionState.Center;
                        _targetPositionWrtCenter = Vector3.zero;
                        StartCoolDown();
                    }
                    break;
            }
        }
        //transform.position += amountToRight * _rightDirectionReference.right ;
    }

    void FixedUpdate()
    {
        //Debug.LogFormat("Target Position: {0}", _targetPositionWrtCenter);
        Vector3 targetPosition = _centerReference.TransformPoint(_targetPositionWrtCenter);
        Vector3 lerpedPosition = Vector3.Lerp(_rb.position, targetPosition, 10.0f * Time.deltaTime);
        transform.position = lerpedPosition;
    }

    public void Jump()
    {
        _unityChanController.ToJump = true;
    }

    public void UpdateForwardSpeed(float speed)
    {
        _unityChanController.ForwardSpeed = speed;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == GameController.ObstacleTag)
        {
            GameController.Instance.GameOver();
        }
    }
}
