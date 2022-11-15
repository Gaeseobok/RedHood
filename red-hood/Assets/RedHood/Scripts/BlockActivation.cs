using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable), typeof(SoundActivation), typeof(PopUpMessage))]
// 현재 블록과 다음 블록을 실행시킨다.
public class BlockActivation : MonoBehaviour
{
    public Coroutine CurrentRoutine { private set; get; } = null;

    [Tooltip("블록의 실행 시간")]
    public float ActiveDelay = 1.5f;

    private XRGrabInteractable block;
    private XRSocketInteractor socket;
    private SoundActivation soundActivation;
    private PopUpMessage popUpMessage;
    private new ParticleSystem particleSystem;
    private IterationBlock iterBlock;
    private BranchBlock branchBlock;

    private const string ITER_END_TAG = "IterEndBlock";

    private const string TEST_SCENE = "BoardTestScene";
    private const string HOME_SCENE = "CottageScene";
    private const string FOREST_SCENE = "ForestScene";

    private void Start()
    {
        block = GetComponent<XRGrabInteractable>();
        socket = GetComponentInChildren<XRSocketInteractor>();
        soundActivation = GetComponent<SoundActivation>();
        popUpMessage = GetComponent<PopUpMessage>();
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

        if (!popUpMessage.isActivated())
        {
            yield return new WaitForSeconds(ActiveDelay);

            BlockActivation nextBlock = GetNextBlock();
            if (nextBlock != null)
            {
                nextBlock.ExecuteBlock();
            }
            else
            {
                ConfirmCodes();
            }
        }
    }

    // 블록을 실행하고, 다음 블록을 트리거한다.
    public void ExecuteBlock()
    {
        if (iterBlock != null)
        {
            bool isValid = iterBlock.SetIteration();
            if (CompareTag(ITER_END_TAG))
            {
                if (isValid)
                {
                    ActivateBlock();
                }
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

    // 
    private void ConfirmCodes()
    {
        if (gameObject.scene.name.Equals(HOME_SCENE))
        {
            SandwichMission component = gameObject.AddComponent<SandwichMission>();
            component.CheckAnswer();
            Destroy(component);
        }
    }
}
