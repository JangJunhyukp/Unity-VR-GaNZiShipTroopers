using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;

using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{
    public Transform pHead;
    public Transform pLeft;
    public Transform pRight;
    public int maxhp = 3;
    public int currenthp;

    public Transform oHead;
    Transform oLeft;
    Transform oRight;

    PhotonView pv;
    void Start()
    {
        currenthp = maxhp;

        pv = GetComponent<PhotonView>();

        XROrigin o = FindObjectOfType<XROrigin>();

        oHead = o.transform.Find("Camera Offset/Main Camera");
        oLeft = o.transform.Find("Camera Offset/Left Controller");
        oRight = o.transform.Find("Camera Offset/Right Controller");

        if(pv.IsMine)
        {
            foreach (var r in GetComponentsInChildren<Renderer>())
            {
                r.enabled = false;
            }
        }
    }

    void SetTransform(Transform t, Transform s)
    {
        t.position = s.position;
        t.rotation = s.rotation;
    }

    
    // Update is called once per frame
    void Update()
    {
        if(pv.IsMine)
        {
            SetTransform(pHead, oHead);
            SetTransform(pLeft, oLeft);
            SetTransform(pRight, oRight);
            if (currenthp <= 0)
            {
                pv.RPC("Death", RpcTarget.AllBuffered);
                GameManager.Instance.isgameover = true;
                GameManager.Instance.gameOverUI.SetActive(true);
            }
        }
    }

    [PunRPC]
    void Death()
    {
        Destroy(gameObject);
    }
}
