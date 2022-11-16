using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SoundActivation : MonoBehaviour
{
    [Tooltip("����� ������ �� ����Ǵ� ����� Ŭ��")]
    [SerializeField] private AudioClip attachingSound;

    [Tooltip("����� ������ �� ����Ǵ� ����� Ŭ��")]
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
        // �ڵ� ��� ���� �浹���� ȿ������ ������� �ʴ´�
        if ((GetComponent<DestroyUnselectedBlock>() != null &&
            collision.gameObject.GetComponent<DestroyUnselectedBlock>() != null) ||
            collision.gameObject.CompareTag("CodingZone") || collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        // ȿ���� ���
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
