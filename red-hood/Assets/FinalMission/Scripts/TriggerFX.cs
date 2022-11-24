using UnityEngine;

public class TriggerFX : MonoBehaviour
{
    private ParticleSystem particle;

    private void Start()
    {
        particle = GetComponentInChildren<ParticleSystem>();
    }

    public void PlayParticle()
    {
        particle.Play();
    }
}
