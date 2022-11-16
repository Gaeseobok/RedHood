using UnityEngine;

public class SandwichMission : MonoBehaviour
{
    private const string BREAD = "Bread";
    private const string CHEESE = "Cheese";
    private const string LETTUCE = "Lettuce";
    private const string TOMATO = "Tomato";

    private static readonly string[] stage1 = { BREAD, LETTUCE, CHEESE, TOMATO, BREAD };
    private static readonly string[] stage2 = { BREAD, LETTUCE, CHEESE, TOMATO,
                                                     LETTUCE, CHEESE, TOMATO,
                                                     LETTUCE, CHEESE, TOMATO, BREAD };
    private static readonly string[] stage3 = { BREAD, TOMATO, CHEESE, CHEESE, CHEESE, LETTUCE,
                                                       TOMATO, CHEESE, CHEESE, CHEESE, LETTUCE, BREAD };
    private static readonly string[][] answer = { stage1, stage2, stage2, stage3 };

    private static int stage = 0;

    public void CheckAnswer()
    {
        GameObject[] questModels = GameObject.FindGameObjectsWithTag("QuestModel");

        string text = "";
        bool isCorrect = true;

        if (!TryGetComponent(out PopUpMessage msg))
        {
            msg = gameObject.AddComponent<PopUpMessage>();
        }

        if (stage >= answer.Length)
        {
            msg.ActivateClearWindow();
            msg.PlayClearSound();
            return;
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
            if (++stage >= answer.Length)
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
            }

            // ���� ���� ����â Ȱ��ȭ
            msg.SetDescWindow(stage - 1, false);
            msg.SetDescWindow(stage, true);

            ResetButton reset = gameObject.AddComponent<ResetButton>();
            reset.DestroyBlocks();
        }
        else
        {
            msg.ActivateFailureWindow(text);
            msg.PlayFailureSound();
        }
    }
}
