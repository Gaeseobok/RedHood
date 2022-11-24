using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalMission : MonoBehaviour
{
    private Coroutine CurrentRoutine { set; get; } = null;


    [Tooltip("큐브 하나가 실행되는 시간")]
    [SerializeField] private float cubeDelay = 5.0f;

    [Tooltip("첫 번째 장애물이 등장할 때까지의 지연 시간")]
    [SerializeField] private float obstacleActiveDelay = 20.0f;

    [Tooltip("설명창 오브젝트")]
    [SerializeField] private GameObject descWindow;

    [Tooltip("타이머 오디오 소스")]
    [SerializeField] private AudioSource timer;

    [Tooltip("카메라 오프셋 오브젝트")]
    [SerializeField] private Animator XRAnimator;

    [Tooltip("맵 오브젝트")]
    [SerializeField] private Animator mapAnimator;
    private Animator treeAnimator;

    [Tooltip("늑대 오브젝트")]
    [SerializeField] private Transform wolfObject;

    [Tooltip("버튼 오브젝트")]
    [SerializeField] private GameObject buttonObject;

    [Tooltip("게임오버 팝업창 오브젝트")]
    [SerializeField] private GameObject failureWindow;

    private BoxCollider[] attachTransforms;
    private PopUpMessage popUpMessage;
    private FadeCanvas fadeCanvas;

    // 유저가 입력한 방향 큐브 리스트
    private static readonly List<GameObject> cubes = new();

    private const string QUEST_MODEL = "QuestModel";

    private const string SUCCESS_ALERT = "SuccessAlert";
    private const string FAILURE_ALERT = "FailureAlert";

    private const string LEFT = "Left";
    private const string RIGHT = "Right";
    private const string UP = "Up";
    private const string DOWN = "Down";

    private void Start()
    {
        attachTransforms = GetComponentsInChildren<BoxCollider>();
        popUpMessage = GetComponent<PopUpMessage>();
        fadeCanvas = FindObjectOfType<FadeCanvas>();

        ResetMission();
    }

    private void ResetMission()
    {
        GameObject[] questModels = GameObject.FindGameObjectsWithTag(QUEST_MODEL);
        foreach (GameObject questModel in questModels)
        {
            Destroy(questModel);
        }
        cubes.Clear();

        timer.Stop();
        //descWindow.SetActive(true);
        buttonObject.SetActive(false);
        wolfObject.gameObject.SetActive(false);
        wolfObject.localPosition = new Vector3(-0.5f, 0f, -12f);
        XRAnimator.enabled = false;
        XRAnimator.transform.localPosition = Vector3.zero;
        XRAnimator.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
        gameObject.SetActive(false);
    }

    public void StartMission()
    {
        gameObject.SetActive(true);
        timer.Play();
        descWindow.SetActive(false);
        buttonObject.SetActive(true);
        wolfObject.gameObject.SetActive(true);
        XRAnimator.enabled = true;

        Invoke(nameof(ExecuteBlocks), obstacleActiveDelay);
    }

    private void TriggerMapAnim(string name)
    {
        if (name.StartsWith(LEFT))
        {
            mapAnimator.Play(LEFT);
        }
        else if (name.StartsWith(RIGHT))
        {
            mapAnimator.Play(RIGHT);
        }
        else if (name.StartsWith(UP))
        {
            mapAnimator.Play(UP);
        }
        else
        {
            mapAnimator.Play(DOWN);
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
                wolfObject.localPosition += new Vector3(0f, 0f, 3.0f);
            }
            else if (!CompareName(attachTransform.name, cubes[i].name))
            {
                TriggerMapAnim(cubes[i].name);
                failureAlert.SetActive(true);
                failureAlert.GetComponent<AudioSource>().Play();
                wolfObject.localPosition += new Vector3(0f, 0f, 3.0f);
            }
            else
            {
                TriggerMapAnim(cubes[i].name);
                successAlert.SetActive(true);
                successAlert.GetComponent<AudioSource>().Play();
            }

            if (wolfObject.localPosition.z > -3.0f)
            {
                GetComponent<AudioSource>().Play();

                fadeCanvas.StartFadeIn();
                yield return fadeCanvas.CurrentRoutine;

                ResetMission();
                failureWindow.SetActive(true);
                Invoke(nameof(ReloadScene), 5.0f);

                fadeCanvas.StartFadeOut();
                yield return fadeCanvas.CurrentRoutine;

                break;
            }

            yield return new WaitForSeconds(cubeDelay);
        }
    }

    private void ReloadScene()
    {
        StopAllCoroutines();
        SceneManager.LoadScene(gameObject.scene.name);
    }

    private void ExecuteBlocks()
    {
        StopAllCoroutines();
        CurrentRoutine = StartCoroutine(CheckAnswers());
    }

    internal void AddCube(GameObject cube)
    {
        cubes.Add(cube);
    }
}
