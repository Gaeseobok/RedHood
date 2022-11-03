using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// ���� ��ϰ� ���� ����� �����Ų��.
public class BlockActivation : MonoBehaviour
{
    public Coroutine CurrentRoutine { private set; get; } = null;

    [Tooltip("����� ���� �ð�")]
    [SerializeField] private float ActiveDelay = 1.5f;

    private XRGrabInteractable block;
    private XRSocketInteractor socket;

    private void Start()
    {
        block = GetComponent<XRGrabInteractable>();
        socket = GetComponentInChildren<XRSocketInteractor>();
    }

    // Active event �Լ��� �����Ѵ�.
    private void ActivateBlock()
    {
        ActivateEventArgs args = new()
        {
            interactableObject = block
        };
        block.activated.Invoke(args);
    }

    // ���� ����� ����� �����´�.
    private XRGrabInteractable GetNextBlock()
    {
        IXRSelectInteractable nextBlock = socket.firstInteractableSelected;
        return nextBlock == null ? null : (XRGrabInteractable)nextBlock;
    }

    // ���� ����� ������ �����Ѵ�. (����ǰ� ���� ��)
    private void SetColorActive()
    {
        // TODO: ��� ���� ����Ʈ �߰�
        return;
    }

    private IEnumerator ExecuteNextBlock()
    {
        yield return new WaitForSeconds(ActiveDelay);
        XRGrabInteractable nextBlock = GetNextBlock();
        if (nextBlock != null)
        {
            nextBlock.GetComponent<BlockActivation>().ExecuteBlock();
        }
        SetColorActive();
    }

    // ����� �����ϰ�, ���� ����� Ʈ�����Ѵ�.
    public void ExecuteBlock()
    {
        SetColorActive();
        ActivateBlock();

        StopAllCoroutines();
        CurrentRoutine = StartCoroutine(ExecuteNextBlock());
    }
}
