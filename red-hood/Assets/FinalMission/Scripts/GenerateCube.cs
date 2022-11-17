using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using Random=UnityEngine.Random;

public class GenerateCube : MonoBehaviour
{
    public GameObject Sockets;
    internal XRSocketInteractor [] SocketAttach;

    int socketIndex = 0;
    // int colliderIndex = 0;
    // float time = 0;

    // answer saved
    bool [] answers;

    // Cubes to be generated
    internal Vector3 socketPosition;
    internal Quaternion defaultDegree = Quaternion.Euler(15,12,0);

    public Canvas errorMessages;
    public Canvas successMessages;

    Image [] errorWindows;
    Image [] successWindows;

    XRGrabInteractable [] generatedCubes;

    private const string ERROR_MESSAGE = "Error Message";
    private const string SUCCESS_MESSAGE = "Success Message";

    // public GameObject posCube;
    // public float speed;
    // public Transform [] colliders;

    public GameObject upObstacle;
    public GameObject downObstacle;
    public GameObject rightObstacle;
    public GameObject leftObstacle;

    // Start is called before the first frame update
    void Awake()
    {   
        SocketAttach = Sockets.GetComponentsInChildren<XRSocketInteractor>();
        answers = new bool [8];

        errorWindows = errorMessages.GetComponentsInChildren<Image>();
        successWindows = successMessages.GetComponentsInChildren<Image>();

        foreach (Image errorwindow in errorWindows)
            errorwindow.enabled = false;

        foreach (Image successWindow in successWindows)
            successWindow.enabled = false;

        generatedCubes = new XRGrabInteractable [8];
    }

    void Update()
    {
        if(socketIndex == 8)
        {
            ActivateAnswer();
            socketIndex++;
        }
    }
    
    // void MoveToObstacle(Transform obstacle)
    // {
    //     float step = speed * Time.deltaTime;
    //     cameraOffset.transform.position = Vector3.MoveTowards(cameraOffset.transform.position, obstacle.position, step);

    //     if(cameraOffset.transform.position == obstacle.position)
    //     {
    //         ActivateAnswer(colliderIndex); 
    //         colliderIndex++; 
    //     }
    // }

    public void InstCube(XRGrabInteractable DirectionCube)
    {
        if(socketIndex<=7)
        {
            Debug.Log("socketIndex : " + socketIndex);

            socketPosition = SocketAttach[socketIndex].transform.position;
            Debug.Log(socketIndex + " Instantiated ");
            XRGrabInteractable newCube = Instantiate(DirectionCube, socketPosition, defaultDegree);
            newCube.transform.localScale = new Vector3(0.18f,0.18f,0.18f);
            newCube.tag = DirectionCube.tag;

            SaveAnswer(socketIndex, newCube);
            generatedCubes[socketIndex] = newCube;

            socketIndex++;
        }
    }

    void SaveAnswer(int socketIndex, XRGrabInteractable newCube)
    {
        if(SocketAttach[socketIndex].CompareTag(newCube.tag))
        {
            answers[socketIndex] = true;
            Debug.Log(socketIndex + " : " + SocketAttach[socketIndex].tag + " & "+ newCube.tag + " -> " + answers[socketIndex]);
        }
        else
        {
            answers[socketIndex] = false;
            Debug.Log(socketIndex + " : " + SocketAttach[socketIndex].tag + " & "+ newCube.tag + " -> " + answers[socketIndex]);
        }
    }

    void ActivateAnswer()
    {
        for(int i = 0; i<8; i++)
        {
            if(answers[i])
            {
                Debug.Log(i + "answer correct");
                successWindows[i].enabled = true;
            }
            else
            {
                Debug.Log(i + "answer wrong");
                errorWindows[i].enabled = true;
                StartCoroutine(CamShake(2.0f, 4.0f));
            }

            if(SocketAttach[i].CompareTag("Up"))
            {
                ObstacleUp();
            }
            else if(SocketAttach[i].CompareTag("Down"))
            {
                ObstacleDown();
            }
            else if(SocketAttach[i].CompareTag("Left"))
            {
                ObstacleLeft();
            }
            else if(SocketAttach[i].CompareTag("Right"))
            {
                ObstacleRight();
            }

            StartCoroutine(MakeDelay());
        }
    }

    IEnumerator CamShake(float duration, float magnitude)
    {
        Vector3 orignalPosition = transform.position;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            Camera.main.transform.position = new Vector3(x, y, -10f);
            elapsed += Time.deltaTime;
            yield return 0;
        }
        Camera.main.transform.position = orignalPosition;
    }
    
    // private void OnTriggerEnter(Collider other)
    // {
    //     colliderIndex++;
    //     ActivateAnswer(colliderIndex);
    //     Debug.Log(colliderIndex + "Collided");
    // }

    IEnumerator MakeDelay()
    {
        yield return new WaitForSeconds(30f);
    }

    public void ObstacleUp()
    {
        Vector3 cameraPosition = Camera.main.transform.position;
        cameraPosition += new Vector3(2f,-2f,2f);
        Quaternion obstacleRotation = Quaternion.Euler(-17.718f, 109.998f, 0);
        GameObject obstacle = Instantiate(upObstacle, cameraPosition, obstacleRotation);
        obstacle.transform.localScale = new Vector3(1.5f,0.7f,1.5f);
        Destroy(obstacle, 10.0f);
    }

    public void ObstacleDown()
    {
        Vector3 cameraPosition = Camera.main.transform.position;
        cameraPosition += new Vector3(1.5f,-0.8f,2f);
        Quaternion obstacleRotation = Quaternion.Euler(0f, 0f, 90f);
        GameObject obstacle = Instantiate(downObstacle, cameraPosition, obstacleRotation);
        obstacle.transform.localScale = new Vector3(1.3f,1.3f,1.3f);
        Destroy(obstacle, 10.0f);
    }

    public void ObstacleLeft()
    {
        Vector3 cameraPosition = Camera.main.transform.position;
        cameraPosition += new Vector3(-0.8f,-1.6f,2f);
        Quaternion obstacleRotation = Quaternion.Euler(0f, 180f, 0f);
        GameObject obstacle = Instantiate(leftObstacle, cameraPosition, obstacleRotation);
        obstacle.transform.localScale = new Vector3(1f, 1f, 1f);
        Destroy(obstacle, 10.0f);
    }

    public void ObstacleRight()
    {
        Vector3 cameraPosition = Camera.main.transform.position;
        cameraPosition += new Vector3(1.1f,-1.6f,2f);
        Quaternion obstacleRotation = Quaternion.Euler(0f, 90f,0f);
        GameObject obstacle = Instantiate(rightObstacle, cameraPosition, obstacleRotation);
        obstacle.transform.localScale = new Vector3(1f, 1f, 1f);
        Destroy(obstacle, 50.0f);
    }
}
