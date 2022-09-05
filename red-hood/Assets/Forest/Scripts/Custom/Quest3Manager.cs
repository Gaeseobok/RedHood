using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class Quest3Manager : MonoBehaviour
{
    private GameObject[] snowman_acc;
    private GameObject successWindow;
    private GameObject codingBoard;

    private XRSocketInteractor CodeSocket1;
    private XRSocketInteractor CodeSocket2;
    private GameObject codeBlock1;
    private GameObject codeBlock2;

    IXRSelectInteractable compareBlock;
    

    void Awake()
    {   
        // List<XRSocketInteractor> CodeSockets = GameObject.FindGameObjectsWithTag("CodeSocket").Select(obj => obj.GetComponent<XRSocketInteractor>()).ToList();
        
        // compareBlock = CodeSocket1.GetOldestInteractableSelected();
        // if((compareBlock.transform.name).Equals("CodeBlock1"))
        // {
        //     compareBlock = CodeSockets2.GetOldestInteractableSelected();
        //     if((compareBlock.transform.name).Equals("CodeBlock2"))
        //     {
        //         codingBoard = GameObject.Find("CodingBoard");
        //         codingBoard.SetActive(false);
        //     }
        // }

        CodeSocket1 = GetComponent<XRSocketInteractor>();
        CodeSocket1.selectEntered.AddListener((SelectEnterEventArgs obj)=>
                            {codingBoard = GameObject.Find("CodingBoard");
                            codingBoard.SetActive(false);

                            // var cubeRenderer = GameObject.Find("CodeBlock1").GetComponent<Renderer>();
                            // cubeRenderer.material.SetColor("_Color", Color.blue);

                            snowman_acc = FindInActiveObjectsByTag("SnowmanAcc");
                            EnableSnowmanAcc(snowman_acc);

                            successWindow = FindInActiveObjectByName("SuccessWindow");
                            successWindow.SetActive(true);

                            codeBlock1 = GameObject.Find("CodeBlock1");
                            codeBlock1.SetActive(false);
                            });

        // if (Convert.ToBoolean(CodeSocket1.selectEntered))
        // {
        //     compareBlock = Convert.ToString(CodeSocket1.interactablesSelected);
        // } 

    }

     public void EnableSnowmanAcc(GameObject[] objs)
    {
        foreach(GameObject obj in objs)
        {
            obj.SetActive(true);
            
        }
    }

    GameObject FindInActiveObjectByName(string name)
    {
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].name == name)
                {
                    return objs[i].gameObject;
                }
            }
        }
        return null;
    }
    
    GameObject FindInActiveObjectByTag(string tag)
    {
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].CompareTag(tag))
                {
                    return objs[i].gameObject;
                }
            }
        }
        return null;
    }

    GameObject[] FindInActiveObjectsByTag(string tag)
    {
        List<GameObject> validTransforms = new List<GameObject>();
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].gameObject.CompareTag(tag))
                {
                    validTransforms.Add(objs[i].gameObject);
                }
            }
        }
        return validTransforms.ToArray();
    }
}
