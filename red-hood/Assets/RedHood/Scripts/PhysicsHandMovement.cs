using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsHandMovement : MonoBehaviour
{
    //[SerializeField] private float followSpeed = 30f;
    //[SerializeField] private float rotateSpeed = 100f;

    [SerializeField] private Transform followTarget;
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private Vector3 rotationOffset;
    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _rigidbody.position = followTarget.position;
        _rigidbody.rotation = followTarget.rotation;
    }

    private void FixedUpdate()
    {
        PhysicsMove();
    }

    private void PhysicsMove()
    {
        // RIgidbody의 isKinematic이 False이므로 Rigidbody.MovePosition 사용 불가
        // 따라서 Rigidbody.velocity(angularVelocity)를 조작해 오브젝트가 followTarget을 따라가도록 함

        // Update position
        Vector3 targetPositionWithOffset = followTarget.position + positionOffset;
        _rigidbody.velocity = (targetPositionWithOffset - transform.position) / Time.fixedDeltaTime;

        // Update rotation
        // 두 오브젝트 간의 rotation 차이를 Quaternion 형태로 계산
        Quaternion targetRotationWithOffset = followTarget.rotation * Quaternion.Euler(rotationOffset);
        Quaternion rotationDiff = targetRotationWithOffset * Quaternion.Inverse(transform.rotation);
        // 각도와 축으로 변환
        rotationDiff.ToAngleAxis(out float angle, out Vector3 axis);
        Vector3 rotationDiffInDegree = angle * axis;
        _rigidbody.angularVelocity = (rotationDiffInDegree * Mathf.Deg2Rad) / Time.fixedDeltaTime;
    }
}