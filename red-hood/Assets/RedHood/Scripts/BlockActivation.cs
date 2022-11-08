using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable), typeof(SoundActivation))]
// 현재 블록과 다음 블록을 실행시킨다.
public class BlockActivation : MonoBehaviour
{
    public Coroutine CurrentRoutine { private set; get; } = null;

    [Tooltip("블록의 실행 시간")]
    [SerializeField] private float ActiveDelay = 1.5f;

    private XRGrabInteractable block;
    private XRSocketInteractor socket;
    private SoundActivation soundActivation;
    private new ParticleSystem particleSystem;
    private IterationBlock iterBlock;
    private BranchBlock branchBlock;

    private const string ITER_END_TAG = "IterEndBlock";

    private void Start()
    {
        block = GetComponent<XRGrabInteractable>();
        socket = GetComponentInChildren<XRSocketInteractor>();
        soundActivation = GetComponent<SoundActivation>();
        particleSystem = GetComponentInChildren<ParticleSystem>();
        iterBlock = GetComponent<IterationBlock>();
        branchBlock = GetComponent<BranchBlock>();
    }

    // Active event 함수를 실행한다.
    private void ActivateBlock()
    {
        soundActivation.PlayActivatedSound();

        if (particleSystem != null)
        {
            particleSystem.Play();
        }

        ActivateEventArgs args = new()
        {
            interactableObject = block
        };
        block.activated.Invoke(args);
    }

    // 다음 연결된 블록을 가져온다.
    public BlockActivation GetNextBlock()
    {
        IXRSelectInteractable nextBlock = socket.firstInteractableSelected;
        return nextBlock == null ? null : ((XRGrabInteractable)nextBlock).GetComponent<BlockActivation>();
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
    }

    // 블록을 실행하고, 다음 블록을 트리거한다.
    public void ExecuteBlock()
    {
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

        // 다음 블록 트리거
        StopAllCoroutines();
        CurrentRoutine = StartCoroutine(ExecuteNextBlock());
    }
}
