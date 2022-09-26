using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// 스크롤 버튼을 눌렀을 때 코딩 보드의 소켓 리스트들을 스크롤한다.
public class SocketListScroll : AttachedBlock
{
    public static Coroutine CurrentRoutine { private set; get; } = null;

    [Tooltip("버튼을 계속 누르고 있을 때, 다음 스크롤까지의 지연 시간")]
    [SerializeField] private float scrollDelay = 1.0f;

    private struct SocketWIthBlock
    {
        public GameObject SocketObject;
        public GameObject AttachedBlock;
        public GameObject AttachedVarBlock;

        public SocketWIthBlock(GameObject socketObject)
        {
            this.SocketObject = socketObject;
            AttachedBlock = null;
            AttachedVarBlock = null;
        }
    }

    // 소켓의 개수
    private int socketNum;

    // 소켓들을 저장하는 리스트
    private static SocketWIthBlock[] sockets;

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
        socketNum = socketList.childCount;
        sockets = new SocketWIthBlock[socketNum];

        for (int i = 0; i < socketNum; i++)
            sockets[i] = new SocketWIthBlock(socketList.GetChild(i).gameObject);
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
        if (sockets[index].AttachedBlock != null)
        {
            Debug.Log($"{index}. {sockets[index].AttachedBlock} 활성화");
            sockets[index].AttachedBlock.SetActive(true);
            if (sockets[index].AttachedVarBlock != null)
            {
                Debug.Log($"{index}. {sockets[index].AttachedVarBlock} 활성화");
                sockets[index].AttachedVarBlock.SetActive(true);
            }
        }
        Debug.Log($"{index}. {sockets[index].SocketObject} 활성화");
        sockets[index].SocketObject.SetActive(true);
    }

    // 소켓과 소켓에 부착된 블록을 비활성화한다.
    private void InactivateSocket(int index)
    {
        // 부착된 블록들을 저장한 후, 비활성화
        XRGrabInteractable attachedBlock = GetAttachedBlock(sockets[index].SocketObject.GetComponentInChildren<XRSocketInteractor>());
        if (attachedBlock != null)
        {
            sockets[index].AttachedBlock = attachedBlock.gameObject;
            XRGrabInteractable attachedVarBlock = GetAttachedVariableBlock(attachedBlock);
            if (attachedVarBlock != null)
            {
                sockets[index].AttachedVarBlock = attachedVarBlock.gameObject;
                sockets[index].AttachedVarBlock.SetActive(false);

                Debug.Log($"{index}. {sockets[index].AttachedVarBlock} 비활성화");
            }

            Debug.Log($"{index}. {sockets[index].AttachedBlock} 비활성화");
            sockets[index].AttachedBlock.SetActive(false);
        }

        Debug.Log($"{index}. {sockets[index].SocketObject} 비활성화");
        sockets[index].SocketObject.SetActive(false);
    }
}