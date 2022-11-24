using UnityEngine;

public class TriggerAnimation : MonoBehaviour
{
    [Tooltip("������ ���� �Ǵ� ���� ������Ʈ�� �ִϸ�����")]
    [SerializeField] private Animator animator;

    public void TriggerTreeAnimation()
    {
        animator.GetComponent<AudioSource>().Play();
        animator.enabled = true;
    }

    public void TriggerWolfDeadAnimation()
    {
        animator.Play("Dead");
    }
}
