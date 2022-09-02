using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// 코딩 보드의 버튼을 눌렀을 때 블록을 초기화하거나 실행한다. (리셋 & 스타트)
public class OnBoardButtonPress : MonoBehaviour
{
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
    private GameObject errorMessage;

    private GameObject failureMessage;
    private GameObject successMessage;

    private const string ERROR_MESSAGE = "Error Message";
    private const string FAILURE_MESSAGE = "Failure Message";
    private const string SUCCESS_MESSAGE = "Success Message";

    private void Start()
    {
        sockets = socketsObject.GetComponentsInChildren<XRSocketInteractor>();
        pointers = socketsObject.GetComponentsInChildren<ChangeMaterial>();

        errorMessage = alertCanvas.transform.Find(ERROR_MESSAGE).gameObject;
        failureMessage = alertCanvas.transform.Find(FAILURE_MESSAGE).gameObject;
        successMessage = alertCanvas.transform.Find(SUCCESS_MESSAGE).gameObject;
    }

    private List<XRGrabInteractable> GetAttachedBlockList()
    {
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
            Destroy(block.gameObject);
        }
    }

    public void PressStartButton()
    {
        // 블록 리스트 가져오기
        List<XRGrabInteractable> blockList = GetAttachedBlockList();

        // 코딩 보드의 빈칸에 블록이 모두 채워지지 않은 경우 알림 메세지 출력
        if (blockList.Count < sockets.Length)
        {
            Debug.Log("빈칸을 모두 채워주세요!");
            errorMessage.GetComponent<CanvasGroup>().alpha = 1.0f;
            errorMessage.GetComponent<FadeCanvas>().StartFadeOut();
            return;
        }

        // 블록 하나씩 activate 하기

        foreach (XRGrabInteractable block in blockList)
        {
            ActivateEventArgs args = new();
            args.interactableObject = block;
            block.activated.Invoke(args);
        }
    }
}