using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 프리팹을 인스턴스화한다.
public class AddIngredient : MonoBehaviour
{
    public GameObject ingredientModel;
    public Vector3 modelPosition;

    public void Add()
    {
        Instantiate(ingredientModel, modelPosition, Quaternion.identity);
    }
}