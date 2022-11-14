using UnityEngine;

public class SandwichMission : MonoBehaviour
{
    private const string BREAD = "Bread";
    private const string CHEESE = "Cheese";
    private const string LETTUCE = "Lettuce";
    private const string TOMATO = "Tomato";

    private static readonly string[] stage1 = { BREAD, LETTUCE, TOMATO, CHEESE, BREAD };
    private static readonly string[] stage2 = { BREAD, LETTUCE, TOMATO, CHEESE,
                                                     LETTUCE, TOMATO, CHEESE,
                                                     LETTUCE, TOMATO, CHEESE,BREAD };
    private static readonly string[][] answer = { stage2 };

    private static int stage = 0;

    public void CheckAnswer()
    {
        if (stage > answer.Length)
        {
            return;
        }

        GameObject[] questModels = GameObject.FindGameObjectsWithTag("QuestModel");

        string text = "";
        bool isCorrect = true;

        if (!TryGetComponent(out PopUpMessage msg))
        {
            msg = gameObject.AddComponent<PopUpMessage>();
        }

        if (questModels.Length < answer[stage].Length)
        {
            text = "��ᰡ �� �ʿ��ؿ�!";
            isCorrect = false;
        }
        else if (questModels.Length > answer[stage].Length)
        {
            text = "��ᰡ �ʹ� ���� �����!";
            isCorrect = false;
        }
        else
        {
            for (int i = 0; i < questModels.Length; i++)
            {
                if (!questModels[i].name.StartsWith(answer[stage][i]))
                {
                    text = "����� ������ �޶��!";
                    isCorrect = false;
                    break;
                }
            }
        }

        if (isCorrect)
        {
            if (++stage == answer.Length)
            {
                // �̼� Ŭ���� ��, Ŭ��� �˸��� �޼����� ���� Ȱ��ȭ
                msg.ActivateClearWindow();
                msg.PlayClearSound();
            }
            else
            {
                // ������ ���, ������ �˸��� �޼����� ���� Ȱ��ȭ
                msg.ActivateSuccessWindow();
                msg.PlaySuccessSound();

                //TODO: ���� ���� ����â Ȱ��ȭ
            }
        }
        else
        {
            msg.ActivateErrorWindow(text);
            msg.PlayFailureSound();
        }
    }
}
