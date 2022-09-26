using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// ��ũ�� ��ư�� ������ �� �ڵ� ������ ���� ����Ʈ���� ��ũ���Ѵ�.
[System.Obsolete]
public class SocketListScroll : AttachedBlock
{
    public static Coroutine CurrentRoutine { private set; get; } = null;

    [Tooltip("��ư�� ��� ������ ���� ��, ���� ��ũ�ѱ����� ���� �ð�")]
    [SerializeField] private float scrollDelay = 1.0f;

    private XRInteractionManager interactionManager;

    // ���� ������Ʈ�� ������ ��ϵ��� �����ϴ� ����ü
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

    // ������ ����
    private int socketNum;

    // ���ϰ� ������ ��ϵ��� �����ϴ� ����Ʈ
    private static SocketInfo[] socketInfos;

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
        interactionManager = gameObject.AddComponent<XRInteractionManager>();
        socketNum = socketList.childCount;
        socketInfos = new SocketInfo[socketNum];

        for (int i = 0; i < socketNum; i++)
            socketInfos[i] = new SocketInfo(socketList.GetChild(i).gameObject);
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

    // ���ϰ� ���Ͽ� ������ ����� ��Ȱ��ȭ�Ѵ�.
    private void InactivateSocket(int index)
    {
        // ������ ��ϵ��� ������ ��, ��Ȱ��ȭ
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