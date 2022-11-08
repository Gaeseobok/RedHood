using UnityEngine;

// 프리팹을 인스턴스화한다.
public class ObjectInstantiation : MonoBehaviour
{
    [Tooltip("인스턴스화할 오브젝트의 프리팹 모델")]
    public GameObject ObjectModel;

    [Tooltip("오브젝트를 인스턴스화할 위치")]
    public Vector3 modelPosition;

    public void InstantiateObj()
    {
        Instantiate(ObjectModel, modelPosition, Quaternion.identity);
    }
}
