using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RoomSetting : MonoBehaviour
{
    [SerializeField]
    private Text roomNameText;
    private RoomInfo roomInfo;
    public RoomInfo RoomInfo
    {
        get { return roomInfo; }
        set 
        { 
            roomInfo = value;
            RoomNameSet();
        }
    }
    public void RoomNameSet()
    {
        roomNameText.text = $"{roomInfo.Name}({roomInfo.PlayerCount}/{roomInfo.MaxPlayers})";
    }

    // �� ���� �õ�
    public void Connect()
    {
        // ������ ���� ���� ���̶��
        if (PhotonNetwork.IsConnected)
        {
            // ���̸��� �°� �� �� ����
            PhotonNetwork.JoinRoom(roomInfo.Name);
        }
    }
}
