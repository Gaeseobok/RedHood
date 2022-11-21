using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalMission : MonoBehaviour
{
    [Tooltip("큐브 하나가 실행되는 시간")]
    [SerializeField] private float cubeDelay = 5.0f;

    [Tooltip("첫 번째 장애물이 등장할 때까지의 지연 시간")]
    [SerializeField] private float obstacleActiveDelay = 15.0f;

    private Animator cameraAnimator;
    private BoxCollider[] attachTransforms;

    private static List<GameObject> cubes;

    private const string SUCCESS_ALERT = "SuccessAlert";
    private const string FAILURE_ALERT = "FailureAlert";

    private const string LEFT = "Left";
    private const string RIGHT = "Right";
    private const string UP = "Up";
    private const string DOWN = "Down";

    private void Start()
    {
        cameraAnimator = Camera.main.GetComponent<Animator>();
        attachTransforms = GetComponentsInChildren<BoxCollider>();
        cubes = new List<GameObject>();
        gameObject.SetActive(false);
        StartMission();
    }

    public void StartMission()
    {
        gameObject.SetActive(true);
        Invoke(nameof(ExecuteBlocks), obstacleActiveDelay);
    }

    private void TriggerCameraAnim(string name)
    {
        if (name.StartsWith(LEFT))
        {
            cameraAnimator.Play(LEFT);
        }
        else if (name.StartsWith(RIGHT))
        {
            cameraAnimator.Play(RIGHT);
        }
        else if (name.StartsWith(UP))
        {
            cameraAnimator.Play(UP);
        }
        else
        {
            cameraAnimator.Play(DOWN);
        }
    }

    private bool CompareName(string obstacleName, string cubeName)
    {
        return obstacleName.StartsWith(cubeName[0]);
    }

    private IEnumerator CheckAnswers()
    {
        for (int i = 0; i < attachTransforms.Length; i++)
        {
            Transform attachTransform = attachTransforms[i].transform;
            GameObject successAlert = attachTransform.Find(SUCCESS_ALERT).gameObject;
            GameObject failureAlert = attachTransform.Find(FAILURE_ALERT).gameObject;

            if (cubes.Count <= i)
            {
                failureAlert.SetActive(true);
                failureAlert.GetComponent<AudioSource>().Play();
            }
            else if (!CompareName(attachTransform.name, cubes[i].name))
            {
                TriggerCameraAnim(cubes[i].name);
                failureAlert.SetActive(true);
                failureAlert.GetComponent<AudioSource>().Play();
            }
            else
            {
                TriggerCameraAnim(cubes[i].name);
                successAlert.SetActive(true);
                successAlert.GetComponent<AudioSource>().Play();
            }

            yield return new WaitForSeconds(cubeDelay);
        }
    }

    private void ExecuteBlocks()
    {
        StopAllCoroutines();
        _ = StartCoroutine(CheckAnswers());
    }

    internal void AddCube(GameObject cube)
    {
        cubes.Add(cube);
    }
}
