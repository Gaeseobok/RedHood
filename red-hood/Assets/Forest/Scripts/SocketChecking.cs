using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketChecking : MonoBehaviour
{
   
    XRSocketInteractor socket;
   
    void Start()
    {
        socket = GetComponent<XRSocketInteractor>();
    }
 
    public void socketCheck()
    {
       
        IXRSelectInteractable objName = socket.GetOldestInteractableSelected();
       
        Debug.Log(objName.transform.name + " in socket of " + transform.name);
    }

}
