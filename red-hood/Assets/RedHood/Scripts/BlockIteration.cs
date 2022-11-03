using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BlockIteration : MonoBehaviour
{
    private static Stack<BlockActivation> iterStartBlockStack = new();

    private const string ITER_START_TAG = "IterStartBlock";
    private const string ITER_END_TAG = "IterEndBlock";
    private const string VAR_SOCKET = "VariableSocket";

    private BlockActivation block;
    private BlockActivation nextBlock = null;
    private bool setup = false;
    private int iterNum = -1;

    private void Start()
    {
        block = GetComponent<BlockActivation>();
    }

    // �ݺ� ��� ���� ����
    public void SetIteration(BlockActivation block)
    {
        if (setup)
        {
            return;
        }

        if (block.CompareTag(ITER_START_TAG))
        {
            Debug.Log("�ݺ� ����!!");

            iterStartBlockStack.Push(block);
        }
        else if (block.CompareTag(ITER_END_TAG))
        {
            try
            {
                nextBlock = iterStartBlockStack.Pop();
            } catch (InvalidOperationException)
            {
                PrintErrorMsg();
            }
            
            XRSocketInteractor variableSocket = block.transform.Find(VAR_SOCKET).GetComponent<XRSocketInteractor>();
            IXRSelectInteractable attach = variableSocket.firstInteractableSelected;
            if (attach == null)
            {
                PrintErrorMsg();
            }
            else
            {
                XRGrabInteractable variableBlock = (XRGrabInteractable)attach;
                iterNum = Convert.ToInt32(variableBlock.GetComponentInChildren<TMP_Text>().text);
                Debug.Log("�ݺ� ����: " + iterNum);
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
            // �ݺ��� �����ٸ� �ݺ��� ��������
            //block.GetNextBlock().ExecuteBlock();
            Debug.Log("�ݺ� ��!!");
        }
    }

    private void PrintErrorMsg()
    {
        // TODO: ���� ó��(���� ����)
        Debug.LogWarning("�ݺ� ����!!");
    }
}
