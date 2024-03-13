
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlanetDrop : MonoBehaviour
{
    Vector3 pos;
    LayerMask layMask;
    RaycastHit2D hitObject;
    public GameObject assistantLine;
    private int level;
    private float[] edgeX = { 2.22f, 2.1f, 2f, 1.88f, 1.77f };
    public bool isDrag, isReady;
    [SerializeField]
    private PlanetPool planetPool;
    private GameObject readyPlanet;
    private Rigidbody2D planetRB;
    private void Start()
    {
        layMask = LayerMask.GetMask("Planet") | LayerMask.GetMask("Wall");
        isDrag = false;
        isReady = false;
        StartCoroutine(Ready());
    }
    private void Update()
    {
        PoolPositionSet();
        AssistantLineSet();

    }
    public void PoolPositionSet()
    {
        if (!isDrag || !isReady)
            return;

        pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.y = 2.2f;
        pos.z = 0;
        if (pos.x <= -edgeX[level])
        {
            pos.x = -edgeX[level];
            transform.position = pos;
        }
        else if (pos.x >= edgeX[level])
        {
            pos.x = edgeX[level];
            transform.position = pos;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, pos, 0.2f);
        readyPlanet.transform.localPosition = new Vector3(transform.position.x, 1.69f - level * 0.1f, transform.position.z);
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
        if (isReady)
            isDrag = true;
    }

    public void OnDrop()
    {
        if (!isReady && !isDrag)
            return;
        isReady = false;
        isDrag = false;
        planetRB.simulated = true;
        readyPlanet = null;
        StartCoroutine(Ready());
    }

    IEnumerator Ready()
    {
        yield return new WaitForSeconds(0.2f);
        level = Random.Range(0, 5);
        readyPlanet = planetPool.nextPlanet[level].First();
        planetPool.nextPlanet[level].Dequeue();
        planetRB = readyPlanet.GetComponent<Rigidbody2D>();
        planetRB.simulated = false;
        readyPlanet.transform.localPosition = gameObject.transform.position;
        readyPlanet.SetActive(true);
        readyPlanet.transform.localPosition = new Vector3(transform.position.x, 1.69f - level * 0.1f, transform.position.z);
        isReady = true;
    }
}
