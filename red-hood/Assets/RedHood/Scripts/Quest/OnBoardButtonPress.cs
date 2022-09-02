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
        Debug.Log("리셋 버튼 눌림");
        List<XRGrabInteractable> blockList = GetAttachedBlockList();
        foreach (XRGrabInteractable block in blockList)
        {
            Destroy(block.gameObject);
        }
    }

    public void PressStartButton()
    {
        Debug.Log("스타트 버튼 눌림");

        // 블록 리스트 가져오기
        List<XRGrabInteractable> blockList = GetAttachedBlockList();
        // 블록 하나씩 activate 하기
        foreach (XRGrabInteractable block in blockList)
        {
            Debug.Log($"블록 {block.ToString()} 실행!!");
            ActivateEventArgs args = new();
            args.interactableObject = block;
            block.activated.Invoke(args);
        }
    }
}