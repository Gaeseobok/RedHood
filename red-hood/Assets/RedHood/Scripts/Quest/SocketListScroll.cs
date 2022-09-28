using System.Collections;
using UnityEngine;
using static SocketInformation;

// ��ũ�� ��ư�� ������ �� �ڵ� ������ ���� ����Ʈ���� ��ũ���Ѵ�.
public class SocketListScroll : MonoBehaviour
{
    public static Coroutine CurrentRoutine { private set; get; } = null;

    [Tooltip("���� ����Ʈ")]
    [SerializeField] private SocketInformation socketList;

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
    private Vector3 defaultPosition;

    private void Start()
    {
        defaultPosition = gameObject.transform.position;
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

    //// ���ϰ� ���Ͽ� ������ ����� Ȱ��ȭ�Ѵ�.
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

    //// ���ϰ� ���Ͽ� ������ ����� ��Ȱ��ȭ�Ѵ�.
    //private void InactivateSocket(int index)
    //{
    //    // ������ ��ϵ��� ������ ��, ��Ȱ��ȭ
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

    // ��ũ���� �ʱ�ȭ�Ѵ�.
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

    // �ε����� �´� ��ġ�� ��ũ��
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
        //Debug.Log($"{x} �ε��� �������� ��ũ��");
    }
}