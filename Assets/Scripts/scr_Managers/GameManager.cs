using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    [Header("Game version")]
    public string str_GameVersion;
    [SerializeField] private TMP_Text txt_GameVersion;

    [Header("Framerate")]
    [SerializeField] private TMP_Text txt_fpsValue;

    //public but hidden variables
    [HideInInspector] public PhotonView view;

    //private variables
    private float timer;
    private float deltaTime;

    private void Awake()
    {
        txt_GameVersion.text = str_GameVersion;

        Screen.SetResolution(1920, 1080, FullScreenMode.ExclusiveFullScreen);
        QualitySettings.vSyncCount = 0;

        view = gameObject.AddComponent<PhotonView>();
    }

    private void Update()
    {
        if (view.IsMine)
        {
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
            float msec = Mathf.FloorToInt(deltaTime * 1000.0f);
            float fps = Mathf.FloorToInt(1.0f / deltaTime);

            timer += Time.unscaledDeltaTime;
            if (timer > 0.1f)
            {
                txt_fpsValue.text = fps + " (" + msec + ")";
                timer = 0;
            }

            if (Input.GetKeyDown(KeyCode.F12))
            {
                Screenshot();
            }
        }
    }

    //press F12 for screenshot
    private void Screenshot()
    {
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\LightsOff\Screenshots";

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        var screenshotName = "Screenshot_" + System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + ".png";
        ScreenCapture.CaptureScreenshot(System.IO.Path.Combine(folderPath, screenshotName), 4);
    }
}