using UnityEngine;

// �������� �ν��Ͻ�ȭ�Ѵ�.
public class ObjectInstantiation : MonoBehaviour
{
    [Tooltip("�ν��Ͻ�ȭ�� ������Ʈ�� ������ ��")]
    public GameObject ObjectModel;

    [Tooltip("������Ʈ�� �ν��Ͻ�ȭ�� ��ġ")]
    public Vector3 modelPosition;

    public void InstantiateObj()
    {
        Instantiate(ObjectModel, modelPosition, Quaternion.identity);
    }
}
