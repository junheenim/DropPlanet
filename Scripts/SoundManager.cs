using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager soundInstance;

    [SerializeField]
    private AudioSource seAudioSource;

    [SerializeField]
    private AudioClip onClick;

    [SerializeField]
    private AudioMixer audioMixer;

    private void Awake()
    {
        if (soundInstance == null)
        {
            soundInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        SetBGM(GameManager.instance.bgmOn);
        SetSE(GameManager.instance.seOn);
    }

    public void SetBGM(bool bgmSet)
    {
        if (bgmSet)
            audioMixer.SetFloat("BGM", 0);
        else
            audioMixer.SetFloat("BGM", -80);
    }

    public void SetSE(bool seSet)
    {
        if (seSet)
            audioMixer.SetFloat("SE", 0);
        else
            audioMixer.SetFloat("SE", -80);
    }
}
