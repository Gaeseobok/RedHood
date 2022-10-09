using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMapMovement : MonoBehaviour
{
    public Coroutine CurrentRoutine { private set; get; } = null;

    private static Animator animator;

    private float animationTime = 0.7f;

    private const string QUEST_MODEL_TAG = "QuestModel";
    private const string CHARACTER_TAG = "CubeMapCharacter";
    private const string MOVE_SPEED_PARAM = "MoveSpeed";

    //private Vector3 defaultEuler = new(0.0f, 180.0f, 0.0f);
    private Vector3 moveInterval = new(0.0f, 0.0f, 0.131f);

    public void InitCubeMap()
    {
        Debug.Log("ť�� �� �ʱ�ȭ");
        GameObject.FindGameObjectWithTag(QUEST_MODEL_TAG).transform.rotation = Quaternion.identity;
        animator = GameObject.FindGameObjectWithTag(CHARACTER_TAG).GetComponent<Animator>();
        animator.transform.localPosition = new(0.0f, 0.0f, 0.0f);
        animator.transform.localRotation = Quaternion.identity;
    }

    public void MoveForward()
    {
        Debug.Log("������ ����");
        StopAllCoroutines();
        StartCoroutine(StartMove());
    }

    public void RotateLeft()
    {
        Debug.Log("�������� ȸ��");
        float leftAngle = -90.0f;

        StopAllCoroutines();
        StartCoroutine(StartRotate(leftAngle));
    }

    public void RotateRight()
    {
        Debug.Log("���������� ȸ��");
        float rightAngle = 90.0f;

        StopAllCoroutines();
        StartCoroutine(StartRotate(rightAngle));
    }

    private IEnumerator StartMove()
    {
        // TODO: �޸��� �ִϸ��̼� ��� �� ����
        Vector3 startPos = animator.transform.localPosition;
        Vector3 destPos = startPos + moveInterval;

        float elapsedTime = 0;

        while (elapsedTime < animationTime)
        {
            animator.transform.localPosition = Vector3.Lerp(startPos, destPos, (elapsedTime / animationTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator StartRotate(float angle)
    {
        Vector3 startRot = animator.transform.rotation.eulerAngles;
        Vector3 destRot = startRot + new Vector3(0.0f, angle, 0.0f);

        float elapsedTime = 0;

        while (elapsedTime < animationTime)
        {
            Vector3 nextRot = Vector3.Lerp(startRot, destRot, (elapsedTime / animationTime));
            animator.transform.localRotation = Quaternion.Euler(nextRot);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}