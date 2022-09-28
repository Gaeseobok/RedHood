using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// ���ϰ� ���Ͽ� ������ ��ϵ��� �����Ѵ�.
public class SocketInformation : MonoBehaviour
{
    private XRInteractionManager interactionManager;

    // ���� ������Ʈ�� ������ ��ϵ��� �����ϴ� ����ü
    public struct SocketInfo
    {
        public XRSocketInteractor Socket;
        public XRGrabInteractable AttachedBlock;
        public XRGrabInteractable AttachedVarBlock;

        public SocketInfo(XRSocketInteractor socket)
        {
            Socket = socket;
            AttachedBlock = null;
            AttachedVarBlock = null;
        }
    }

    internal static int socketNum;
    internal static SocketInfo[] socketInfos;

    private void Awake()
    {
        interactionManager = gameObject.AddComponent<XRInteractionManager>();

        socketNum = transform.childCount;
        socketInfos = new SocketInfo[socketNum];

        for (int i = 0; i < socketNum; i++)
            socketInfos[i] = new SocketInfo(transform.GetChild(i).GetComponent<XRSocketInteractor>());
    }

    // ���Ͽ� ����� ��ϵǸ�, socketInfos�� ������Ʈ�Ѵ�.
    public void SetCurEnteredBlock(int idx)
    {
        IXRSelectInteractable attachedBlock = socketInfos[idx].Socket.firstInteractableSelected;
        if (attachedBlock != null)
        {
            Debug.Log($"{idx}��° ��� ��ϵ�");

            socketInfos[idx].AttachedBlock = (XRGrabInteractable)attachedBlock;
            socketInfos[idx].AttachedVarBlock = GetAttachedVariableBlock(socketInfos[idx].AttachedBlock);
        }
    }

    // ���Ͽ��� ����� �����Ǹ�, socketInfos�� ������Ʈ�Ѵ�.
    public void SetCurExitedBlock(int idx)
    {
        Debug.Log($"{idx}��° ��� ������");

        socketInfos[idx].AttachedBlock = null;
        socketInfos[idx].AttachedVarBlock = null;
    }

    // ����ִ� ������ �ִ��� Ȯ���Ѵ�.
    public static bool IsSocketEmpty()
    {
        foreach (SocketInfo socketInfo in socketInfos)
        {
            if (socketInfo.AttachedBlock == null)
                return true;

            XRSocketInteractor variableSocket = socketInfo.AttachedBlock.GetComponentInChildren<XRSocketInteractor>();
            if (variableSocket != null && socketInfo.AttachedVarBlock == null)
                return true;
        }
        return false;
    }

    // ��� ���� ���� ����(Socket_Variable)�� ���� ����� �����Ѵٸ� �ش� ���� ����� �����Ѵ�.
    public XRGrabInteractable GetAttachedVariableBlock(XRGrabInteractable block)
    {
        XRSocketInteractor variableSocket = block.GetComponentInChildren<XRSocketInteractor>();
        if (variableSocket != null)
        {
            IXRSelectInteractable variableBlock = variableSocket.firstInteractableSelected;
            if (variableBlock != null)
                return (XRGrabInteractable)variableBlock;
        }
        return null;
    }

    // ���ϰ� ���Ͽ� ������ ����� Ȱ��ȭ�Ѵ�.
    public void ActivateSocket(int idx)
    {
        socketInfos[idx].Socket.gameObject.SetActive(true);

        if (socketInfos[idx].AttachedBlock != null)
        {
            socketInfos[idx].AttachedBlock.gameObject.SetActive(true);
            interactionManager.SelectEnter(socketInfos[idx].Socket, socketInfos[idx].AttachedBlock);

            if (socketInfos[idx].AttachedVarBlock != null)
            {
                XRSocketInteractor socketInteractor = socketInfos[idx].AttachedBlock.GetComponentInChildren<XRSocketInteractor>();

                socketInfos[idx].AttachedVarBlock.gameObject.SetActive(true);
                interactionManager.SelectEnter(socketInteractor, socketInfos[idx].AttachedVarBlock);
            }
        }
    }

    // ���ϰ� ���Ͽ� ������ ����� ��Ȱ��ȭ�Ѵ�.
    public void InactivateSocket(int idx)
    {
        SetCurEnteredBlock(idx);
        socketInfos[idx].Socket.gameObject.SetActive(false);

        if (socketInfos[idx].AttachedBlock != null)
        {
            socketInfos[idx].AttachedBlock.gameObject.SetActive(false);
            if (socketInfos[idx].AttachedVarBlock != null)
                socketInfos[idx].AttachedVarBlock.gameObject.SetActive(false);
        }

        // ������ ��ϵ��� ������ ��, ��Ȱ��ȭ
        //IXRSelectInteractable interactableSelected = socketInfos[idx].Socket.firstInteractableSelected;

        //if (interactableSelected != null)
        //{
        //    XRGrabInteractable attachedBlock = (XRGrabInteractable)interactableSelected;
        //    socketInfos[idx].AttachedBlock = attachedBlock;

        //    XRGrabInteractable attachedVarBlock = GetAttachedVariableBlock(attachedBlock);
        //    if (attachedVarBlock != null)
        //    {
        //        socketInfos[idx].AttachedVarBlock = attachedVarBlock;
        //        socketInfos[idx].AttachedVarBlock.gameObject.SetActive(false);
        //    }

        //    socketInfos[idx].AttachedBlock.gameObject.SetActive(false);
        //}

        //socketInfos[idx].Socket.gameObject.SetActive(false);
    }
}