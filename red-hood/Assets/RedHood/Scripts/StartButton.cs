using UnityEngine;

// 스타트 버튼
public class StartButton : MonoBehaviour
{
    [SerializeField] private BlockActivation startBlock;

    // 스타트 버튼을 누르면 시작 블록을 실행한다.
    public void OnStartButtonPress()
    {
        ResetResult();
        startBlock.ExecuteBlock();
    }

    // 블록 실행 전 실행 결과와 알림창을 모두 지운다.
    private void ResetResult()
    {
        ResetButton reset = gameObject.AddComponent<ResetButton>();
        reset.DestroyResults();
        Destroy(reset);
    }
}
