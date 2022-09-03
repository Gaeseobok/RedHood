using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// 코딩 보드의 버튼을 눌렀을 때 블록을 초기화하거나 실행한다. (리셋 & 스타트)
public class OnBoardButtonPress : MonoBehaviour
{
    public Coroutine CurrentRoutine { private set; get; } = null;

    [Tooltip("소켓들의 상위 오브젝트")]
    [SerializeField] private GameObject socketsObject;

    [Tooltip("알림 메세지를 출력할 캔버스")]
    [SerializeField] private Canvas alertCanvas;

    [Tooltip("다음 코드 블록이 실행될 때까지의 지연 시간")]
    [SerializeField] private float delay = 2.0f;

    // 코드 블록이 위치하는 소켓들
    private XRSocketInteractor[] sockets;

    // 소켓들의 상태를 표현하는 오브젝트
    private ChangeMaterial[] pointers;

    // 상황 별 알림 메세지 출력을 위한 변수
    private FadeCanvas errorMessage;

    //private GameObject failureMessage;
    //private GameObject successMessage;

    private const string ERROR_MESSAGE = "Error Message";
    private const string FAILURE_MESSAGE = "Failure Message";
    private const string SUCCESS_MESSAGE = "Success Message";

    private void Start()
    {
        sockets = socketsObject.GetComponentsInChildren<XRSocketInteractor>();
        pointers = socketsObject.GetComponentsInChildren<ChangeMaterial>();

        errorMessage = alertCanvas.transform.Find(ERROR_MESSAGE).GetComponent<FadeCanvas>();
        //failureMessage = alertCanvas.transform.Find(FAILURE_MESSAGE).gameObject;
        //successMessage = alertCanvas.transform.Find(SUCCESS_MESSAGE).gameObject;
    }

    private List<XRGrabInteractable> GetAttachedBlockList()
    {
        // 소켓에 위치한 모든 블록을 리스트 형태로 리턴
        List<XRGrabInteractable> blockList = new();
        foreach (XRSocketInteractor socket in sockets)
        {
            List<IXRSelectInteractable> attachedBlocks = socket.interactablesSelected;
            if (attachedBlocks.Count != 0)
                blockList.Add((XRGrabInteractable)attachedBlocks[0]);
        }
        return blockList;
    }

    public void PressResetButton()
    {
        // 블록 리스트 가져오기
        List<XRGrabInteractable> blockList = GetAttachedBlockList();
        // 모든 블록 제거하기
        foreach (XRGrabInteractable block in blockList)
        {
            // 블록 내부 변수 소켓(Socket_Variable)의 변수 블록도 제거하기
            XRSocketInteractor variableSocket = block.GetComponentInChildren<XRSocketInteractor>();
            if (variableSocket != null)
            {
                List<IXRSelectInteractable> variableBlocks = variableSocket.interactablesSelected;
                if (variableBlocks.Count > 0)
                {
                    XRGrabInteractable variableBlock = (XRGrabInteractable)variableSocket.interactablesSelected[0];
                    Destroy(variableBlock.gameObject);
                }
            }
            Destroy(block.gameObject);
        }
    }

    private bool IsSocketEmpty(List<XRGrabInteractable> blockList)
    {
        // 블록 내부에 비어있는 변수 소켓(Socket_Variable)이 있다면 true 리턴
        foreach (XRGrabInteractable block in blockList)
        {
            XRSocketInteractor variableSocket = block.GetComponentInChildren<XRSocketInteractor>();
            if (variableSocket != null)
            {
                List<IXRSelectInteractable> variableBlocks = variableSocket.interactablesSelected;
                if (variableBlocks.Count == 0)
                    return true;
            }
        }
        return false;
    }

    private void ActivateBlock(XRGrabInteractable block)
    {
        // 블록의 Activated 이벤트를 활성화
        ActivateEventArgs args = new();
        args.interactableObject = block;
        block.activated.Invoke(args);
    }

    private IEnumerator ExecuteBlockCodes(List<XRGrabInteractable> blockList)
    {
        // 일정 간격으로 블록을 하나씩 실행
        for (int i = 0; i < blockList.Count; i++)
        {
            pointers[i].ChangeToActivatedMaterial();
            ActivateBlock(blockList[i]);
            yield return new WaitForSeconds(delay);
            pointers[i].ChangeToSelectedMaterial();
        }
    }

    public void PressStartButton()
    {
        // 블록 리스트 가져오기
        List<XRGrabInteractable> blockList = GetAttachedBlockList();

        // 모든 소켓에 블록이 모두 채워지지 않은 경우 알림 메세지 출력
        if (blockList.Count < sockets.Length || IsSocketEmpty(blockList))
        {
            errorMessage.SetAlpha(1.0f);
            errorMessage.StartFadeOut();
            return;
        }

        // 모든 블록 실행하기
        StopAllCoroutines();
        CurrentRoutine = StartCoroutine(ExecuteBlockCodes(blockList));
    }
}