using UnityEngine;

public class SandwichMission : MonoBehaviour
{
    private const string BREAD = "Bread";
    private const string CHEESE = "Cheese";
    private const string LETTUCE = "Lettuce";
    private const string TOMATO = "Tomato";

    private static readonly string[] answer1 = { BREAD, LETTUCE, TOMATO, CHEESE, BREAD };

    public void CheckAnswer()
    {
        GameObject[] questModels = GameObject.FindGameObjectsWithTag("QuestModel");

        string text = "";
        bool isCorrect = true;

        if (!TryGetComponent(out PopUpMessage msg))
        {
            msg = gameObject.AddComponent<PopUpMessage>();
        }

        if (questModels.Length < answer1.Length)
        {
            text = "��ᰡ �� �ʿ��ؿ�!";
            isCorrect = false;
        }
        else if (questModels.Length > answer1.Length)
        {
            text = "��ᰡ �ʹ� ���� �����!";
            isCorrect = false;
        }
        else
        {
            for (int i = 0; i < questModels.Length; i++)
            {
                if (!questModels[i].name.StartsWith(answer1[i]))
                {
                    text = "����� ������ �޶��!";
                    isCorrect = false;
                    break;
                }
            }
        }

        if (isCorrect)
        {
            msg.ActivateSuccessWindow();
        }
        else
        {
            msg.ActivateErrorWindow(text);
        }
    }
}
