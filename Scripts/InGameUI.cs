using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InGameUI : MonoBehaviour
{
    [SerializeField]
    private GameObject returnCheckUI;

    [SerializeField]
    private Text scoreText;
    private int score = 0;
    private int[] scoreTeble = { 1, 3, 6, 11, 19, 32, 53, 87, 142, 231, 375 };

    // GameOver
    public bool isOver;
    [SerializeField]
    private GameObject pad, returnUI;
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private RectTransform gameOverText;
    [SerializeField]
    private Text endscoreText;

    private void Start()
    {
        isOver = false;
        returnUI.SetActive(false);
        gameOverPanel.SetActive(false);
        pad.SetActive(true);
        gameOverText.anchoredPosition = new Vector2(0, 1200);
        returnCheckUI.SetActive(false);
        score = 0;
        scoreText.text = score.ToString();
    }

    public void SetScroe(int level)
    {
        score += scoreTeble[level];
        scoreText.text = score.ToString();
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
            LoadingSceneManager.LoadScene("MainMenu");
        else
            returnCheckUI.SetActive(false);
    }

    public void GameOver()
    {
        if (isOver)
            return;
        isOver = true;
        pad.SetActive(false);
        gameOverPanel.SetActive(true);
        GameObject.Find("PlanetDroper").GetComponent<PlanetDrop>().isDrag = false;
        StartCoroutine(GameOverTextDown());
        // 기록 갱신
        if (GameManager.instance.score[9] < score)
        {
            GameManager.instance.score[9] = score;
            Array.Sort(GameManager.instance.score);
            Array.Reverse(GameManager.instance.score);
        }
        GameManager.instance.SaveData();
    }

    IEnumerator GameOverTextDown()
    {
        while(true)
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
        returnUI.SetActive(true);
        endscoreText.text = "점수 : " + score.ToString();
    }
}
