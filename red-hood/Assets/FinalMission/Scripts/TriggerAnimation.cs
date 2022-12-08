using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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
        StopAllCoroutines();
        _ = StartCoroutine(LoadEnding());
    }

    private IEnumerator LoadEnding()
    {
        yield return new WaitForSeconds(10);
        SceneManager.LoadScene(4);
    }
}
