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
            text = "재료가 더 필요해요!";
            isCorrect = false;
        }
        else if (questModels.Length > answer[stage].Length)
        {
            text = "재료가 너무 많이 들어갔어요!";
            isCorrect = false;
        }
        else
        {
            for (int i = 0; i < questModels.Length; i++)
            {
                if (!questModels[i].name.StartsWith(answer[stage][i]))
                {
                    text = "재료의 순서가 달라요!";
                    isCorrect = false;
                    break;
                }
            }
        }

        if (isCorrect)
        {
            if (++stage == answer.Length)
            {
                // 미션 클리어 시, 클리어를 알리는 메세지와 사운드 활성화
                msg.ActivateClearWindow();
                msg.PlayClearSound();
            }
            else
            {
                // 정답인 경우, 정답을 알리는 메세지와 사운드 활성화
                msg.ActivateSuccessWindow();
                msg.PlaySuccessSound();

                //TODO: 다음 문제 설명창 활성화
            }
        }
        else
        {
            msg.ActivateErrorWindow(text);
            msg.PlayFailureSound();
        }
    }
}
