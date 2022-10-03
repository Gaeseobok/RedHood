using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// ���ϰ� ���Ͽ� ������ ��ϵ��� �����Ѵ�.
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

    // ���Ͽ� ���� ������ ����� �����ϰ� �����Ѵ�. (������ ����� ������ null ����)
    public XRGrabInteractable SetCurrentBlock(int idx)
    {
        IXRSelectInteractable attachedBlock = sockets[idx].firstInteractableSelected;
        blocks[idx] = (attachedBlock == null) ? null : (XRGrabInteractable)attachedBlock;
        return blocks[idx];
    }

    // ��� ���� ���� ����(Socket_Variable)�� ���� ����� �����Ѵٸ� �ش� ���� ����� �����Ѵ�.
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

    // ����ִ� ������ �ִ��� Ȯ���Ѵ�.
    internal bool IsSocketEmpty()
    {
        XRSocketInteractor variableSocket;

        for (int i = 0; i < socketNum; i++)
        {
            // ������ ��Ȱ��ȭ�� ���
            if (!sockets[i].gameObject.activeInHierarchy)
            {
                // ������ ����ִٸ� true ����
                if (blocks[i] == null)
                    return true;

                // ��� ���� ������ ����ִٸ� true ����
                variableSocket = blocks[i].GetComponentInChildren<XRSocketInteractor>();
                if (variableSocket != null && variableBlocks[i] == null)
                    return true;
            }
            // ������ Ȱ��ȭ�� ���, ���� ������ ����� ���� ������
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
        // �� ������ ���ٸ� false ����
        return false;
    }

    // ���ϰ� ���Ͽ� ������ ����� Ȱ��ȭ�Ѵ�.
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

    // ���ϰ� ���Ͽ� ������ ����� ��Ȱ��ȭ�Ѵ�.
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