using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// 소켓과 소켓에 부착된 블록들을 조작한다.
public class SocketList : MonoBehaviour
{
    private XRInteractionManager interactionManager;

    internal int socketNum;

    private XRSocketInteractor[] sockets;
    private XRGrabInteractable[] blocks;
    private XRGrabInteractable[] variableBlocks;

    private void Awake()
    {
        interactionManager = gameObject.AddComponent<XRInteractionManager>();

        socketNum = transform.childCount;
        sockets = GetComponentsInChildren<XRSocketInteractor>(includeInactive: true);
        blocks = new XRGrabInteractable[socketNum];
        variableBlocks = new XRGrabInteractable[socketNum];
    }

    // 소켓에 현재 부착된 블록을 저장하고 리턴한다. (부착된 블록이 없으면 null 리턴)
    public XRGrabInteractable SetCurrentBlock(int idx)
    {
        IXRSelectInteractable attachedBlock = sockets[idx].firstInteractableSelected;
        blocks[idx] = (attachedBlock == null) ? null : (XRGrabInteractable)attachedBlock;
        return blocks[idx];
    }

    // 블록 내부 변수 소켓(Socket_Variable)에 변수 블록이 존재한다면 해당 변수 블록을 리턴한다.
    internal XRGrabInteractable GetCurrentVariableBlock(XRGrabInteractable block)
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

    // 비어있는 소켓이 있는지 확인한다.
    internal bool IsSocketEmpty()
    {
        XRSocketInteractor variableSocket;

        for (int i = 0; i < socketNum; i++)
        {
            // 소켓이 비활성화된 경우
            if (!sockets[i].gameObject.activeInHierarchy)
            {
                // 소켓이 비어있다면 true 리턴
                if (blocks[i] == null)
                    return true;

                // 블록 내부 소켓이 비어있다면 true 리턴
                variableSocket = blocks[i].GetComponentInChildren<XRSocketInteractor>();
                if (variableSocket != null && variableBlocks[i] == null)
                    return true;
            }
            // 소켓이 활성화된 경우, 현재 부착된 블록을 새로 가져옴
            else
            {
                XRGrabInteractable block = SetCurrentBlock(i);
                if (block == null)
                    return true;

                variableSocket = block.GetComponentInChildren<XRSocketInteractor>();
                if (variableSocket != null)
                {
                    XRGrabInteractable variableBlock = GetCurrentVariableBlock(block);
                    if (variableBlock == null)
                        return true;
                }
            }
        }
        // 빈 소켓이 없다면 false 리턴
        return false;
    }

    // 소켓과 소켓에 부착된 블록을 활성화한다.
    internal void ActivateSocket(int idx)
    {
        if (sockets[idx].gameObject.activeInHierarchy)
            return;

        sockets[idx].gameObject.SetActive(true);

        if (blocks[idx] != null)
        {
            blocks[idx].gameObject.SetActive(true);
            interactionManager.SelectEnter(sockets[idx], (IXRSelectInteractable)blocks[idx]);

            if (variableBlocks[idx] != null)
            {
                XRSocketInteractor variableSocket = blocks[idx].GetComponentInChildren<XRSocketInteractor>();
                variableBlocks[idx].gameObject.SetActive(true);
                interactionManager.SelectEnter(variableSocket, (IXRSelectInteractable)variableBlocks[idx]);
            }
        }
    }

    // 소켓과 소켓에 부착된 블록을 비활성화한다.
    internal void InactivateSocket(int idx)
    {
        if (!sockets[idx].gameObject.activeInHierarchy)
            return;

        XRGrabInteractable block = SetCurrentBlock(idx);

        if (block != null)
        {
            variableBlocks[idx] = GetCurrentVariableBlock(block);
            if (variableBlocks[idx] != null)
            {
                variableBlocks[idx].gameObject.SetActive(false);
            }
            block.gameObject.SetActive(false);
        }

        sockets[idx].gameObject.SetActive(false);
    }

    internal void DestroyBlocks(int idx)
    {
        if (!sockets[idx].gameObject.activeInHierarchy && blocks[idx] != null)
        {
            Destroy(blocks[idx].gameObject);
            blocks[idx] = null;

            if (variableBlocks[idx] != null)
            {
                Destroy(variableBlocks[idx].gameObject);
                variableBlocks[idx] = null;
            }
        }
        if (sockets[idx].gameObject.activeInHierarchy)
        {
            XRGrabInteractable block = SetCurrentBlock(idx);
            if (block != null)
            {
                XRGrabInteractable variableBlock = GetCurrentVariableBlock(block);
                if (variableBlock != null)
                    Destroy(variableBlock.gameObject);

                Destroy(block.gameObject);
            }
        }
    }
}