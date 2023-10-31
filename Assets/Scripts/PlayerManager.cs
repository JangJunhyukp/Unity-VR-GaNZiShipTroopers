using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEditor;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    GameObject p;

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        p = PhotonNetwork.Instantiate("Player", new Vector3(Random.Range(-7f,7f),3f,13.5f), transform.rotation);
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
    }

    public void quit()
    {
        Application.Quit();
    }
}
