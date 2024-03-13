using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject score;
    private Scoreboard scoreboard;

    private void Awake()
    {
        scoreboard = score.GetComponent<Scoreboard>();
    }

    public void OnCLickSinglePlay()
    {
        LoadingSceneManager.LoadScene("SinglePlayScene");
    }
    public void OnClickMultyPlay()
    {
        LoadingSceneManager.LoadScene("MultyRoom");
    }
    public void OnClickScoreboard()
    {
        score.SetActive(true);
        scoreboard.SetScore();
    }

    public void OcCLickOption()
    {
        UIManager.uiInstance.OpenOptionUI();
    }

    public void OnCLickQuit()
    {
        GameManager.instance.SaveData();
        UIManager.uiInstance.OpenQuitCheckUI();
    }
}
