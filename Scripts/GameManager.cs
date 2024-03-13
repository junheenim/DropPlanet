using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Data;

public class Data
{
    public int[] score = new int[10];
    public bool bgmOn;
    public bool seOn;
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // 저장
    private string path;
    public Data data;
    
    // 데이터 
    public int[] score;
    public bool bgmOn;
    public bool seOn;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this; 
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        // Data Load
        path = Path.Combine(Application.dataPath, "ScoreData.josn");
        LoadData();
        Application.targetFrameRate = 60;
    }

    public void SaveData()
    {
        for (int i = 0; i < score.Length; i++)
            data.score[i] = score[i];
        data.bgmOn = bgmOn;
        data.seOn = seOn;

        string jsonFile = JsonUtility.ToJson(data);

        File.WriteAllText(path, jsonFile);
    }

    private void LoadData()
    {
        data = new Data();
        if (File.Exists(path))
        {
            string jsonFile = File.ReadAllText(path);
            data = JsonUtility.FromJson<Data>(jsonFile);

            // Data Load
            for (int i = 0; i < 10; i++)
                score[i] = data.score[i];
            bgmOn = data.bgmOn;
            seOn = data.seOn;
        }
    }

    private void ResetData()
    {
        for (int i = 0; i < 10; i++)
            score[i] = 0;
        bgmOn = true;
        seOn = true;
    }
}
