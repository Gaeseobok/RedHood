using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// 오브젝트의 머티리얼을 변경한다.
public class ChangeMaterial : MonoBehaviour
{
    [SerializeField] private Material hoveredMaterial;
    [SerializeField] private Material selectedMaterial;

    private XRSimpleInteractable interactable;

    private Material[] defaultMaterials;
    private MeshRenderer[] renderers;

    private void Start()
    {
        interactable = GetComponent<XRSimpleInteractable>();
        renderers = GetComponentsInChildren<MeshRenderer>(true);
        defaultMaterials = new Material[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            defaultMaterials[i] = renderers[i].material;
        }
    }
    public void ToggleMaterials()
    {
        if (interactable.isSelected)
        {
            SetMaterials(selectedMaterial);
        }
        else if (interactable.isHovered)
        {
            SetMaterials(hoveredMaterial);
        }
        else
        {
            RevertMaterials();
        }
    }

    private void SetMaterials(Material material)
    {
        foreach (MeshRenderer renderer in renderers)
        {
            renderer.material = material;
        }
    }

    private void RevertMaterials()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = defaultMaterials[i];
        }
    }
}