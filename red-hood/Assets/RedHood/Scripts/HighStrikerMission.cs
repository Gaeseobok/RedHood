using System.Collections.Generic;
using UnityEngine;

public class HighStrikerMission : MonoBehaviour
{
    private static readonly int[] stage1 = { 200 };
    private static readonly int[] stage2 = { 200, 300 };
    private static readonly int[] stage3 = { 100, 200, 300, 400 };
    private static readonly int[][] answer = { stage1, stage2, stage3 };

    private static int stage = 0;

    public void CheckAnswer()
    {
        VariableBlock[] variables = FindObjectsOfType<VariableBlock>();
        List<int> attachedVariables = new();
        foreach (VariableBlock variable in variables)
        {
            if (variable.IsSelectedBySocket())
            {
                attachedVariables.Add(variable.GetInt());
            }
        }

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

        if (attachedVariables.Count < answer[stage].Length)
        {
            text = "������ �� �ʿ��ؿ�!";
            isCorrect = false;
        }
        else
        {
            for (int i = 0; i < attachedVariables.Count; i++)
            {
                if (!attachedVariables.Contains(answer[stage][i]))
                {
                    text = "������ ���� ������ ��������!";
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
            Destroy(reset);
        }
        else
        {
            msg.ActivateFailureWindow(text);
            msg.PlayFailureSound();
        }

        HighStrikerHit highStrikerHit = gameObject.AddComponent<HighStrikerHit>();
        highStrikerHit.ResetScore();
        Destroy(highStrikerHit);
    }
}
