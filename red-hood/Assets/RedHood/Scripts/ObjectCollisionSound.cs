using UnityEngine;

// ��ü�� �浹�� �� �Ҹ��� ����Ѵ�.
[RequireComponent(typeof(AudioSource))]
public class ObjectCollisionSound : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        audioSource.Play();
    }
}
