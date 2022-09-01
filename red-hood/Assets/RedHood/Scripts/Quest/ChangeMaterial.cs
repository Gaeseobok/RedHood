using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������Ʈ�� ��Ƽ������ ����Ѵ�.
// ���Ͽ� ȣ��(Hover Entered/Hover Exited)�� ������ ������ ����(��Ƽ����)�� �����ϱ� ���� ��ũ��Ʈ
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