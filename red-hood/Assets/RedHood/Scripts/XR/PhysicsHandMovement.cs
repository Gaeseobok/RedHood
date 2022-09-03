using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Hand ���� ��Ʈ�ѷ��� �����ӿ� ���� ���������� �����̰� �ϴ� ��ũ��Ʈ
public class PhysicsHandMovement : MonoBehaviour
{
    // Physics hand movement

    [SerializeField] private Transform followTarget;
    [SerializeField] private float followSpeed = 30f;
    [SerializeField] private float rotateSpeed = 100f;
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private Vector3 rotationOffset;
    private Rigidbody _rigidbody;
    private Collider[] _handColliders;
    private SkinnedMeshRenderer _handRenderer;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.position = followTarget.position;
        _rigidbody.rotation = followTarget.rotation;
        _rigidbody.maxAngularVelocity = float.PositiveInfinity;

        _handColliders = GetComponentsInChildren<Collider>();
        _handRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
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
        float dist = Vector3.Distance(targetPositionWithOffset, transform.position);
        _rigidbody.velocity = dist * followSpeed * (targetPositionWithOffset - transform.position).normalized;

        // Update rotation
        // �� ������Ʈ ���� rotation ���̸� Quaternion ���·� ���
        Quaternion targetRotationWithOffset = followTarget.rotation * Quaternion.Euler(rotationOffset);
        Quaternion rotationDiff = targetRotationWithOffset * Quaternion.Inverse(transform.rotation);
        // ������ ������ ��ȯ
        rotationDiff.ToAngleAxis(out float angle, out Vector3 axis);
        if (Mathf.Abs(axis.magnitude) != Mathf.Infinity)
        {
            if (angle > 180.0f)
                angle -= 360.0f;

            Vector3 rotationDiffInDegree = angle * axis;
            _rigidbody.angularVelocity = Mathf.Deg2Rad * rotateSpeed * rotationDiffInDegree;
        }
    }

    private void EnableHandColliders()
    {
        foreach (Collider c in _handColliders)
        {
            c.enabled = true;
        }
    }

    public void EnableHandWithDelay(float delay)
    {
        _handRenderer.enabled = true;
        Invoke("EnableHandColliders", delay);
    }

    public void DisableHand()
    {
        _handRenderer.enabled = false;
        foreach (Collider c in _handColliders)
        {
            c.enabled = false;
        }
    }
}