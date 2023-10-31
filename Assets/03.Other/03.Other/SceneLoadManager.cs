using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    public static string loadingSecenName = "LoadingScene";
    public static string sceneName;

    public Image bar;


    public IEnumerator Start()
    {
        yield return YieldCache.WaitForSeconds(1);
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;
        for (float i = 0; i < 0.9f; i = op.progress)
        {
            bar.fillAmount = Mathf.Lerp(bar.fillAmount, i, Time.deltaTime * 5);
            yield return null;
        }
        for (float i = 0; i < 1f; i += Time.deltaTime * 0.5f)
        {
            bar.fillAmount = Mathf.Lerp(bar.fillAmount, 1, Time.deltaTime * 5);
            yield return null;
        }
        yield return YieldCache.WaitForSeconds(1);
        op.allowSceneActivation = true;
    }

    public static void LoadScene(string _sceneName)
    {
        sceneName = _sceneName;
        SceneManager.LoadScene(loadingSecenName);
    }
}