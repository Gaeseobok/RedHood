using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AttachedBlock : MonoBehaviour
{
    [Tooltip("���ϵ��� ���� ������Ʈ(���� ����Ʈ)")]
    public Transform socketList;

    // ���� ���Ͽ� ������ ����� �����Ѵ�.
    public XRGrabInteractable GetAttachedBlock(XRSocketInteractor socket)
    {
        List<IXRSelectInteractable> attachedBlocks = socket.interactablesSelected;
        if (attachedBlocks.Count != 0)
            return (XRGrabInteractable)attachedBlocks[0];
        return null;
    }

    // ���� ���Ͽ� ������ ��� ����� ����Ʈ ���·� �����Ѵ�.
    public List<XRGrabInteractable> GetAttachedBlockList(XRSocketInteractor[] sockets)
    {
        List<XRGrabInteractable> blockList = new();
        foreach (XRSocketInteractor socket in sockets)
        {
            XRGrabInteractable attachedBlock = GetAttachedBlock(socket);
            if (attachedBlock != null)
                blockList.Add(attachedBlock);
        }
        return blockList;
    }

    // ��� ���� ���� ����(Socket_Variable)�� ���� ����� �����Ѵٸ� �ش� ���� ����� �����Ѵ�.
    public XRGrabInteractable GetAttachedVariableBlock(XRGrabInteractable block)
    {
        XRSocketInteractor variableSocket = block.GetComponentInChildren<XRSocketInteractor>();
        if (variableSocket != null)
        {
            List<IXRSelectInteractable> variableBlocks = variableSocket.interactablesSelected;
            if (variableBlocks.Count > 0)
            {
                XRGrabInteractable variableBlock = (XRGrabInteractable)variableSocket.interactablesSelected[0];
                return variableBlock;
            }
        }
        return null;
    }
}