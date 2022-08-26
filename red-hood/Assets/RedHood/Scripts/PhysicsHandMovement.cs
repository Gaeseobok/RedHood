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
        // RIgidbody�� isKinematic�� False�̹Ƿ� Rigidbody.MovePosition ��� �Ұ�
        // ���� Rigidbody.velocity(angularVelocity)�� ������ ������Ʈ�� followTarget�� ���󰡵��� ��

        // Update position
        Vector3 targetPositionWithOffset = followTarget.position + positionOffset;
        _rigidbody.velocity = (targetPositionWithOffset - transform.position) / Time.fixedDeltaTime;

        // Update rotation
        // �� ������Ʈ ���� rotation ���̸� Quaternion ���·� ���
        Quaternion targetRotationWithOffset = followTarget.rotation * Quaternion.Euler(rotationOffset);
        Quaternion rotationDiff = targetRotationWithOffset * Quaternion.Inverse(transform.rotation);
        // ������ ������ ��ȯ
        rotationDiff.ToAngleAxis(out float angle, out Vector3 axis);
        Vector3 rotationDiffInDegree = angle * axis;
        _rigidbody.angularVelocity = (rotationDiffInDegree * Mathf.Deg2Rad) / Time.fixedDeltaTime;
    }
}