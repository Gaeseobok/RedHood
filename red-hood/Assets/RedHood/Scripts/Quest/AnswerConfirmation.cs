using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// Socket�� ���� ��ġ�� ����� �������� Ȯ���Ѵ�.
public class AnswerConfirmation : MonoBehaviour
{
    [Tooltip("���� �ڵ� ��� ������(None���� ���� �� ��� ��� ���� ó��)")]
    [SerializeField] private XRGrabInteractable AnswerBlock;

    private XRSocketInteractor socketInteractor;

    // ���� �ڵ� ����� �ؽ�Ʈ
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