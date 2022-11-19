using UnityEngine;

// 플레이어가 특정 구역에 들어오면 미션을 시작한다.
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
