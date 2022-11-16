using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SoundActivation : MonoBehaviour
{
    [Tooltip("블록을 부착할 때 재생되는 오디오 클립")]
    [SerializeField] private AudioClip attachingSound;

    [Tooltip("블록을 실행할 때 재생되는 오디오 클립")]
    [SerializeField] private AudioClip activatedSound;

    private AudioSource audioSource;
    private AudioClip collisionSound;
    private XRSocketInteractor socket;

    private void Start()
    {
        if (TryGetComponent(out audioSource))
        {
            collisionSound = audioSource.clip;
        }
        socket = GetComponent<XRSocketInteractor>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 코드 블록 간의 충돌에는 효과음을 재생하지 않는다
        if ((GetComponent<DestroyUnselectedBlock>() != null &&
            collision.gameObject.GetComponent<DestroyUnselectedBlock>() != null) ||
            collision.gameObject.CompareTag("CodingZone") || collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        // 효과음 재생
        if (audioSource != null)
        {
            audioSource.clip = collisionSound;
            audioSource.PlayOneShot(audioSource.clip, 0.2f);
        }
    }

    private void PlayAttachingSound()
    {
        if (audioSource != null)
        {
            audioSource.clip = attachingSound;
            audioSource.Play();
        }
    }

    public void OnBlockAttached()
    {
        if (socket != null)
        {
            IXRSelectInteractable attachedBlock = socket.firstInteractableSelected;
            if (attachedBlock != null)
            {
                ((XRGrabInteractable)attachedBlock).GetComponent<SoundActivation>().PlayAttachingSound();
            }
        }
    }

    public void PlayActivatedSound()
    {
        if (audioSource != null)
        {
            audioSource.clip = activatedSound;
            audioSource.Play();
        }
    }
}
