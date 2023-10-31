using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class FollowUI : MonoBehaviour
{
    Transform oHead;
    public GameObject[] follwer;
    // Start is called before the first frame update
    void Start()
    {
        XROrigin o = FindObjectOfType<XROrigin>();

        oHead = o.transform.Find("Camera Offset/Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 overUIPosition = oHead.transform.position + oHead.transform.forward * 5f + new Vector3(0f,1.2f,0f);
        Vector3 hitPanelPosition = oHead.transform.position + oHead.transform.forward * 5f; //+ new Vector3(0f,1.2f,0f);
        Vector3 timePosition = oHead.transform.position + oHead.transform.forward * 5f + new Vector3(0f,1f,0f);

        Quaternion cameraRotation = oHead.transform.rotation;


        follwer[0].transform.position = overUIPosition;
        follwer[1].transform.position = hitPanelPosition;
        follwer[2].transform.position = timePosition;
        gameObject.transform.rotation = cameraRotation;
    }
}
