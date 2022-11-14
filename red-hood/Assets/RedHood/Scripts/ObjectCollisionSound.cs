using UnityEngine;

// 물체가 충돌할 때 소리를 재생한다.
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
