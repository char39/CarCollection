using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController_Trigger : MonoBehaviour
{
    private Camera playerCam;
    public float rayDistance = 10f;

    void Start()
    {
        playerCam = this.transform.GetChild(0).GetComponent<Camera>();
    }

    void LateUpdate()
    {
        Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);

        if (Physics.Raycast(ray, rayDistance))
        {
            //if(Input.GetKeyDown(KeyCode.E) && !GetInOutCar.instance.carInside)
                //GetInOutCar.instance.PlayerGetIn();
        }
    }
}
