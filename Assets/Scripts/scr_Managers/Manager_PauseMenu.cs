using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Manager_PauseMenu : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] GameObject par_Managers;

    //public but hidden variables
    [HideInInspector] public bool isPMOpen;

    //private variables
    private Manager_UIReuse UIReuseScript;

    private void Start()
    {
        UIReuseScript = par_Managers.GetComponent<Manager_UIReuse>();
        UIReuseScript.LoadPMContent();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPMOpen = !isPMOpen;

            if (isPMOpen
                && !UIReuseScript.par_PauseMenu.activeInHierarchy)
            {
                OpenPauseMenu();
            }
            else if (!isPMOpen
                     && UIReuseScript.par_PauseMenu.activeInHierarchy)
            {
                ClosePauseMenu();
            }
        }
    }

    public void OpenPauseMenu()
    {
        UIReuseScript.par_PauseMenu.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ClosePauseMenu()
    {
        UIReuseScript.par_PauseMenu.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void ReturnToMainMenu()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("Scene_MainMenu");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}