using Photon.Pun;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MultyInGameUI : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject returnCheckUI;

    public int score = 0;

    // GameOver
    public bool isOver;
    [SerializeField]
    private GameObject returnUI;
    [SerializeField]
    private GameObject pad, gameOverPanel, returnButton;

    [SerializeField]
    private RectTransform gameOverText;
    [SerializeField]
    private GameObject winText, loseText;
    [SerializeField]
    private Text Timer;
    [SerializeField]
    private RawImage rawImage;
    [SerializeField]
    private RenderTexture[] playerTexture;
    private void Start()
    {
        isOver = false;
        returnButton.SetActive(false);
        returnUI.SetActive(false);
        gameOverPanel.SetActive(false);
        gameOverText.anchoredPosition = new Vector2(0, 1200);
        returnCheckUI.SetActive(false);
        pad.SetActive(false);
        rawImage.texture = playerTexture[PhotonNetwork.LocalPlayer.ActorNumber - 1];
    }
    private void Update()
    {
        TimerText();
    }
    private void TimerText()
    {
        if (MultyPlayerManager.multyPlayerInstance.readyTimer > 0)
        {
            Timer.text = ((int)MultyPlayerManager.multyPlayerInstance.readyTimer - 1).ToString();
            if (MultyPlayerManager.multyPlayerInstance.readyTimer <= 1)
            {
                Timer.color = Color.red;
                Timer.text = "Start!";
            }
            if (MultyPlayerManager.multyPlayerInstance.readyTimer <= 0.1)
            {
                pad.SetActive(true);
            }
        }
        else
        {
            Timer.text = ((int)MultyPlayerManager.multyPlayerInstance.gameTimer).ToString();
            Timer.color = new Color(255, 120, 0);
            if (MultyPlayerManager.multyPlayerInstance.gameTimer <= 0)
            {
                pad.SetActive(false);
                Timer.gameObject.SetActive(false);
                GameOver();
            }
        }
    }

    public void OnClickOption()
    {
        UIManager.uiInstance.OpenOptionUI();
    }

    public void ReturnMueu()
    {
        returnCheckUI.SetActive(true);
    }
    public void OnCLickReturnCheck(bool yes)
    {
        if (yes)
        {
            PhotonNetwork.Disconnect();
        }

        else
            returnCheckUI.SetActive(false);
    }
    public override void OnLeftRoom()
    {
        LoadingSceneManager.LoadScene("MainMenu");
    }

    public void GameOver()
    {
        if (isOver)
            return;
        isOver = true;
        pad.SetActive(false);
        gameOverPanel.SetActive(true);
        GameObject.Find("PlanetDroper").GetComponent<MultyPlanetDorp>().isDrag = false;
        winText.SetActive(false); 
        loseText.SetActive(false);
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            if (MultyPlayerManager.multyPlayerInstance.score1 >= MultyPlayerManager.multyPlayerInstance.score2)
                winText.SetActive(true);
            else
                loseText.SetActive(true);
        }
        else if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            if (MultyPlayerManager.multyPlayerInstance.score1 <= MultyPlayerManager.multyPlayerInstance.score2)
                winText.SetActive(true);
            else
                loseText.SetActive(true);
        }

        StartCoroutine(GameOverTextDown());
    }

    IEnumerator GameOverTextDown()
    {
        while (true)
        {
            gameOverText.anchoredPosition += Vector2.down * 10;
            if (gameOverText.anchoredPosition.y <= 250)
            {
                gameOverText.anchoredPosition = new Vector2(0, 250);
                break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(1.5f);
        returnButton.SetActive(true);
    }
}
