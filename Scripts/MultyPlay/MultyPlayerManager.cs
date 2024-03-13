using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Realtime;

public class MultyPlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public static MultyPlayerManager multyPlayerInstance;

    public int score1, score2;
    public bool gameStart, gameEnd;
    public float gameTimer, readyTimer;

    public GameObject playerPrefab, obj;
    public MultyPlanetDorp mp;
    public EventTrigger trigger;
    private EventTrigger.Entry entry;

    public Text score1text, score2text;
    private void Awake()
    {
        multyPlayerInstance = this;

        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            obj = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 0, 0), Quaternion.identity);
            Camera.main.transform.position = new Vector3(0, 0, -10);
        }
        else
        {
            obj = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(10, 0, 0), Quaternion.identity);
            Camera.main.transform.position = new Vector3(10, 0, -10);
        }
        mp = obj.GetComponentInChildren<MultyPlanetDorp>();
    }
    private void Start()
    {
        AccesTrigger();
        score1 = 0;
        score2 = 0;
        gameEnd = false;

        gameTimer = 60;
        readyTimer = 4;
        StartCoroutine(ReadyTimer());
    }
    // 이벤트 트리거 
    public void AccesTrigger()
    {
        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) => { Drag(); });
        trigger.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener((data) => { Drop(); });
        trigger.triggers.Add(entry);
    }
    public override void OnPlayerLeftRoom(Player other)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount < 2)
        {
            readyTimer = -0.1f;
            gameTimer = 0;
            gameEnd = true;
            if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
            {
                score2 = 0;
            }
            else
            {
                score1 = 0;
            }
            score1text.text = score1.ToString();
            score2text.text = score2.ToString();
        }
    }
    private void Drag()
    {
        mp.OnDrag();
    }
    private void Drop()
    {
        mp.OnDrop();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(score1);
            stream.SendNext(score2);
            stream.SendNext(readyTimer);
            stream.SendNext(gameTimer);
            stream.SendNext(gameEnd);
        }
        else
        {
            score1 = (int)stream.ReceiveNext();
            score2 = (int)stream.ReceiveNext();
            readyTimer = (float)stream.ReceiveNext();
            gameTimer = (float)stream.ReceiveNext();
            gameEnd = (bool)stream.ReceiveNext();
        }
        score1text.text = score1.ToString();
        score2text.text = score2.ToString();
    }

    IEnumerator ReadyTimer()
    {
        while (true)
        {
            readyTimer -= Time.deltaTime;
            if (readyTimer <= -0.1)
            {
                readyTimer = -0.1f;
                break;
            }
            yield return null;
        }
        StartCoroutine(TimerStart());
    }

    IEnumerator TimerStart()
    {
        while (true)
        {
            gameTimer -= Time.deltaTime;
            if (gameTimer <= 0)
            {
                gameTimer = 0;
                break;
            }
            yield return null;
        }
        
        gameEnd = true;
    }

}
