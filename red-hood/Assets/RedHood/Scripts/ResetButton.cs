using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// 리셋 버튼
public class ResetButton : MonoBehaviour
{
    [SerializeField] private BlockActivation startBlock;

    private const string QUEST_MODEL_TAG = "QuestModel";

    // 리셋 버튼을 누르면 모든 블록과 블록의 실행 결과를 제거한다.
    public void OnResetButtonPress()
    {
        Debug.Log("리셋~");
        DestroyResults();

        BlockActivation block = startBlock.GetNextBlock();
        if (block != null)
        {
            DestroyBlock(block.gameObject);
        }
    }

    // 블록들을 재귀적으로 제거한다.
    private void DestroyBlock(GameObject block)
    {
        XRSocketInteractor[] sockets = block.GetComponentsInChildren<XRSocketInteractor>();

        // 소켓이 없으면 바로 오브젝트 제거
        if (sockets.Length == 0)
        {
            Destroy(gameObject);
            return;
        }

        // 부착된 블록이 있다면 먼저 제거
        foreach (XRSocketInteractor socket in sockets)
        {
            IXRSelectInteractable attach = socket.firstInteractableSelected;
            if (attach != null)
            {
                DestroyBlock(((XRGrabInteractable)attach).gameObject);
            }
        }

        // 현재 블록 제거
        Destroy(block);
    }

    // 블록의 실행 결과를 제거한다.
    private void DestroyResults()
    {
        GameObject[] results = GameObject.FindGameObjectsWithTag(QUEST_MODEL_TAG);
        foreach (GameObject result in results)
        {
            Destroy(result);
        }
    }
}
