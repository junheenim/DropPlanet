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

    // 룸 접속 시도
    public void Connect()
    {
        // 마스터 서버 접속 중이라면
        if (PhotonNetwork.IsConnected)
        {
            // 룸이름에 맞게 그 룸 접속
            PhotonNetwork.JoinRoom(roomInfo.Name);
        }
    }
}
