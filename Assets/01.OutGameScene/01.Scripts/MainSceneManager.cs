using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject difficultySelectUI;

    public void ShowDifficultySelectUI()
    {
        difficultySelectUI.SetActive(true);
    }

    public void CloseDifficultySelectUI()
    {
        difficultySelectUI.SetActive(false);
    }

    public void SelectDifficulty(int difficulty)
    {
        InGameManager.difficulty = difficulty;
        SceneLoadManager.LoadScene("PlayScene");
    }

    public void LoadGameHistoryScene()
    {
        SceneLoadManager.LoadScene("GameHistoryScene");
    }
}
