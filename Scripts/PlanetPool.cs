using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class PlanetObject
{
    public GameObject[] Planet;
}
public class PlanetPool : MonoBehaviour
{
    [SerializeField]
    private PlanetObject[] planetObjects;
    public Queue<GameObject>[] nextPlanet = new Queue<GameObject>[11];

    [SerializeField]
    private ParticleSystem[] boom;
    public Queue<ParticleSystem> particlePool = new Queue<ParticleSystem>();

    public AudioSource audioSource;
    [SerializeField]
    private AudioClip mergeSound;

    private void Awake()
    {
        // Planet Queue
        for (int i = 0; i < planetObjects.Length; i++)
        {
            nextPlanet[i] = new Queue<GameObject>();
            for (int j = 0; j < planetObjects[i].Planet.Length; j++)
                nextPlanet[i].Enqueue(planetObjects[i].Planet[j]);
        }
        // ÆÄÆ¼Å¬ Queue
        for (int i = 0; i < boom.Length; i++)
            particlePool.Enqueue(boom[i]);
    }
    public void MergeSoundON()
    {
        audioSource.PlayOneShot(mergeSound);
    }
}
