using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AttachedBlock : MonoBehaviour
{
    [Tooltip("���ϵ��� ���� ������Ʈ(���� ����Ʈ)")]
    public Transform socketList;

    // ���� ���Ͽ� ������ ��� ����� ����Ʈ ���·� �����Ѵ�.
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
}