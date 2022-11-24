using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlNpc : MonoBehaviour
{
    public GameObject dialogueUI;
    public GameObject werewolf_head;

    public AudioSource shocked;
    
    public GameObject attackWolf;
    public GameObject nanaWolf;

    Vector3 talkingPosition = new Vector3(1.342f, 0.641f,-1.519f);
    Quaternion talkingRotation = Quaternion.Euler(0f, 180f, 0f);

    Quaternion headTurn = Quaternion.Euler(48.157f, 25.549f, -14.846f);

    Vector3 modifiedPosition = new Vector3(-3.335f, 0f, -1f);
    Quaternion modifiedRotation = Quaternion.Euler(0f, 190f, 0f);

    void Start()
    {
        AudioSource audio = GetComponent<AudioSource>();
    }

    public void MoveDialogue()
    {
        dialogueUI.transform.localPosition = talkingPosition;
        dialogueUI.transform.rotation = talkingRotation;
    }

    public void TurnHead()
    {
        werewolf_head.transform.localRotation = headTurn;
    }
    
    public void PlayShocked()
    {
        shocked.Play();
    }

    public void ActivateAttackWolf()
    {
        //Transform [] wolfMesh = attackWolf.GetComponentsInChildren<Transform>();
        Transform wolfMesh = attackWolf.transform.Find("WolfGuy");
        SkinnedMeshRenderer mesh = wolfMesh.gameObject.GetComponent<SkinnedMeshRenderer>();
        mesh.enabled = true;
    }

    public void DeactivateNanaWolf()
    {
        //Transform [] wolfMesh = nanaWolf.GetComponentsInChildren<Transform>();
        Transform wolfMesh = nanaWolf.transform.Find("WolfGuy");
        SkinnedMeshRenderer mesh = wolfMesh.gameObject.GetComponent<SkinnedMeshRenderer>();
        mesh.enabled = false;

        //Transform [] findHat = werewolf_head.GetComponentsInChildren<Transform>();
        Transform findHat = werewolf_head.transform.Find("Hat");
        findHat.gameObject.SetActive(false);
    }

    public void MovePlayer()
    {
        GameObject player = GameObject.Find("Camera Offset");
        player.transform.position = modifiedPosition;
        player.transform.rotation = modifiedRotation;
    }
}
