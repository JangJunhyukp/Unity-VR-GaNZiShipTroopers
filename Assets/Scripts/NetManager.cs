using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.Demo.Cockpit;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]

public class Room
{
    public string name;
    public int sceneNumber;

}

public class NetManager : MonoBehaviourPunCallbacks
{
    public Room room;
    public GameObject lobbyBtn;
    public PhotonView pv;

    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    void Start()
    {
        //Connect();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();

        /*RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        PhotonNetwork.JoinOrCreateRoom("Killroom", roomOptions, TypedLobby.Default);*/
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        lobbyBtn.SetActive(true);
    }

    public void InitRoom(int iRoomNumber)
    {
        PhotonNetwork.LoadLevel(room.sceneNumber);

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        PhotonNetwork.JoinOrCreateRoom(room.name, roomOptions, TypedLobby.Default);
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        pv.RPC("StartGame", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void StartGame()
    {
        GameManager.Instance.isgameover = false;
    }
}
