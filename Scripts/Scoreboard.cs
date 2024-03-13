using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
    [SerializeField]
    private Text[] scores;

    private void Start()
    {
        CloseScoreboard();
    }
    public void SetScore()
    {
        gameObject.SetActive(true);
        for (int i = 0; i < 10; i++)
            scores[i].text = GameManager.instance.score[i].ToString();
    }

    public void CloseScoreboard()
    {
        gameObject.SetActive(false);
    }
}
