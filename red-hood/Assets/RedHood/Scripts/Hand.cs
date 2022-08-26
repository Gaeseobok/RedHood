using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace RedHood
{
    // Hand class handles anything that the hand can do
    [RequireComponent(typeof(Animator))]
    public class Hand : MonoBehaviour
    {
        [SerializeField] private float animationSpeed;
        private Animator _animator;
        private float _gripTarget;
        private float _triggerTarget;
        private float _gripCurrent;
        private float _triggerCurrent;

        private const string ANIMATOR_PARAM_GRIP = "Grip";
        private const string ANIMATOR_PARAM_TRIGGER = "Trigger";

        [SerializeField] private GameObject followObject;
        [SerializeField] private float followSpeed = 30f;
        [SerializeField] private float rotateSpeed = 100f;
        [SerializeField] private Vector3 positionOffset;
        [SerializeField] private Vector3 rotationOffset;
        private Transform _followTarget;
        private Rigidbody _rigidbody;

        private void Start()
        {
            _animator = GetComponent<Animator>();

            _followTarget = followObject.transform;
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            _rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            _rigidbody.mass = 20f;

            _rigidbody.position = _followTarget.position;
            _rigidbody.rotation = _followTarget.rotation;
        }

        private void Update()
        {
            AnimateHand();
            PhysicsMove();
        }

        internal void SetGrip(float v)
        {
            _gripTarget = v;
        }

        internal void SetTrigger(float v)
        {
            _triggerTarget = v;
        }

        // ���� �����ӿ� ���� ���������� Animator parameters�� ��ȭ���� Animation�� Ʈ�����Ѵ�.
        private void AnimateHand()
        {
            if (_gripCurrent != _gripTarget)
            {
                _gripCurrent = Mathf.MoveTowards(_gripCurrent, _gripTarget, Time.deltaTime * animationSpeed);
                _animator.SetFloat(ANIMATOR_PARAM_GRIP, _gripCurrent);
            }
            if (_triggerCurrent != _triggerTarget)
            {
                _triggerCurrent = Mathf.MoveTowards(_triggerCurrent, _triggerTarget, Time.deltaTime * animationSpeed);
                _animator.SetFloat(ANIMATOR_PARAM_TRIGGER, _triggerCurrent);
            }
        }

        private void PhysicsMove()
        {
            // Update position
            Vector3 targetPositionWithOffset = _followTarget.position + positionOffset;
            // �� ������Ʈ ���� �Ÿ� ���
            float dist = Vector3.Distance(targetPositionWithOffset, transform.position);
            // �� ������Ʈ ���� �Ÿ�, �ӷ�, ���⿡ ���� rigidbody�� �ӵ� ����
            _rigidbody.velocity = dist * followSpeed * (targetPositionWithOffset - transform.position).normalized;

            // Update rotation
            Quaternion targetRotationWithOffset = _followTarget.rotation * Quaternion.Euler(rotationOffset);
            // �� ������Ʈ ���� rotation ���̸� Quaternion ���·� ���
            Quaternion q = targetRotationWithOffset * Quaternion.Inverse(transform.rotation);
            // ������ ������ ��ȯ
            q.ToAngleAxis(out float angle, out Vector3 axis);
            _rigidbody.angularVelocity = angle * Mathf.Deg2Rad * rotateSpeed * axis;
        }
    }
}