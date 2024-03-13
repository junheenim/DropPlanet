using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    // 이동 씬이름 저장
    public static string nextScene;

    // 로딩 바
    [SerializeField]
    private Image loadingBar;
    [SerializeField]
    private Image planetImage;
    [SerializeField]
    private Sprite[] planet;
    AsyncOperation aop;

    private void Start()
    {
        planetImage.sprite = planet[Random.Range(0, planet.Length)];
        StartCoroutine(LoadScene());
    }

    public static void LoadScene(string sceneName)
    {
        // 이동할 신
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator LoadScene()
    {
        yield return null;
        // 씬을 불러오는 동안 일시중지 발생 하지 않는 로드 방식(비동기방식 로드)
        // LoadSceneAsync()로 로딩의 진행정도 받아오기
        aop = SceneManager.LoadSceneAsync(nextScene);

        // 자동으로 신이동 막기
        aop.allowSceneActivation = false;

        //로딩바 Lerp 변수
        float timer = 0f;

        // 작업완료 나타내는 프로퍼티
        while (!aop.isDone)
        {
            yield return null;
            timer += Time.deltaTime;
            // 로딩진행 로딩 완료시 0.9에서 끝남
            if (aop.progress < 0.9f)
            {
                loadingBar.fillAmount =
                    Mathf.Lerp(loadingBar.fillAmount, aop.progress, timer);

                if (loadingBar.fillAmount >= aop.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                // 페이크 로딩 1초후 씬전환
                loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, 1f, timer);

                if (loadingBar.fillAmount >= 1.0f)
                {
                    aop.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }

}
