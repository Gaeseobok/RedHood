using System.Collections;
using UnityEngine;
// 스크롤 버튼을 눌렀을 때 코딩 보드의 소켓 리스트들을 스크롤한다.
public class SocketListScroll : MonoBehaviour
{
    public static Coroutine CurrentRoutine { private set; get; } = null;

    [Tooltip("소켓 리스트(소켓들의 상위 오브젝트)")]
    [SerializeField] private SocketList socketList;

    [Tooltip("스크롤바 오브젝트")]
    [SerializeField] private Transform scrollBar;

    [Tooltip("버튼을 계속 누르고 있을 때, 다음 스크롤까지의 지연 시간")]
    [SerializeField] private float scrollDelay = 1.0f;

    // 현재 보이는 첫 번째 소켓의 인덱스
    private static int firstVisibleIndex = 0;

    // 한 번에 보이는 소켓의 최대 개수
    private const int MAX_VISIBLE_SOCKETS = 7;

    // 소켓 간 간격
    private const float SOCKET_INTERVAL = 0.05f;

    // 스크롤바 이동 범위
    private const float SCROLLBAR_RANGE = 0.28f;

    private Vector3 direction = new(0.0f, SOCKET_INTERVAL, 0.0f);
    private Vector3 defaultPosition;

    private Vector3 scrollBarDirection;
    private Vector3 scrollBarDefaultPosition;

    private void Start()
    {
        InitSocketListScroll();
    }

    public void InitSocketListScroll()
    {
        defaultPosition = socketList.transform.localPosition;
        scrollBarDefaultPosition = scrollBar.localPosition;
        firstVisibleIndex = 0;
        CalcScrollBarDirection();
    }

    private void CalcScrollBarDirection()
    {
        // 스크롤해야 하는 소켓의 개수
        float scrollSocketNum = socketList.socketNum - MAX_VISIBLE_SOCKETS;

        // 스크롤이 필요 없으면, 스크롤바 비활성화
        if (scrollSocketNum <= 0)
        {
            scrollBar.gameObject.SetActive(false);
            return;
        }

        scrollBarDirection = new(0.0f, -(SCROLLBAR_RANGE / scrollSocketNum), 0.0f);
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
        if (firstVisibleIndex + MAX_VISIBLE_SOCKETS >= socketList.socketNum)
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
            scrollBar.Translate(-scrollBarDirection);

            yield return new WaitForSeconds(scrollDelay);
        }
    }

    private IEnumerator ScrollDown()
    {
        while (firstVisibleIndex + MAX_VISIBLE_SOCKETS < socketList.socketNum)
        {
            socketList.ActivateSocket(firstVisibleIndex + MAX_VISIBLE_SOCKETS);
            socketList.InactivateSocket(firstVisibleIndex++);
            socketList.transform.Translate(direction);
            scrollBar.Translate(scrollBarDirection);

            yield return new WaitForSeconds(scrollDelay);
        }
    }

    // 스크롤을 초기화한다.
    internal void ResetScroll()
    {
        socketList.transform.localPosition = defaultPosition;
        scrollBar.transform.localPosition = scrollBarDefaultPosition;

        for (int i = 0; i < socketList.socketNum; i++)
        {
            if (i < MAX_VISIBLE_SOCKETS)
            {
                socketList.ActivateSocket(i);
            }
            else if (i >= MAX_VISIBLE_SOCKETS)
            {
                socketList.InactivateSocket(i);
            }
        }
        firstVisibleIndex = 0;
    }

    // 인덱스에 맞는 위치로 스크롤
    internal void SetScroll(int index)
    {
        int scrollNum = index - (MAX_VISIBLE_SOCKETS / 2);

        ResetScroll();
        while (firstVisibleIndex <= scrollNum && firstVisibleIndex + MAX_VISIBLE_SOCKETS < socketList.socketNum)
        {
            socketList.ActivateSocket(firstVisibleIndex + MAX_VISIBLE_SOCKETS);
            socketList.InactivateSocket(firstVisibleIndex++);
            socketList.transform.Translate(direction);
            scrollBar.Translate(scrollBarDirection);
        }
    }
}