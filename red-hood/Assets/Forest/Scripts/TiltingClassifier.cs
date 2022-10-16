using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiltingClassifier : MonoBehaviour
{
    public GameObject classifier;
    public Vector3 rotationDegree;
    private Quaternion initialDegree;

    public void Start() 
    {
        initialDegree = classifier.transform.rotation;
    }

    public void Tilt()
    {
        classifier.transform.Rotate(rotationDegree);
    }

    public void InitialPosition()
    {
        classifier.transform.rotation = initialDegree;
    }
}
