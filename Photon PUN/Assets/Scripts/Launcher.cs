using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;

public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField roomName;
    [SerializeField] private TMP_Text errorText, roomNameText;
    [SerializeField] private Transform roomsList, playerList;
    [SerializeField] private GameObject roomButtonPrefab, playerItemPrefab;
    [SerializeField] private Transform startGameButton;


    public static Launcher instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        MenuManager.instance.openMenu("title");
        PhotonNetwork.NickName = "Player " + Random.Range(0, 20).ToString("000");
    }

    public void createRoom()
    {
        if (string.IsNullOrEmpty(roomName.text))
        {
            return;
        }
        PhotonNetwork.CreateRoom(roomName.text);
        MenuManager.instance.openMenu("loading");
    }

    public override void OnJoinedRoom()
    {
        MenuManager.instance.openMenu("room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        Player[] players = PhotonNetwork.PlayerList;

        foreach (Transform trans in playerList)
        {
            Destroy(trans.gameObject);
        }

        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(playerItemPrefab, playerList).GetComponent<playerItem>().setUp(players[i]);
        }

        startGameButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Creation failed due to: " + message;
        MenuManager.instance.openMenu("error");
    }

    public void leaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.instance.openMenu("loading");
    }

    public void joinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.instance.openMenu("loading");
    }

    public override void OnLeftRoom()
    {
        MenuManager.instance.openMenu("title");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform trans in roomsList)
        {
            Destroy(trans.gameObject);
        }

        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
                continue;
            Instantiate(roomButtonPrefab, roomsList).GetComponent<roomItem>().setUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerItemPrefab, playerList).GetComponent<playerItem>().setUp(newPlayer);
    }

    public void startGame()
    {
        PhotonNetwork.LoadLevel(1);
    }
}
