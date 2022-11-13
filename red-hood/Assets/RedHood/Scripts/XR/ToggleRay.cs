using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// Direct interactor�� Ray interactor�� ����Ѵ�.
// Direct interactor�� �ٸ� ������Ʈ�� ��ȣ�ۿ��ϰ� ���� ���� ���� ����Ѵ�.
public class ToggleRay : MonoBehaviour
{
    [SerializeField] private XRDirectInteractor directInteractor;
    [SerializeField] private PhysicsHandMovement physicsHand;
    private XRRayInteractor rayInteractor;
    private SpriteRenderer reticleRenderer;
    private bool isSwitched;

    private void Awake()
    {
        rayInteractor = GetComponent<XRRayInteractor>();
        //reticleRenderer = GetComponent<XRInteractorLineVisual>().reticle.GetComponentInChildren<SpriteRenderer>();
        SwitchInteractors(false);
    }

    public void ActivateRay()
    {
        if (!TouchingObject())
        {
            SwitchInteractors(true);
            physicsHand.DisableHand();
        }
    }

    public void DeactivateRay()
    {
        if (isSwitched)
        {
            SwitchInteractors(false);
            physicsHand.EnableHandWithDelay(0.0f);
        }
    }

    public bool TouchingObject()
    {
        List<IXRInteractable> targets = new();
        directInteractor.GetValidTargets(targets);
        return targets.Count > 0;
    }

    private void SwitchInteractors(bool value)
    {
        isSwitched = value;
        rayInteractor.enabled = value;
        //reticleRenderer.enabled = value;
        directInteractor.enabled = !value;
    }
}