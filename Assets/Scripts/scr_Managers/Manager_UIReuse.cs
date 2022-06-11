using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Manager_UIReuse : MonoBehaviour
{
    [Header("Main menu")]
    public GameObject par_MainMenuContent;
    public GameObject par_MainMenuConnectionContent;
    public Button btn_LoadConnectionContent;
    public Button btn_ReturnToMM;
    public Button btn_QuitGameFromMM;
    [SerializeField] private Manager_MainMenu MainMenuManagerScript;

    [Header("Main menu connection")]
    public TMP_InputField Input_Create;
    public TMP_InputField Input_Join;

    [Header("Pause menu")]
    public GameObject par_PauseMenu;
    public Button btn_ReturnToGame;
    public Button btn_ReturnToMainMenu;
    public Button btn_QuitGameFromPM;
    [SerializeField] private Manager_PauseMenu PauseMenuScript;

    public void LoadMMContent()
    {
        par_MainMenuContent.SetActive(true);
        par_MainMenuConnectionContent.SetActive(false);

        //reset mm buttons
        btn_LoadConnectionContent.onClick.RemoveAllListeners();
        btn_LoadConnectionContent.onClick.AddListener(MainMenuManagerScript.LoadConnectionContent);
        btn_LoadConnectionContent.gameObject.SetActive(true);

        btn_ReturnToMM.onClick.RemoveAllListeners();
        btn_ReturnToMM.onClick.AddListener(MainMenuManagerScript.ReturnToMM); ;
        btn_ReturnToMM.gameObject.SetActive(false);

        btn_QuitGameFromMM.onClick.RemoveAllListeners();
        btn_QuitGameFromMM.onClick.AddListener(MainMenuManagerScript.Quit);
        btn_QuitGameFromMM.gameObject.SetActive(true);
    }

    public void LoadPMContent()
    {
        btn_ReturnToGame.onClick.RemoveAllListeners();
        btn_ReturnToGame.onClick.AddListener(PauseMenuScript.ClosePauseMenu);

        btn_ReturnToMainMenu.onClick.RemoveAllListeners();
        btn_ReturnToMainMenu.onClick.AddListener(PauseMenuScript.ReturnToMainMenu); ;

        btn_QuitGameFromPM.onClick.RemoveAllListeners();
        btn_QuitGameFromPM.onClick.AddListener(PauseMenuScript.QuitGame);

        PauseMenuScript.ClosePauseMenu();
    }
}