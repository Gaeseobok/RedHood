using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// 현재 블록과 다음 블록을 실행시킨다.
public class BlockActivation : MonoBehaviour
{
    public Coroutine CurrentRoutine { private set; get; } = null;

    [Tooltip("블록의 실행 시간")]
    [SerializeField] private float ActiveDelay = 1.5f;

    private XRGrabInteractable block;
    private XRSocketInteractor socket;

    private void Start()
    {
        block = GetComponent<XRGrabInteractable>();
        socket = GetComponentInChildren<XRSocketInteractor>();
    }

    // Active event 함수를 실행한다.
    private void ActivateBlock()
    {
        ActivateEventArgs args = new()
        {
            interactableObject = block
        };
        block.activated.Invoke(args);
    }

    // 다음 연결된 블록을 가져온다.
    private XRGrabInteractable GetNextBlock()
    {
        IXRSelectInteractable nextBlock = socket.firstInteractableSelected;
        return nextBlock == null ? null : (XRGrabInteractable)nextBlock;
    }

    // 현재 블록의 색상을 변경한다. (실행되고 있을 때)
    private void SetColorActive()
    {
        // TODO: 블록 실행 이펙트 추가
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

    // 블록을 실행하고, 다음 블록을 트리거한다.
    public void ExecuteBlock()
    {
        SetColorActive();
        ActivateBlock();

        StopAllCoroutines();
        CurrentRoutine = StartCoroutine(ExecuteNextBlock());
    }
}
