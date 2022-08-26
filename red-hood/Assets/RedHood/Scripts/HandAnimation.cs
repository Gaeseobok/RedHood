using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �� ��Ʈ�ѷ��� �ִϸ��̼��� ����
[RequireComponent(typeof(Animator))]
public class HandAnimation : MonoBehaviour
{
    [SerializeField] private float animationSpeed;
    private Animator _animator;
    private float _gripTarget;
    private float _triggerTarget;
    private float _gripCurrent;
    private float _triggerCurrent;

    private const string ANIMATOR_PARAM_GRIP = "Grip";
    private const string ANIMATOR_PARAM_TRIGGER = "Trigger";

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        AnimateHand();
        Debug.Log("Grip: " + _animator.GetFloat(ANIMATOR_PARAM_GRIP));
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
}