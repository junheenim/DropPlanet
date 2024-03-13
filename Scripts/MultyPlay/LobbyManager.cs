using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1.0";

    [SerializeField]
    private Text connectionInfo;
    [SerializeField]
    private Button reTryConnect;

    [SerializeField]
    private Button randdomRoom, createRoom;

    [SerializeField]
    private GameObject[] room;
    private Dictionary<string, GameObject> roomIndex = new Dictionary<string, GameObject>();
    [SerializeField]
    private GameObject inRoom;
    [SerializeField]
    private Button startbutton;
    [SerializeField]
    private Text playerText;


    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = gameVersion;

        PhotonNetwork.ConnectUsingSettings();

        for (int i = 0; i < 4; i++)
            room[i].SetActive(false);

        reTryConnect.interactable = false;
        randdomRoom.interactable = false;
        createRoom.interactable = false;
        connectionInfo.text = "������";

    }

    public override void OnConnectedToMaster()
    {
        randdomRoom.interactable = true;
        createRoom.interactable = true;
        connectionInfo.text = "������ ���� �����";
        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        connectionInfo.text = "���� �ȵ�";
        reTryConnect.interactable = true;
        randdomRoom.interactable = false;
        createRoom.interactable = false;
    }
    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void ReturnToMainmenu()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("MainMenu");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        GameObject tempRoom = null;
        for (int i = 0; i < roomList.Count; i++)
        {
            // ���� ������ ���
            if (roomList[i].RemovedFromList)
            {
                roomIndex[roomList[i].Name].SetActive(false);
                roomIndex.Remove(roomList[i].Name);
            }
            else
            {
                //�� ó�� ����
                if (!roomIndex.ContainsKey(roomList[i].Name))
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (!room[j].activeSelf)
                        {
                            tempRoom = room[i];
                            break;
                        }
                    }
                    tempRoom.SetActive(true);
                    tempRoom.GetComponent<RoomSetting>().RoomInfo = roomList[i];
                    roomIndex.Add(roomList[i].Name, tempRoom);

                }
                // �� ���� ����
                else
                {
                    roomIndex.TryGetValue(roomList[i].Name, out tempRoom);
                    tempRoom.GetComponent<RoomSetting>().RoomInfo = roomList[i];
                }
            }
        }
    }

    public void CreatRoom()
    {
        for (int i = 0; i < 4; i++)
        {
            if (!room[i].activeSelf)
            {
                PhotonNetwork.CreateRoom($"Room{i}", new RoomOptions { MaxPlayers = 2 });
                return;
            }
        }
        connectionInfo.text = "�� ���� �Ұ�";
    }
    public override void OnCreatedRoom()
    {
        connectionInfo.text = "�� ����";
    }

    public override void OnJoinedRoom()
    {
        inRoom.SetActive(true);
        startbutton.gameObject.SetActive(false);
        if (PhotonNetwork.IsMasterClient)
        {
            startbutton.gameObject.SetActive(true);
            startbutton.interactable = false;
        }
        playerText.text = $"�����ο� : ( {PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers} )";
        connectionInfo.text = "�� ����";
    }
    public override void OnPlayerLeftRoom(Player other)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            startbutton.interactable = false;
        }
        playerText.text = $"�����ο� : ( {PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers} )";
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                startbutton.interactable = true;
            }
        }
        playerText.text = $"�����ο� : ({PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers})";
    }
    public void OnClickRandomJoin()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        inRoom.SetActive(false);
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connectionInfo.text = "��� ����";
        CreatRoom();
    }
    public void OnClickStart()
    {
        PhotonNetwork.LoadLevel("MultyPlayScene");
    }
}
