using System.Collections;
using UnityEngine;

// ��ũ�� ��ư�� ������ �� �ڵ� ������ ���� ����Ʈ���� ��ũ���Ѵ�.
public class SocketListScroll : MonoBehaviour
{
    public static Coroutine CurrentRoutine { private set; get; } = null;

    [Tooltip("���� ����Ʈ(���ϵ��� ���� ������Ʈ)")]
    [SerializeField] private SocketList socketList;

    [Tooltip("��ư�� ��� ������ ���� ��, ���� ��ũ�ѱ����� ���� �ð�")]
    [SerializeField] private float scrollDelay = 1.0f;

    // ���� ���̴� ù ��° ������ �ε���
    private static int firstVisibleIndex = 0;

    // �� ���� ���̴� ������ �ִ� ����
    private const int MAX_VISIBLE_SOCKETS = 7;

    // ���� ����Ʈ�� ������ �Ǵ� �߰� �ε���
    private const int MIDDLE_SOCKET_IDX = MAX_VISIBLE_SOCKETS / 2;

    // ���� �� ����
    private const float SOCKET_INTERVAL = 0.05f;

    private Vector3 direction = new(0.0f, SOCKET_INTERVAL, 0.0f);
    private Vector3 defaultPosition = new(0.0f, 0.0f, 0.01f);

    public void ResetFisrtVisibleIndex()
    {
        firstVisibleIndex = 0;
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

            yield return new WaitForSeconds(scrollDelay);
        }
    }

    // ��ũ���� �ʱ�ȭ�Ѵ�.
    internal void ResetScroll()
    {
        socketList.transform.localPosition = defaultPosition;

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
        ResetFisrtVisibleIndex();
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
        }
    }
}