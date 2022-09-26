using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 오브젝트의 머티리얼을 토글한다.
// 소켓에 호버(Hover Entered/Hover Exited)할 때마다 소켓의 색상(머티리얼)을 변경하기 위한 스크립트
public class ChangeMaterial : MonoBehaviour
{
    [Tooltip("블록이 소켓에 Hover 또는 Select된 상태일 때, 포인터의 머티리얼")]
    [SerializeField] private Material selectedMaterial;

    [Tooltip("블록이 Activate될 때, 포인터의 머티리얼")]
    [SerializeField] private Material activatedMaterial;

    private Material defaultMaterial;
    private MeshRenderer [] renderers;
    private bool isChanged = false;
       
    private void Start()
    {
        renderers = GetComponentsInChildren<MeshRenderer>();
        defaultMaterial = renderers[0].material;
    }

    private void SetMaterials(Material newMaterial)
    {
        foreach (MeshRenderer renderer in renderers)
            renderer.material = newMaterial;
    }

    public void ChangeToDefaultMaterial()
    {
        SetMaterials(defaultMaterial);
    }

    public void ChangeToSelectedMaterial()
    {
        if (isChanged)
            SetMaterials(defaultMaterial);
        else
            SetMaterials(selectedMaterial);

        isChanged = !isChanged;
    }

    public void ChangeToActivatedMaterial()
    {
        SetMaterials(activatedMaterial);
    }
}