using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

using Photon.Pun;
using Photon.Realtime;

public class Crosshair : MonoBehaviour
{
    public XRController controller; // XR 컨트롤러
    public Transform crosshair;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
       if (controller != null)
       {
            Vector3 controllerPosition = controller.transform.position + controller.transform.forward * 10f;

            Quaternion controllerRotation = controller.transform.rotation;


            crosshair.position = controllerPosition;
            crosshair.rotation = controllerRotation;
       }      
    }
}
