using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(AudioSource))]
public class SoundActivation : MonoBehaviour
{
    //[Tooltip("����� ���� �� ����Ǵ� ����� Ŭ��")]
    //[SerializeField] private AudioClip holdingSound;

    [Tooltip("����� ������ �� ����Ǵ� ����� Ŭ��")]
    [SerializeField] private AudioClip attachingSound;

    [Tooltip("����� ������ �� ����Ǵ� ����� Ŭ��")]
    [SerializeField] private AudioClip activatedSound;

    private AudioSource audioSource;
    private BlockActivation blockActivation;
    private XRSocketInteractor socket;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        blockActivation = GetComponent<BlockActivation>();
    }

    //public void PlayHoldingSound()
    //{
    //    audioSource.clip = holdingSound;
    //    audioSource.Play();
    //}

    private void PlayAttachingSound()
    {
        audioSource.clip = attachingSound;
        audioSource.Play();
    }

    public void OnBlockAttached()
    {
        BlockActivation attachedBlock = blockActivation.GetNextBlock();
        if (attachedBlock != null)
        {
            attachedBlock.GetComponent<SoundActivation>().PlayAttachingSound();
        }
    }

    public void PlayActivatedSound()
    {
        audioSource.clip = activatedSound;
        audioSource.Play();
    }
}
