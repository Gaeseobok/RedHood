using UnityEngine;

public class TriggerAnimation : MonoBehaviour
{
    [Tooltip("쓰러질 나무 또는 늑대 오브젝트의 애니메이터")]
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
