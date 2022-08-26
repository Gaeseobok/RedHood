using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 손 컨트롤러에 애니메이션을 결합
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

    // 여러 프레임에 걸쳐 점진적으로 Animator parameters를 변화시켜 Animation을 트리거한다.
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