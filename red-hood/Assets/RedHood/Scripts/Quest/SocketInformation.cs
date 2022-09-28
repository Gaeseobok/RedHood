using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// 소켓과 소켓에 부착된 블록들을 조작한다.
public class SocketInformation : MonoBehaviour
{
    private XRInteractionManager interactionManager;

    // 소켓 오브젝트와 부착된 블록들을 저장하는 구조체
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

    // 소켓에 블록이 등록되면, socketInfos를 업데이트한다.
    public void SetCurEnteredBlock(int idx)
    {
        IXRSelectInteractable attachedBlock = socketInfos[idx].Socket.firstInteractableSelected;
        if (attachedBlock != null)
        {
            Debug.Log($"{idx}번째 블록 등록됨");

            socketInfos[idx].AttachedBlock = (XRGrabInteractable)attachedBlock;
            socketInfos[idx].AttachedVarBlock = GetAttachedVariableBlock(socketInfos[idx].AttachedBlock);
        }
    }

    // 소켓에서 블록이 해제되면, socketInfos를 업데이트한다.
    public void SetCurExitedBlock(int idx)
    {
        Debug.Log($"{idx}번째 블록 해제됨");

        socketInfos[idx].AttachedBlock = null;
        socketInfos[idx].AttachedVarBlock = null;
    }

    // 비어있는 소켓이 있는지 확인한다.
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

    // 블록 내부 변수 소켓(Socket_Variable)에 변수 블록이 존재한다면 해당 변수 블록을 리턴한다.
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

    // 소켓과 소켓에 부착된 블록을 활성화한다.
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

    // 소켓과 소켓에 부착된 블록을 비활성화한다.
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

        // 부착된 블록들을 저장한 후, 비활성화
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