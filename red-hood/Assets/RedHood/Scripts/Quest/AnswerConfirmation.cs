using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// Socket에 현재 위치한 블록이 정답인지 확인한다.
public class AnswerConfirmation : MonoBehaviour
{
    [Tooltip("정답 코드 블록 프리팹(None으로 설정 시 모든 블록 정답 처리)")]
    [SerializeField] private XRGrabInteractable AnswerBlock;

    private XRSocketInteractor socketInteractor;

    // 정답 코드 블록의 텍스트
    private string answer;

    private void Start()
    {
        socketInteractor = GetComponent<XRSocketInteractor>();
        if (AnswerBlock != null)
            answer = AnswerBlock.GetComponentInChildren<TMP_Text>().text;
    }

    public bool CompareAnswer(XRGrabInteractable currentBlock)
    {
        if (AnswerBlock == null)
            return true;

        string code = currentBlock.GetComponentInChildren<TMP_Text>().text;
        return code == answer;
    }
}