using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// 스크롤 버튼을 눌렀을 때 코딩 보드의 소켓 리스트들을 스크롤한다.
public class SocketListScroll : MonoBehaviour
{
    public static Coroutine CurrentRoutine { private set; get; } = null;

    [Tooltip("소켓들의 상위 오브젝트(소켓 리스트)")]
    [SerializeField] private Transform socketList;

    [Tooltip("버튼을 계속 누르고 있을 때, 다음 스크롤까지의 지연 시간")]
    [SerializeField] private float scrollDelay = 1.0f;

    private struct Socket
    {
        public GameObject SocketObject;
        public GameObject AttachedBlock;
        public GameObject AttachedVarBlock;

        public Socket(GameObject socketObject)
        {
            this.SocketObject = socketObject;
            AttachedBlock = null;
            AttachedVarBlock = null;
        }
    }

    // 소켓들을 저장하는 리스트
    private Socket[] sockets;

    // 소켓의 개수
    private int socketNum;

    // 소켓에 부착된 블록을 가져오기 위한 컴포넌트
    private BlockExecution blockExecution;

    // 현재 보이는 첫 번째 소켓의 인덱스
    private static int firstVisibleIndex = 0;

    // 한 번에 보이는 소켓의 최대 개수
    private const int MAX_VISIBLE_SOCKETS = 7;

    // 소켓 간 간격
    private const float SOCKET_INTERVAL = 0.05f;

    private void Start()
    {
        blockExecution = gameObject.GetComponent<BlockExecution>();
        socketNum = socketList.childCount;
        sockets = new Socket[socketNum];

        for (int i = 0; i < socketNum; i++)
            sockets[i] = new Socket(socketList.GetChild(i).gameObject);
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
        Vector3 direction = new Vector3(0, -SOCKET_INTERVAL, 0);

        while (firstVisibleIndex > 0)
        {
            ActivateSocket(--firstVisibleIndex);
            InactivateSocket(firstVisibleIndex + MAX_VISIBLE_SOCKETS);
            socketList.Translate(direction);

            yield return new WaitForSeconds(scrollDelay);
        }
    }

    private IEnumerator ScrollDown()
    {
        Vector3 direction = new Vector3(0, SOCKET_INTERVAL, 0);

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
            sockets[index].AttachedBlock.SetActive(true);
            if (sockets[index].AttachedVarBlock != null)
                sockets[index].AttachedVarBlock.SetActive(true);
        }
        sockets[index].SocketObject.SetActive(true);
    }

    // 소켓과 소켓에 부착된 블록을 비활성화한다.
    private void InactivateSocket(int index)
    {
        // 부착된 블록들을 저장한 후, 비활성화
        XRGrabInteractable attachedBlock = blockExecution.GetAttachedBlock(sockets[index].SocketObject.GetComponentInChildren<XRSocketInteractor>());
        if (attachedBlock != null)
        {
            sockets[index].AttachedBlock = attachedBlock.gameObject;
            XRGrabInteractable attachedVarBlock = blockExecution.GetAttachedVariableBlock(attachedBlock);
            if (attachedVarBlock != null)
            {
                sockets[index].AttachedVarBlock = attachedVarBlock.gameObject;
                sockets[index].AttachedVarBlock.SetActive(false);
            }
            sockets[index].AttachedBlock.SetActive(false);
        }

        sockets[index].SocketObject.SetActive(false);
    }
}