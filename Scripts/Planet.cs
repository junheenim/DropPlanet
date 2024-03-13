using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;

public class Planet : MonoBehaviourPun
{
    public int level;
    public bool isMerge;
    private Animator animator;
    private Planet mergePlanet;
    private PlanetPool pool;
    private GameObject nextLevelPlanet;
    private Rigidbody2D planetRB;
    private Rigidbody2D nextLevelPlanetRB;
    private InGameUI gameUI;
    float deadTime = 0;
    private SpriteRenderer planetRenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        pool = GetComponentInParent<PlanetPool>();
        planetRB = GetComponent<Rigidbody2D>();
        planetRenderer = GetComponent<SpriteRenderer>();
        gameUI = GameObject.Find("GameUI").GetComponent<InGameUI>();
    }

    void Start()
    {
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
        if (gameUI != null)
        {
            if (collision.tag == "CutLine" && !gameUI.isOver)
            {
                deadTime += Time.deltaTime;
                if (deadTime > 2.0f)
                {
                    planetRenderer.color = Color.red;
                }
                if (deadTime > 5f)
                {
                    if (gameUI != null)
                        gameUI.GameOver();
                    else
                        MultyPlayerManager.multyPlayerInstance.gameTimer = 0;
                }
            }
        }
        else
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
                    MultyPlayerManager.multyPlayerInstance.gameTimer = 0;
                    MultyPlayerManager.multyPlayerInstance.gameEnd = true;
                }
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
            mergePlanet = collision.gameObject.GetComponent<Planet>();

            if (mergePlanet.level == level && !isMerge && !mergePlanet.isMerge && level < 11)
            {
                // 2개 합친거 숨기기
                mergePlanet.isMerge = true;
                isMerge = true;
                collision.gameObject.SetActive(false);
                gameObject.SetActive(false);
                pool.nextPlanet[level].Enqueue(gameObject);
                pool.nextPlanet[level].Enqueue(collision.gameObject);

                // paticle
                pool.particlePool.First().transform.position = collision.contacts[0].point;
                pool.particlePool.First().Play();
                pool.particlePool.Enqueue(pool.particlePool.First());
                pool.particlePool.Dequeue();

                pool.MergeSoundON();
                // 스코어 증가
                if (gameUI != null)
                    gameUI.SetScroe(level);

                if (level < 10)
                {
                    // 다음 레벨 행성 활성화
                    nextLevelPlanet = pool.nextPlanet[level + 1].First();
                    pool.nextPlanet[level + 1].Dequeue();

                    nextLevelPlanet.transform.position = collision.contacts[0].point;
                    nextLevelPlanet.SetActive(true);
                    nextLevelPlanetRB = nextLevelPlanet.GetComponent<Rigidbody2D>();
                    nextLevelPlanetRB.velocity = Vector2.zero;
                    nextLevelPlanetRB.rotation = 0;
                    nextLevelPlanetRB.simulated = true;
                }
            }
        }
    }
}
