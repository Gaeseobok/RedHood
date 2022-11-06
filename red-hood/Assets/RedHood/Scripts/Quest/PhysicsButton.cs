using UnityEngine;
using UnityEngine.Events;

// 버튼에 이벤트를 결합한다.
[RequireComponent(typeof(ConfigurableJoint))]
public class PhysicsButton : MonoBehaviour
{
    // 이벤트가 발생하는 역치 (버튼의 눌림 정도)
    [SerializeField] private float threshold = 0.1f;

    // 물리적 효과(bounciness)으로 인해 버튼이 여러 번 눌리는 것(pressed/released)을 방지하기 위한 구간
    [SerializeField] private float deadZone = 0.025f;

    // 매 프레임마다 이벤트를 트리거하지 않기 위해 버튼의 눌림 상태를 저장한다.
    private bool isPressed = false;

    private Vector3 startPos;
    private ConfigurableJoint joint;

    public UnityEvent OnPressed, OnReleased;

    private void Start()
    {
        startPos = transform.localPosition;
        joint = GetComponent<ConfigurableJoint>();
    }

    private void Update()
    {
        if (!isPressed && GetPressingIndex() + threshold >= 1)
        {
            Pressed();
        }

        if (isPressed && GetPressingIndex() - threshold <= 0)
        {
            Released();
        }
    }

    private void Pressed()
    {
        isPressed = true;
        OnPressed.Invoke();
    }

    private void Released()
    {
        isPressed = false;
        OnReleased.Invoke();
    }

    private float GetPressingIndex()
    {
        float pressingIdx = Vector3.Distance(startPos, transform.localPosition) / joint.linearLimit.limit;
        pressingIdx = Mathf.Abs(pressingIdx);

        if (pressingIdx < deadZone)
        {
            return 0.0f;
        }

        return Mathf.Clamp(pressingIdx, 0f, 1f);
    }
}