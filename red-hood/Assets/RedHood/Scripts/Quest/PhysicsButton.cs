using UnityEngine;
using UnityEngine.Events;

// ��ư�� �̺�Ʈ�� �����Ѵ�.
[RequireComponent(typeof(ConfigurableJoint))]
public class PhysicsButton : MonoBehaviour
{
    // �̺�Ʈ�� �߻��ϴ� ��ġ (��ư�� ���� ����)
    [SerializeField] private float threshold = 0.1f;

    // ������ ȿ��(bounciness)���� ���� ��ư�� ���� �� ������ ��(pressed/released)�� �����ϱ� ���� ����
    [SerializeField] private float deadZone = 0.025f;

    // �� �����Ӹ��� �̺�Ʈ�� Ʈ�������� �ʱ� ���� ��ư�� ���� ���¸� �����Ѵ�.
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