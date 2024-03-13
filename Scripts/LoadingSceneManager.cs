using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    // �̵� ���̸� ����
    public static string nextScene;

    // �ε� ��
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
        // �̵��� ��
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator LoadScene()
    {
        yield return null;
        // ���� �ҷ����� ���� �Ͻ����� �߻� ���� �ʴ� �ε� ���(�񵿱��� �ε�)
        // LoadSceneAsync()�� �ε��� �������� �޾ƿ���
        aop = SceneManager.LoadSceneAsync(nextScene);

        // �ڵ����� ���̵� ����
        aop.allowSceneActivation = false;

        //�ε��� Lerp ����
        float timer = 0f;

        // �۾��Ϸ� ��Ÿ���� ������Ƽ
        while (!aop.isDone)
        {
            yield return null;
            timer += Time.deltaTime;
            // �ε����� �ε� �Ϸ�� 0.9���� ����
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
                // ����ũ �ε� 1���� ����ȯ
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
