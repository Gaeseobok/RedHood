using System.Collections;
using System.Collections.Generic;
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
    private IterationBlock iterBlock;
    private BranchBlock branchBlock;

    private const string ITER_END_TAG = "IterEndBlock";

    private void Start()
    {
        block = GetComponent<XRGrabInteractable>();
        socket = GetComponentInChildren<XRSocketInteractor>();
        iterBlock = GetComponent<IterationBlock>();
        branchBlock = GetComponent<BranchBlock>();
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
    public BlockActivation GetNextBlock()
    {
        IXRSelectInteractable nextBlock = socket.firstInteractableSelected;
        return nextBlock == null ? null : ((XRGrabInteractable)nextBlock).GetComponent<BlockActivation>();
    }

    // ���� ����� ������ �����Ѵ�. (����ǰ� ���� ��)
    private void SetColorActive()
    {
        // TODO: ��� ���� ����Ʈ �߰�
        return;
    }

    private IEnumerator ExecuteNextBlock()
    {
        ActivateBlock();

        yield return new WaitForSeconds(ActiveDelay);

        BlockActivation nextBlock = GetNextBlock();
        if (nextBlock != null)
        {
            nextBlock.ExecuteBlock();
        }

        SetColorActive();
    }

    // ����� �����ϰ�, ���� ����� Ʈ�����Ѵ�.
    public void ExecuteBlock()
    {
        SetColorActive();

        if (iterBlock != null)
        {
            iterBlock.SetIteration();
            if (CompareTag(ITER_END_TAG))
            {
                ActivateBlock();
                return;
            }
        }
        if (branchBlock != null)
        {
            ActivateBlock();
            return;
        }

        // ���� ��� Ʈ����
        StopAllCoroutines();
        CurrentRoutine = StartCoroutine(ExecuteNextBlock());
    }
}
