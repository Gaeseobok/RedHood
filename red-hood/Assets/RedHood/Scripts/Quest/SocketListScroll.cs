using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// ��ũ�� ��ư�� ������ �� �ڵ� ������ ���� ����Ʈ���� ��ũ���Ѵ�.
public class SocketListScroll : AttachedBlock
{
    public static Coroutine CurrentRoutine { private set; get; } = null;

    [Tooltip("��ư�� ��� ������ ���� ��, ���� ��ũ�ѱ����� ���� �ð�")]
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

    // ������ ����
    private int socketNum;

    // ���ϵ��� �����ϴ� ����Ʈ
    private static SocketWIthBlock[] sockets;

    // ���� ���̴� ù ��° ������ �ε���
    private static int firstVisibleIndex = 0;

    // �� ���� ���̴� ������ �ִ� ����
    private const int MAX_VISIBLE_SOCKETS = 7;

    // ���� �� ����
    private const float SOCKET_INTERVAL = 0.05f;

    // ���� ����Ʈ�� �̵��� ����
    private Vector3 direction = new(0.0f, SOCKET_INTERVAL, 0.0f);

    private void Start()
    {
        socketNum = socketList.childCount;
        sockets = new SocketWIthBlock[socketNum];

        for (int i = 0; i < socketNum; i++)
            sockets[i] = new SocketWIthBlock(socketList.GetChild(i).gameObject);
    }

    // ���� ����Ʈ�� ���� ��ũ���ϱ� �����Ѵ�.
    public void StartScrollUp()
    {
        Debug.Log($"{firstVisibleIndex}. �� ��ư ����");
        // ���̻� ��ũ���� ������ ������ ��ũ������ �ʴ´�.
        if (firstVisibleIndex == 0)
            return;

        StopAllCoroutines();
        CurrentRoutine = StartCoroutine(ScrollUp());
    }

    // ���� ����Ʈ�� �Ʒ��� ��ũ���ϱ� �����Ѵ�.
    public void StartScrollDown()
    {
        Debug.Log($"{firstVisibleIndex}. �ٿ� ��ư ����");
        // ���̻� ��ũ���� ������ ������ ��ũ������ �ʴ´�.
        if (firstVisibleIndex + MAX_VISIBLE_SOCKETS >= socketNum)
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

    // ���ϰ� ���Ͽ� ������ ����� Ȱ��ȭ�Ѵ�.
    private void ActivateSocket(int index)
    {
        if (sockets[index].AttachedBlock != null)
        {
            Debug.Log($"{index}. {sockets[index].AttachedBlock} Ȱ��ȭ");
            sockets[index].AttachedBlock.SetActive(true);
            if (sockets[index].AttachedVarBlock != null)
            {
                Debug.Log($"{index}. {sockets[index].AttachedVarBlock} Ȱ��ȭ");
                sockets[index].AttachedVarBlock.SetActive(true);
            }
        }
        Debug.Log($"{index}. {sockets[index].SocketObject} Ȱ��ȭ");
        sockets[index].SocketObject.SetActive(true);
    }

    // ���ϰ� ���Ͽ� ������ ����� ��Ȱ��ȭ�Ѵ�.
    private void InactivateSocket(int index)
    {
        // ������ ��ϵ��� ������ ��, ��Ȱ��ȭ
        XRGrabInteractable attachedBlock = GetAttachedBlock(sockets[index].SocketObject.GetComponentInChildren<XRSocketInteractor>());
        if (attachedBlock != null)
        {
            sockets[index].AttachedBlock = attachedBlock.gameObject;
            XRGrabInteractable attachedVarBlock = GetAttachedVariableBlock(attachedBlock);
            if (attachedVarBlock != null)
            {
                sockets[index].AttachedVarBlock = attachedVarBlock.gameObject;
                sockets[index].AttachedVarBlock.SetActive(false);

                Debug.Log($"{index}. {sockets[index].AttachedVarBlock} ��Ȱ��ȭ");
            }

            Debug.Log($"{index}. {sockets[index].AttachedBlock} ��Ȱ��ȭ");
            sockets[index].AttachedBlock.SetActive(false);
        }

        Debug.Log($"{index}. {sockets[index].SocketObject} ��Ȱ��ȭ");
        sockets[index].SocketObject.SetActive(false);
    }
}