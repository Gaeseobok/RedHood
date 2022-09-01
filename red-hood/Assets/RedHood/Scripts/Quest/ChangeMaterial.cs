using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 오브젝트의 머티리얼을 토글한다.
// 소켓에 호버(Hover Entered/Hover Exited)할 때마다 소켓의 색상(머티리얼)을 변경하기 위한 스크립트
public class ChangeMaterial : MonoBehaviour
{
    [SerializeField] private Material anotherMaterial;
    private Material material;
    private MeshRenderer[] renderers;
    private bool isChanged = false;

    private void Start()
    {
        renderers = GetComponentsInChildren<MeshRenderer>();
        material = renderers[0].material;
    }

    private void SetMaterials(Material newMaterial)
    {
        foreach (MeshRenderer renderer in renderers)
            renderer.material = newMaterial;
    }

    public void Change()
    {
        if (isChanged)
            SetMaterials(material);
        else
            SetMaterials(anotherMaterial);

        isChanged = !isChanged;
    }
}