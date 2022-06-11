using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_MainMenu : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] GameObject par_Managers;

    //private variables
    private Manager_UIReuse UIReuseScript;

    private void Awake()
    {
        UIReuseScript = par_Managers.GetComponent<Manager_UIReuse>();
        UIReuseScript.LoadMMContent();
    }

    public void LoadConnectionContent()
    {
        UIReuseScript.par_MainMenuContent.SetActive(false);
        UIReuseScript.par_MainMenuConnectionContent.SetActive(true);
        UIReuseScript.btn_ReturnToMM.gameObject.SetActive(true);
    }
    public void ReturnToMM()
    {
        UIReuseScript.Input_Create.text = "";
        UIReuseScript.Input_Join.text = "";

        UIReuseScript.par_MainMenuConnectionContent.SetActive(false);
        UIReuseScript.btn_ReturnToMM.gameObject.SetActive(false);
        UIReuseScript.par_MainMenuContent.SetActive(true);
    }
    public void Quit()
    {
        Application.Quit();
    }
}