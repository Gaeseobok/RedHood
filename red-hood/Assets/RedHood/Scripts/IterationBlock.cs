using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class IterationBlock : MonoBehaviour
{
    private static Stack<BlockActivation> iterStartBlockStack = new();

    private const string ITER_START_BLOCK = "IterStartBlock";
    private const string ITER_END_BLOCK = "IterEndBlock";
    private const string VAR_SOCKET = "VariableSocket";

    private BlockActivation blockActivation;
    private BlockActivation nextBlock = null;
    private bool setup = false;
    private int iterNum = -1;

    private void Start()
    {
        blockActivation = GetComponent<BlockActivation>();
    }

    // �ݺ� ��� ���� ����
    public void SetIteration()
    {
        if (setup)
        {
            return;
        }

        if (blockActivation.CompareTag(ITER_START_BLOCK))
        {
            Debug.Log("�ݺ� ����!!");

            iterStartBlockStack.Push(blockActivation);
        }
        else if (blockActivation.CompareTag(ITER_END_BLOCK))
        {
            try
            {
                nextBlock = iterStartBlockStack.Pop();
            } catch (InvalidOperationException)
            {
                // TODO: ���� ó��(���� ����)
                Debug.LogWarning("�ݺ� ����: �ݺ� ���� ����� �������� ����");
                return;
            }
            
            XRSocketInteractor variableSocket = blockActivation.transform.Find(VAR_SOCKET).GetComponent<XRSocketInteractor>();
            IXRSelectInteractable attach = variableSocket.firstInteractableSelected;
            if (attach != null)
            {
                XRGrabInteractable variableBlock = (XRGrabInteractable)attach;
                iterNum = variableBlock.GetComponent<VariableBlock>().GetInt();
                Debug.Log("�ݺ� ����: " + iterNum);
            }
            else
            {
                Debug.LogWarning("�ݺ� ����: ���� ����� �������� ����");
            }
        }

        setup = true;
    }

    // �ݺ��� ����
    public void Iterate()
    {
        iterNum--;
        if (iterNum > 0)
        {
            // �ݺ� Ƚ���� ���Ҵٸ� �ݺ��� �ٽ� ����
            nextBlock.ExecuteBlock();
            Debug.Log("�ݺ� ����: " + iterNum);

        }
        else
        {
            Debug.Log("�ݺ� ��!!");
            // �ݺ��� �����ٸ� �ݺ��� ��������
            BlockActivation nextBlock = blockActivation.GetNextBlock();
            if (nextBlock != null)
            {
                nextBlock.ExecuteBlock();
            }

            // TODO: IterStartBlock, IterEndBlock ������ ����
        }
    }

}
