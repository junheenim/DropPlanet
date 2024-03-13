using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MultyPlanetDorp : MonoBehaviourPun
{
    Vector3 pos;
    LayerMask layMask;
    RaycastHit2D hitObject;
    public GameObject assistantLine;
    [SerializeField]
    private int level;
    [SerializeField]
    private Transform parantPos;
    private float[] edgeX = { 2.22f, 2.1f, 2f, 1.88f, 1.77f };
    public bool isDrag, isReady;
    [SerializeField]
    private PlanetPool planetPool;
    private GameObject readyPlanet;
    private Rigidbody2D planetRB;

    public GameObject[] planetSet;
    private void Start()
    {
        layMask = LayerMask.GetMask("Planet") | LayerMask.GetMask("Wall");
        isDrag = false;
        isReady = false;
        assistantLine.SetActive(false);
        if (photonView.IsMine)
        {
            StartCoroutine(Ready());
            assistantLine.SetActive(true);
        }
    }
    private void Update()
    {
        PoolPositionSet();
    }

    public void PoolPositionSet()
    {
        if (!photonView.IsMine || !isDrag || !isReady)
            return;

        pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.y = 2.2f;
        pos.z = 0;
        if (pos.x <= -edgeX[level] + parantPos.position.x)
        {
            pos.x = -edgeX[level] + parantPos.position.x;
            transform.position = pos;
        }
        else if (pos.x >= edgeX[level] + parantPos.position.x)
        {
            pos.x = edgeX[level] + parantPos.position.x;
            transform.position = pos;
        }
        transform.position = Vector3.Lerp(transform.position, pos, 0.2f);
        readyPlanet.transform.position = new Vector3(transform.position.x, 1.69f - level * 0.1f, transform.position.z);
        AssistantLineSet();
    }

    private void AssistantLineSet()
    {
        hitObject = Physics2D.Raycast(transform.position, Vector3.down, 80f, layMask);
        if (hitObject)
        {
            assistantLine.gameObject.transform.localScale =
                new Vector3((gameObject.transform.position.y - hitObject.transform.position.y) * 6.7f, 0.05f, 1);
        }
    }

    public void OnDrag()
    {
        if (photonView.IsMine && isReady)
        {
            isDrag = true;
        }
    }
    public void OnDrop()
    {
        if (!photonView.IsMine || !isReady && !isDrag)
            return;

        isReady = false;
        isDrag = false;
        planetRB.simulated = true;
        planetRB.velocity = Vector2.zero;
        planetRB.rotation = 0;
        readyPlanet = null;
        StartCoroutine(Ready());
    }

    IEnumerator Ready()
    {
        yield return new WaitForSeconds(0.3f);
        level = Random.Range(0, 5);
        readyPlanet = planetPool.nextPlanet[level].First();
        planetPool.nextPlanet[level].Dequeue();
        planetRB = readyPlanet.GetComponent<Rigidbody2D>();
        planetRB.simulated = false;
        readyPlanet.GetComponent<MultyPlanet>().isMerge = false;

        readyPlanet.SetActive(true);

        readyPlanet.transform.position = new Vector3(transform.position.x, 1.69f - level * 0.1f, transform.position.z);

        isReady = true;
    }
}
