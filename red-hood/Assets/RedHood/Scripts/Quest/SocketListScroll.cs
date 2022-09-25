using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// ��ũ�� ��ư�� ������ �� �ڵ� ������ ���� ����Ʈ���� ��ũ���Ѵ�.
public class SocketListScroll : MonoBehaviour
{
    public static Coroutine CurrentRoutine { private set; get; } = null;

    [Tooltip("���ϵ��� ���� ������Ʈ(���� ����Ʈ)")]
    [SerializeField] private Transform socketList;

    [Tooltip("��ư�� ��� ������ ���� ��, ���� ��ũ�ѱ����� ���� �ð�")]
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

    // ���ϵ��� �����ϴ� ����Ʈ
    private Socket[] sockets;

    // ������ ����
    private int socketNum;

    // ���Ͽ� ������ ����� �������� ���� ������Ʈ
    private BlockExecution blockExecution;

    // ���� ���̴� ù ��° ������ �ε���
    private static int firstVisibleIndex = 0;

    // �� ���� ���̴� ������ �ִ� ����
    private const int MAX_VISIBLE_SOCKETS = 7;

    // ���� �� ����
    private const float SOCKET_INTERVAL = 0.05f;

    private void Start()
    {
        blockExecution = gameObject.GetComponent<BlockExecution>();
        socketNum = socketList.childCount;
        sockets = new Socket[socketNum];

        for (int i = 0; i < socketNum; i++)
            sockets[i] = new Socket(socketList.GetChild(i).gameObject);
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

    // ���ϰ� ���Ͽ� ������ ����� Ȱ��ȭ�Ѵ�.
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

    // ���ϰ� ���Ͽ� ������ ����� ��Ȱ��ȭ�Ѵ�.
    private void InactivateSocket(int index)
    {
        // ������ ��ϵ��� ������ ��, ��Ȱ��ȭ
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