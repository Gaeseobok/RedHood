using UnityEngine;

public class SandwichMission : MonoBehaviour
{
    private const string BREAD = "Bread";
    private const string CHEESE = "Cheese";
    private const string LETTUCE = "Lettuce";
    private const string TOMATO = "Tomato";

    private static readonly string[] answer1 = { BREAD, LETTUCE, TOMATO, CHEESE, BREAD };
    private static readonly string[] answer2 = { BREAD, LETTUCE, LETTUCE, LETTUCE,
                                                TOMATO, TOMATO, TOMATO, CHEESE, CHEESE, CHEESE, BREAD };
    private static readonly string[][] answer = { answer1, answer2, answer2 };

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
            // ������ ���, ������ �˸��� �޼����� ����ϰ� ��� ��ϰ� ���� ����� ����
            msg.ActivateSuccessWindow();

            //TODO: ���� ���� ����â Ȱ��ȭ
            msg.PlaySuccessSound();
            stage++;
        }
        else
        {
            msg.ActivateErrorWindow(text);
            msg.PlayFailureSound();
        }
    }
}
