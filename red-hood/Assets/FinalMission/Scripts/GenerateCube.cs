using UnityEngine;

public class GenerateCube : MonoBehaviour
{
    private FinalMission finalMission;
    private BoxCollider[] attachTransforms;
    private static int attachIndex;

    private const int MAX_INDEX = 8;

    private void Start()
    {
        finalMission = GetComponent<FinalMission>();
        attachTransforms = GetComponentsInChildren<BoxCollider>();
        attachIndex = 0;
    }

    public void InstantiateCube(GameObject Cube)
    {
        if (attachIndex < MAX_INDEX)
        {
            Transform cubeTransform = attachTransforms[attachIndex++].transform;
            GameObject cube = Instantiate(Cube, cubeTransform);
            cube.transform.localPosition = Vector3.zero;
            finalMission.AddCube(cube);
        }
    }
}
