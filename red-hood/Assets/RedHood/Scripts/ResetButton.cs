using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// ���� ��ư
public class ResetButton : MonoBehaviour
{
    [SerializeField] private BlockActivation startBlock;

    private const string QUEST_MODEL_TAG = "QuestModel";

    // ���� ��ư�� ������ ��� ��ϰ� ����� ���� ����� �����Ѵ�.
    public void OnResetButtonPress()
    {
        Debug.Log("����~");
        DestroyResults();

        BlockActivation block = startBlock.GetNextBlock();
        if (block != null)
        {
            DestroyBlock(block.gameObject);
        }
    }

    // ��ϵ��� ��������� �����Ѵ�.
    private void DestroyBlock(GameObject block)
    {
        XRSocketInteractor[] sockets = block.GetComponentsInChildren<XRSocketInteractor>();

        // ������ ������ �ٷ� ������Ʈ ����
        if (sockets.Length == 0)
        {
            Destroy(gameObject);
            return;
        }

        // ������ ����� �ִٸ� ���� ����
        foreach (XRSocketInteractor socket in sockets)
        {
            IXRSelectInteractable attach = socket.firstInteractableSelected;
            if (attach != null)
            {
                DestroyBlock(((XRGrabInteractable)attach).gameObject);
            }
        }

        // ���� ��� ����
        Destroy(block);
    }

    // ����� ���� ����� �����Ѵ�.
    private void DestroyResults()
    {
        GameObject[] results = GameObject.FindGameObjectsWithTag(QUEST_MODEL_TAG);
        foreach (GameObject result in results)
        {
            Destroy(result);
        }
    }
}
