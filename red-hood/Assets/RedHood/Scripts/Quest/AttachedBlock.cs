using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AttachedBlock : MonoBehaviour
{
    [Tooltip("소켓들의 상위 오브젝트(소켓 리스트)")]
    public Transform socketList;

    // 현재 소켓에 부착된 모든 블록을 리스트 형태로 리턴한다.
    public List<XRGrabInteractable> GetAttachedBlockList(XRSocketInteractor[] sockets)
    {
        List<XRGrabInteractable> blockList = new();
        foreach (XRSocketInteractor socket in sockets)
        {
            IXRSelectInteractable attachedBlock = socket.firstInteractableSelected;
            if (attachedBlock != null)
                blockList.Add((XRGrabInteractable)attachedBlock);
        }
        return blockList;
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
}