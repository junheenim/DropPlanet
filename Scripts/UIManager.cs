using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public static UIManager uiInstance;

    [SerializeField]
    private GameObject quitCheckUI, optionUI;

    [SerializeField]
    private Toggle bgmToggle, seToggle;

    private void Awake()
    {
        if (uiInstance == null)
        {
            uiInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        quitCheckUI.SetActive(false);
        optionUI.SetActive(false);
        bgmToggle.isOn = GameManager.instance.bgmOn;
        seToggle.isOn = GameManager.instance.seOn;
    }

    public void OpenOptionUI()
    {
        optionUI.SetActive(true);
    }
    public void OpenQuitCheckUI()
    {
        quitCheckUI.SetActive(true);
    }
    public void OnClickQuitYes()
    {
        GameManager.instance.SaveData();
        // 유니티 에디터 Play false시키기
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 게임 종료
        Application.Quit();
#endif
    }
    public void OnCLickQuitNo()
    {
        quitCheckUI.SetActive(false);
    }

    public void OnClickCloseOptionUI()
    {
        optionUI.SetActive(true);
    }

    public void BGMControl()
    {
        GameManager.instance.bgmOn = bgmToggle.isOn;
        SoundManager.soundInstance.SetBGM(bgmToggle.isOn);
    }

    public void SEControl()
    {
        GameManager.instance.seOn = seToggle.isOn;
        SoundManager.soundInstance.SetSE(seToggle.isOn);
    }
}
