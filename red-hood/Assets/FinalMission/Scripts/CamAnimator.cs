using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamAnimator : MonoBehaviour
{
    Animator ani;

    // Start is called before the first frame update
    void Start()
    {
        this.ani = GetComponent<Animator>();
    }

    public void ActivateCam()
    {
        ani.SetTrigger("CamStart");
    }


    public void DeactivateCam()
    {
        ani.SetTrigger("CamEnd");
    }
}
