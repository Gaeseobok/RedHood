using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������Ʈ�� ��Ƽ������ ����Ѵ�.
// ���Ͽ� ȣ��(Hover Entered/Hover Exited)�� ������ ������ ����(��Ƽ����)�� �����ϱ� ���� ��ũ��Ʈ
public class ChangeMaterial : MonoBehaviour
{
    [Tooltip("����� ���Ͽ� Hover �Ǵ� Select�� ������ ��, �������� ��Ƽ����")]
    [SerializeField] private Material selectedMaterial;

    [Tooltip("����� Activate�� ��, �������� ��Ƽ����")]
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