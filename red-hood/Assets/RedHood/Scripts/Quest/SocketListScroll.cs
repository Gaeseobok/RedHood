using System.Collections;
using UnityEngine;

// ��ũ�� ��ư�� ������ �� �ڵ� ������ ���� ����Ʈ���� ��ũ���Ѵ�.
public class SocketListScroll : MonoBehaviour
{
    public static Coroutine CurrentRoutine { private set; get; } = null;

    [Tooltip("���� ����Ʈ(���ϵ��� ���� ������Ʈ)")]
    [SerializeField] private SocketList socketList;

    [Tooltip("��ũ�ѹ� ������Ʈ")]
    [SerializeField] private Transform scrollBar;

    [Tooltip("��ư�� ��� ������ ���� ��, ���� ��ũ�ѱ����� ���� �ð�")]
    [SerializeField] private float scrollDelay = 1.0f;

    // ���� ���̴� ù ��° ������ �ε���
    private static int firstVisibleIndex = 0;

    // �� ���� ���̴� ������ �ִ� ����
    private const int MAX_VISIBLE_SOCKETS = 7;

    // ���� �� ����
    private const float SOCKET_INTERVAL = 0.05f;

    // ��ũ�ѹ� �̵� ����
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
        // ��ũ���ؾ� �ϴ� ������ ����
        float scrollSocketNum = socketList.socketNum - MAX_VISIBLE_SOCKETS;

        // ��ũ���� �ʿ� ������, ��ũ�ѹ� ��Ȱ��ȭ
        if (scrollSocketNum <= 0)
        {
            scrollBar.gameObject.SetActive(false);
            return;
        }

        scrollBarDirection = new(0.0f, -(SCROLLBAR_RANGE / scrollSocketNum), 0.0f);
    }

    // ���� ����Ʈ�� ���� ��ũ���ϱ� �����Ѵ�.
    public void StartScrollUp()
    {
        // ���̻� ��ũ���� ������ ������ ��ũ������ �ʴ´�.
        if (firstVisibleIndex == 0)
            return;

        StopAllCoroutines();
        CurrentRoutine = StartCoroutine(ScrollUp());
    }

    // ���� ����Ʈ�� �Ʒ��� ��ũ���ϱ� �����Ѵ�.
    public void StartScrollDown()
    {
        // ���̻� ��ũ���� ������ ������ ��ũ������ �ʴ´�.
        if (firstVisibleIndex + MAX_VISIBLE_SOCKETS >= socketList.socketNum)
            return;

        StopAllCoroutines();
        CurrentRoutine = StartCoroutine(ScrollDown());
    }

    // ��ũ���� �����.
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

    // ��ũ���� �ʱ�ȭ�Ѵ�.
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

    // �ε����� �´� ��ġ�� ��ũ��
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