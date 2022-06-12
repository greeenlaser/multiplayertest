using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class Player_Camera : MonoBehaviour
{
    [Header("Assignables")]
    [SerializeField] private Slider slider_MouseSpeed;
    [SerializeField] private Slider slider_FOV;
    [SerializeField] private TMP_Text txt_MouseSpeed;
    [SerializeField] private TMP_Text txt_FOV;

    [Header("Scripts")]
    [SerializeField] private GameObject par_Managers;

    //public but hidden variables
    [HideInInspector] public bool isCamEnabled;
    [HideInInspector] public float mouseSpeed = 40;
    [HideInInspector] public int fov = 90;

    //private variables
    private float sensX;
    private float sensY;
    private float mouseX;
    private float mouseY;
    private float xRot;
    private PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        view.RequestOwnership();

        gameObject.GetComponent<Camera>().fieldOfView = fov;
        sensX = mouseSpeed;
        sensY = mouseSpeed;
        txt_MouseSpeed.text = mouseSpeed.ToString();
        txt_FOV.text = gameObject.GetComponent<Camera>().fieldOfView.ToString();

        slider_MouseSpeed.onValueChanged.AddListener(delegate { SetMouseSpeed(); });
        slider_FOV.onValueChanged.AddListener(delegate { SetFOV(); });

        StartCoroutine(Wait());
    }

    private void Update()
    {
        if (isCamEnabled
            && view.IsMine)
        {
            mouseX = Input.GetAxis("Mouse X") * sensX * 5 * Time.deltaTime;
            mouseY = Input.GetAxis("Mouse Y") * sensY * 5 * Time.deltaTime;

            xRot -= mouseY;

            xRot = Mathf.Clamp(xRot, -90f, 90f);
            transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);

            transform.parent.Rotate(Vector3.up * mouseX);
        }
    }

    public void SetMouseSpeed()
    {
        mouseSpeed = slider_MouseSpeed.value;

        sensX = slider_MouseSpeed.value;
        sensY = slider_MouseSpeed.value;

        txt_MouseSpeed.text = slider_MouseSpeed.value.ToString();
    }

    public void SetFOV()
    {
        fov = Mathf.FloorToInt(slider_FOV.value);
        txt_FOV.text = slider_FOV.value.ToString();

        gameObject.GetComponent<Camera>().fieldOfView = fov;
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.2f);
        isCamEnabled = true;
    }
}