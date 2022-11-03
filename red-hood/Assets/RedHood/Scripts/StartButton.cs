using System.Collections;
using UnityEngine;

// 스타트 버튼
public class StartButton : MonoBehaviour
{
    [SerializeField] private BlockActivation startBlock;

    // 스타트 버튼을 누르면 시작 블록을 실행한다.
    public void OnStartButtonPress()
    {
        startBlock.ExecuteBlock();
    }
}
