using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(AudioSource))]
public class SoundActivation : MonoBehaviour
{
    //[Tooltip("블록을 잡을 때 재생되는 오디오 클립")]
    //[SerializeField] private AudioClip holdingSound;

    [Tooltip("블록을 부착할 때 재생되는 오디오 클립")]
    [SerializeField] private AudioClip attachingSound;

    [Tooltip("블록을 실행할 때 재생되는 오디오 클립")]
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
