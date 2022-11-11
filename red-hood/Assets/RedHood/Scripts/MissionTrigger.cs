using UnityEngine;

// �÷��̾ Ư�� ������ ������ �̼��� �����Ѵ�.
public class MissionTrigger : MonoBehaviour
{
    [SerializeField] private GameObject missionObject;

    private ParticleSystem particle;

    private const string PLAYER_TAG = "Player";

    private void Start()
    {
        particle = GetComponentInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG))
        {
            particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            missionObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG))
        {
            particle.Play();
            missionObject.SetActive(false);
        }
    }
}
