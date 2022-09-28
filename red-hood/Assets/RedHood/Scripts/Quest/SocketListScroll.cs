using System.Collections;
using UnityEngine;
using static SocketInformation;

// 스크롤 버튼을 눌렀을 때 코딩 보드의 소켓 리스트들을 스크롤한다.
public class SocketListScroll : MonoBehaviour
{
    public static Coroutine CurrentRoutine { private set; get; } = null;

    [Tooltip("소켓 리스트")]
    [SerializeField] private SocketInformation socketList;

    [Tooltip("버튼을 계속 누르고 있을 때, 다음 스크롤까지의 지연 시간")]
    [SerializeField] private float scrollDelay = 1.0f;

    // 현재 보이는 첫 번째 소켓의 인덱스
    private static int firstVisibleIndex = 0;

    // 한 번에 보이는 소켓의 최대 개수
    private const int MAX_VISIBLE_SOCKETS = 7;

    // 소켓 리스트의 기준이 되는 중간 인덱스
    private const int MIDDLE_SOCKET_IDX = MAX_VISIBLE_SOCKETS / 2;

    // 소켓 간 간격
    private const float SOCKET_INTERVAL = 0.05f;

    private Vector3 direction = new(0.0f, SOCKET_INTERVAL, 0.0f);
    private Vector3 defaultPosition;

    private void Start()
    {
        defaultPosition = gameObject.transform.position;
    }

    // 소켓 리스트를 위로 스크롤하기 시작한다.
    public void StartScrollUp()
    {
        // 더이상 스크롤할 소켓이 없으면 스크롤하지 않는다.
        if (firstVisibleIndex == 0)
            return;

        StopAllCoroutines();
        CurrentRoutine = StartCoroutine(ScrollUp());
    }

    // 소켓 리스트를 아래로 스크롤하기 시작한다.
    public void StartScrollDown()
    {
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
            socketList.ActivateSocket(--firstVisibleIndex);
            socketList.InactivateSocket(firstVisibleIndex + MAX_VISIBLE_SOCKETS);
            socketList.transform.Translate(-direction);

            yield return new WaitForSeconds(scrollDelay);
        }
    }

    private IEnumerator ScrollDown()
    {
        while (firstVisibleIndex + MAX_VISIBLE_SOCKETS < socketNum)
        {
            socketList.ActivateSocket(firstVisibleIndex + MAX_VISIBLE_SOCKETS);
            socketList.InactivateSocket(firstVisibleIndex++);
            socketList.transform.Translate(direction);

            yield return new WaitForSeconds(scrollDelay);
        }
    }

    //// 소켓과 소켓에 부착된 블록을 활성화한다.
    //private void ActivateSocket(int index)
    //{
    //    socketInfos[index].SocketObject.SetActive(true);

    //    if (socketInfos[index].AttachedBlock != null)
    //    {
    //        XRSocketInteractor socketInteractor = socketInfos[index].SocketObject.GetComponentInChildren<XRSocketInteractor>();

    //        socketInfos[index].AttachedBlock.gameObject.SetActive(true);
    //        interactionManager.SelectEnter(socketInteractor, socketInfos[index].AttachedBlock);

    //        if (socketInfos[index].AttachedVarBlock != null)
    //        {
    //            socketInteractor = socketInfos[index].AttachedBlock.GetComponentInChildren<XRSocketInteractor>();

    //            socketInfos[index].AttachedVarBlock.gameObject.SetActive(true);
    //            interactionManager.SelectEnter(socketInteractor, socketInfos[index].AttachedVarBlock);
    //        }
    //    }
    //}

    //// 소켓과 소켓에 부착된 블록을 비활성화한다.
    //private void InactivateSocket(int index)
    //{
    //    // 부착된 블록들을 저장한 후, 비활성화
    //    IXRSelectInteractable interactableSelected = socketInfos[index].SocketObject
    //                                                 .GetComponentInChildren<XRSocketInteractor>().firstInteractableSelected;

    //    if (interactableSelected != null)
    //    {
    //        XRGrabInteractable attachedBlock = (XRGrabInteractable)interactableSelected;
    //        socketInfos[index].AttachedBlock = attachedBlock;

    //        XRGrabInteractable attachedVarBlock = GetAttachedVariableBlock(attachedBlock);
    //        if (attachedVarBlock != null)
    //        {
    //            socketInfos[index].AttachedVarBlock = attachedVarBlock;
    //            socketInfos[index].AttachedVarBlock.gameObject.SetActive(false);
    //        }

    //        socketInfos[index].AttachedBlock.gameObject.SetActive(false);
    //    }

    //    socketInfos[index].SocketObject.SetActive(false);
    //}

    // 스크롤을 초기화한다.
    //public void RevertScroll()
    //{
    //    sockets.socketList.position = defaultPosition;

    //    for (int i = 0; i < sockets.socketNum; i++)
    //    {
    //        if (i < MAX_VISIBLE_SOCKETS && !socketInfos[i].SocketObject.activeInHierarchy)
    //        {
    //            ActivateSocket(i);
    //        }
    //        else if (i >= MAX_VISIBLE_SOCKETS && socketInfos[i].SocketObject.activeInHierarchy)
    //        {
    //            InactivateSocket(i);
    //        }
    //    }
    //}

    // 인덱스에 맞는 위치로 스크롤
    private void SetScroll(int index)
    {
        //if (index <= MIDDLE_SOCKET_IDX)
        //{
        //    RevertScroll();
        //}
        //else if (socketNum - index <= MIDDLE_SOCKET_IDX)
        //{
        //    SetScroll(socketNum - MIDDLE_SOCKET_IDX - 1);
        //}
        //else
        //{
        //    int scrollNum = MIDDLE_SOCKET_IDX - index;

        //}

        //if (socketNum <= MAX_VISIBLE_SOCKETS)
        //    return;

        //RevertScroll();

        //int x = index - MIDDLE_SOCKET_IDX;
        //if (socketNum - x <= MIDDLE_SOCKET_IDX)
        //{
        //    x = socketNum - x - 1;
        //}
        //Debug.Log($"{x} 인덱스 기준으로 스크롤");
    }
}