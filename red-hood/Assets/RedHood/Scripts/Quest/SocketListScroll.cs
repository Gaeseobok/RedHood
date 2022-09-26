using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// 스크롤 버튼을 눌렀을 때 코딩 보드의 소켓 리스트들을 스크롤한다.
[System.Obsolete]
public class SocketListScroll : AttachedBlock
{
    public static Coroutine CurrentRoutine { private set; get; } = null;

    [Tooltip("버튼을 계속 누르고 있을 때, 다음 스크롤까지의 지연 시간")]
    [SerializeField] private float scrollDelay = 1.0f;

    private XRInteractionManager interactionManager;

    // 소켓 오브젝트와 부착된 블록들을 저장하는 구조체
    private struct SocketInfo
    {
        public GameObject SocketObject;
        public XRGrabInteractable AttachedBlock;
        public XRGrabInteractable AttachedVarBlock;

        public SocketInfo(GameObject socketObject)
        {
            this.SocketObject = socketObject;
            AttachedBlock = null;
            AttachedVarBlock = null;
        }
    }

    // 소켓의 개수
    private int socketNum;

    // 소켓과 부착된 블록들을 저장하는 리스트
    private static SocketInfo[] socketInfos;

    // 현재 보이는 첫 번째 소켓의 인덱스
    private static int firstVisibleIndex = 0;

    // 한 번에 보이는 소켓의 최대 개수
    private const int MAX_VISIBLE_SOCKETS = 7;

    // 소켓 간 간격
    private const float SOCKET_INTERVAL = 0.05f;

    // 소켓 리스트가 이동할 방향
    private Vector3 direction = new(0.0f, SOCKET_INTERVAL, 0.0f);

    private void Start()
    {
        interactionManager = gameObject.AddComponent<XRInteractionManager>();
        socketNum = socketList.childCount;
        socketInfos = new SocketInfo[socketNum];

        for (int i = 0; i < socketNum; i++)
            socketInfos[i] = new SocketInfo(socketList.GetChild(i).gameObject);
    }

    // 소켓 리스트를 위로 스크롤하기 시작한다.
    public void StartScrollUp()
    {
        Debug.Log($"{firstVisibleIndex}. 업 버튼 눌림");
        // 더이상 스크롤할 소켓이 없으면 스크롤하지 않는다.
        if (firstVisibleIndex == 0)
            return;

        StopAllCoroutines();
        CurrentRoutine = StartCoroutine(ScrollUp());
    }

    // 소켓 리스트를 아래로 스크롤하기 시작한다.
    public void StartScrollDown()
    {
        Debug.Log($"{firstVisibleIndex}. 다운 버튼 눌림");
        // 더이상 스크롤할 소켓이 없으면 스크롤하지 않는다.
        if (firstVisibleIndex + MAX_VISIBLE_SOCKETS >= socketNum)
            return;

        StopAllCoroutines();
        CurrentRoutine = StartCoroutine(ScrollDown());
    }

    // 스크롤을 멈춘다.
    public void StopScroll()
    {
        StopAllCoroutines();
    }

    private IEnumerator ScrollUp()
    {
        while (firstVisibleIndex > 0)
        {
            ActivateSocket(--firstVisibleIndex);
            InactivateSocket(firstVisibleIndex + MAX_VISIBLE_SOCKETS);
            socketList.Translate(-direction);

            yield return new WaitForSeconds(scrollDelay);
        }
    }

    private IEnumerator ScrollDown()
    {
        while (firstVisibleIndex + MAX_VISIBLE_SOCKETS < socketNum)
        {
            ActivateSocket(firstVisibleIndex + MAX_VISIBLE_SOCKETS);
            InactivateSocket(firstVisibleIndex++);
            socketList.Translate(direction);

            yield return new WaitForSeconds(scrollDelay);
        }
    }

    // 소켓과 소켓에 부착된 블록을 활성화한다.
    private void ActivateSocket(int index)
    {
        socketInfos[index].SocketObject.SetActive(true);

        if (socketInfos[index].AttachedBlock != null)
        {
            XRSocketInteractor socketInteractor = socketInfos[index].SocketObject.GetComponentInChildren<XRSocketInteractor>();

            socketInfos[index].AttachedBlock.gameObject.SetActive(true);
            interactionManager.SelectEnter(socketInteractor, socketInfos[index].AttachedBlock);

            if (socketInfos[index].AttachedVarBlock != null)
            {
                socketInteractor = socketInfos[index].AttachedBlock.GetComponentInChildren<XRSocketInteractor>();

                socketInfos[index].AttachedVarBlock.gameObject.SetActive(true);
                interactionManager.SelectEnter(socketInteractor, socketInfos[index].AttachedVarBlock);
            }
        }
    }

    // 소켓과 소켓에 부착된 블록을 비활성화한다.
    private void InactivateSocket(int index)
    {
        // 부착된 블록들을 저장한 후, 비활성화
        IXRSelectInteractable interactableSelected = socketInfos[index].SocketObject
                                                     .GetComponentInChildren<XRSocketInteractor>().firstInteractableSelected;

        if (interactableSelected != null)
        {
            XRGrabInteractable attachedBlock = (XRGrabInteractable)interactableSelected;
            socketInfos[index].AttachedBlock = attachedBlock;

            XRGrabInteractable attachedVarBlock = GetAttachedVariableBlock(attachedBlock);
            if (attachedVarBlock != null)
            {
                socketInfos[index].AttachedVarBlock = attachedVarBlock;
                socketInfos[index].AttachedVarBlock.gameObject.SetActive(false);
            }

            socketInfos[index].AttachedBlock.gameObject.SetActive(false);
        }

        socketInfos[index].SocketObject.SetActive(false);
    }
}