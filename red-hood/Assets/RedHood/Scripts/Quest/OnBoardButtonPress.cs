using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OnBoardButtonPress : MonoBehaviour
{
    private XRSocketInteractor[] sockets;

    private void Start()
    {
        sockets = GetComponentsInChildren<XRSocketInteractor>();
    }

    private List<XRGrabInteractable> GetAttachedBlockList()
    {
        List<XRGrabInteractable> blockList = new();
        foreach (XRSocketInteractor socket in sockets)
        {
            List<IXRSelectInteractable> attachedBlocks = socket.interactablesSelected;
            if (attachedBlocks.Count != 0)
                blockList.Add((XRGrabInteractable)attachedBlocks[0]);
        }
        return blockList;
    }

    private XRGrabInteractable GetAttachedBlock(XRSocketInteractor socket)
    {
        List<IXRSelectInteractable> attachedBlocks = socket.interactablesSelected;
        if (attachedBlocks.Count == 0)
            return null;
        return (XRGrabInteractable)attachedBlocks[0];
    }

    public void PressResetButton()
    {
        Debug.Log("���� ��ư ����");
        List<XRGrabInteractable> blockList = GetAttachedBlockList();
        foreach (XRGrabInteractable block in blockList)
        {
            Destroy(block.gameObject);
        }
    }

    public void PressStartButton()
    {
        Debug.Log("��ŸƮ ��ư ����");

        // ��� ����Ʈ ��������
        List<XRGrabInteractable> blockList = GetAttachedBlockList();
        // ��� �ϳ��� activate �ϱ�
        foreach (XRGrabInteractable block in blockList)
        {
            Debug.Log($"��� {block.ToString()} ����!!");
            ActivateEventArgs args = new();
            args.interactableObject = block;
            block.activated.Invoke(args);
        }
    }
}