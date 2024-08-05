using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetInOutCar : MonoBehaviour
{
    public static bool carInside = false;
    public static bool carOutside = false;
    private string playerTag = "Player";
    public GameObject fpsPlayer;
    public Transform player;
    public Transform carOff;
    public Camera mainCam;

    void Start()
    {
        fpsPlayer = GameObject.Find(playerTag);
        player = fpsPlayer.transform.GetChild(0).gameObject.transform;
        mainCam = Camera.main;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag(playerTag))
            PlayerGetIn();
    }

    private void PlayerGetIn()
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
        fpsPlayer.transform.position = carOff.transform.position + carOff.transform.right * -2;
        player.gameObject.SetActive(true);
        mainCam.depth = -1f;
        fpsPlayer.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Camera>().depth = 0f;
    }

    void Update()
    {
        if (carOutside)
            PlayerGetOut();
    }
}
