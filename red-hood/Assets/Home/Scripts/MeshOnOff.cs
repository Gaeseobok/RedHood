using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MeshOnOff : MonoBehaviour
{
    public GameObject mission;

    // Start is called before the first frame update
    void Awake()
    {
        TurnOffChildMesh(mission);
    }

    private void TurnOffChildMesh(GameObject obj){
        if (obj == null)
        {
            return;
        }

        foreach (Transform child in obj.transform){
            if (child == null)
                continue;

            try
            {
                MeshRenderer ms = child.GetComponent<MeshRenderer>();
                ms.enabled = false;
            }
            catch (System.Exception)
            {
                try
                {
                    if(child.GetComponent<TextMeshProUGUI>())
                    {
                        TextMeshProUGUI ms = child.GetComponent<TextMeshProUGUI>();
                        ms.enabled = false;
                    }
                    else if(child.GetComponent<Image>())
                    {
                        Image ms = child.GetComponent<Image>();
                        ms.enabled = false;
                    }
                }
                catch (NullReferenceException)
                {
                    continue;
                }
            }
            TurnOffChildMesh(child.gameObject);
        }
    }

    public void TurnOnChildMesh(GameObject obj){
        if (obj == null)
        {
            return;
        }

        foreach (Transform child in obj.transform){
            if (child == null)
                continue;

            try
            {
                MeshRenderer ms = child.GetComponent<MeshRenderer>();
                ms.enabled = true;
            }
            catch (System.Exception)
            {
                try
                {
                    if(child.GetComponent<TextMeshProUGUI>())
                    {
                        TextMeshProUGUI ms = child.GetComponent<TextMeshProUGUI>();
                        ms.enabled = true;
                    }
                    else if(child.GetComponent<Image>())
                    {
                        Image ms = child.GetComponent<Image>();
                        ms.enabled = true;
                    }
                }
                catch (NullReferenceException)
                {
                    continue;
                }
            }
            TurnOnChildMesh(child.gameObject);
        }
    }

}
