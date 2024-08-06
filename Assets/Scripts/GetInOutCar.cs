using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetInOutCar : MonoBehaviour
{
    public static GetInOutCar instance;
    public bool carInside = false;
    public bool carOutside = false;
    //private string playerTag = "Player";
    public GameObject fpsPlayer;
    public Transform player;
    public Transform takeOff;
    public Camera mainCam;
/* 
    void Start()
    {
        instance = this;
        fpsPlayer = GameObject.Find(playerTag);
        player = fpsPlayer.transform.GetChild(0).gameObject.transform;
        mainCam = Camera.main;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag(playerTag))
            PlayerGetIn();
    }

    public void PlayerGetIn()
    {
        carInside = true;
        player.gameObject.SetActive(false);
        mainCam.depth = 0f;
        fpsPlayer.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Camera>().depth = -1f;
    }

    public void PlayerGetOut()
    {
        carInside = false;
        carOutside = false;
        fpsPlayer.transform.position = takeOff.transform.position + takeOff.transform.right * -2;
        player.gameObject.SetActive(true);
        mainCam.depth = -1f;
        fpsPlayer.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Camera>().depth = 0f;
    }

    void Update()
    {
        if (carOutside)
            PlayerGetOut();
    } */
}
