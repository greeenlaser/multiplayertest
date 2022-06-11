using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class CreateAndJoinLobbies : MonoBehaviourPunCallbacks
{
    [Header("Scripts")]
    [SerializeField] private GameObject par_Managers;

    //private variables
    private Manager_UIReuse UIReuseScript;

    private void Awake()
    {
        UIReuseScript = par_Managers.GetComponent<Manager_UIReuse>();
    }

    public void CreateRoom()
    {
        CheckInputText(UIReuseScript.Input_Create.text, "create");
    }
    public void JoinRoom()
    {
        CheckInputText(UIReuseScript.Input_Join.text, "join");
    }
    private void CheckInputText(string roomCode, string callerType)
    {
        if (roomCode != ""
            && roomCode.Length <= 10)
        {
            if (callerType == "create")
            {
                PhotonNetwork.CreateRoom(roomCode);
            }
            else if (callerType == "join")
            {
                PhotonNetwork.JoinRoom(roomCode);
            }
        }
        else
        {
            Debug.LogWarning("Error: Invalid room code!");
        }
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Scene_Game");
    }
}