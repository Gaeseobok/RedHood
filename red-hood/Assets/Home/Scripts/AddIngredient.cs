using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �������� �ν��Ͻ�ȭ�Ѵ�.
public class AddIngredient : MonoBehaviour
{
    public GameObject ingredientModel;
    public Vector3 modelPosition;

    public void Add()
    {
        Instantiate(ingredientModel, modelPosition, Quaternion.identity);
    }
}