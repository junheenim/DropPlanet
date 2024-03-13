using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;

public class MultyPlanet : MonoBehaviourPun
{
    public int level;
    public bool isMerge;
    private Animator animator;
    private MultyPlanet mergePlanet;
    private PlanetPool pool;
    private GameObject nextLevelPlanet;
    private GameObject curPlanet;
    public Rigidbody2D planetRB;
    private Rigidbody2D nextLevelPlanetRB;
    private MultyInGameUI gameUI;
    float deadTime = 0;
    private SpriteRenderer planetRenderer;
    private int[] scoreTeble = { 1, 3, 6, 11, 19, 32, 53, 87, 142, 231, 375 };
    private void Awake()
    {
        animator = GetComponent<Animator>();
        pool = GetComponentInParent<PlanetPool>();
        planetRB = GetComponent<Rigidbody2D>();
        planetRenderer = GetComponent<SpriteRenderer>();

        gameUI = GameObject.Find("GameUI").GetComponent<MultyInGameUI>();
    }

    void Start()
    {
        if (photonView.IsMine)
            gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        animator.SetTrigger(level.ToString());
        isMerge = false;
        planetRB.simulated = false;
        deadTime = 0;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "CutLine" && !MultyPlayerManager.multyPlayerInstance.gameEnd)
        {
            deadTime += Time.deltaTime;
            if (deadTime > 2.0f)
            {
                planetRenderer.color = Color.red;
            }
            if (deadTime > 5f)
            {
                if (photonView.IsMine)
                    photonView.RPC("GameOver", RpcTarget.MasterClient);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        deadTime = 0;
        planetRenderer.color = Color.white;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameUI.isOver)
            return;

        if (collision.gameObject.tag == "Planet")
        {
            mergePlanet = collision.gameObject.GetComponent<MultyPlanet>();
            if (mergePlanet.level == level && !isMerge && !mergePlanet.isMerge && level < 11)
            {
                mergePlanet.isMerge = true;
                isMerge = true;

                curPlanet = collision.gameObject;

                pool.nextPlanet[level].Enqueue(gameObject);
                pool.nextPlanet[level].Enqueue(collision.gameObject);

                planetRB.simulated = false;
                mergePlanet.planetRB.simulated = false;

                if (photonView.IsMine)
                    photonView.RPC("PlanetPos", RpcTarget.All, curPlanet.name);

                curPlanet.transform.rotation = Quaternion.Euler(Vector3.zero);
                gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);

                planetRB.velocity = Vector2.zero;
                planetRB.rotation = 0;
                mergePlanet.planetRB.velocity = Vector2.zero;
                mergePlanet.planetRB.rotation = 0;



                // paticle
                pool.particlePool.First().transform.position = collision.contacts[0].point;
                pool.particlePool.First().Play();
                pool.particlePool.Enqueue(pool.particlePool.First());
                pool.particlePool.Dequeue();

                pool.MergeSoundON();
                // 스코어 증가
                if (photonView.IsMine)
                    photonView.RPC("SetScore", RpcTarget.MasterClient);

                if (level < 10)
                {
                    // 다음 레벨 행성 활성화
                    nextLevelPlanet = pool.nextPlanet[level + 1].First();
                    pool.nextPlanet[level + 1].Dequeue();

                    nextLevelPlanet.transform.position = collision.contacts[0].point;
                    nextLevelPlanet.SetActive(true);
                    nextLevelPlanetRB = nextLevelPlanet.GetComponent<Rigidbody2D>();
                    nextLevelPlanetRB.simulated = true;
                    nextLevelPlanet.GetComponent<MultyPlanet>().isMerge = false;
                    nextLevelPlanetRB.velocity = Vector2.zero;
                    nextLevelPlanetRB.rotation = 0;
                }
            }
        }
    }

    [PunRPC]
    public void PlanetPos(string name)
    {
        gameObject.transform.localPosition = Vector3.zero;
        transform.parent.Find(name).transform.localPosition = Vector3.zero;
    }

    [PunRPC]
    public void SetScore()
    {
        if (photonView.IsMine)
            MultyPlayerManager.multyPlayerInstance.score1 += scoreTeble[level];
        else
            MultyPlayerManager.multyPlayerInstance.score2 += scoreTeble[level];
    }
    [PunRPC]
    public void GameOver()
    {
        if (photonView.IsMine)
            MultyPlayerManager.multyPlayerInstance.score1 = 0;
        else
            MultyPlayerManager.multyPlayerInstance.score2 = 0;

        MultyPlayerManager.multyPlayerInstance.gameTimer = 0;
        MultyPlayerManager.multyPlayerInstance.gameEnd = true;
    }
}
