using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameResult : MonoSington<GameResult>
{
    [SerializeField] private GameObject resultUI;
    [SerializeField] private GameObject renewedBestScore;

    [SerializeField] private TMP_Text score_T;
    [SerializeField] private TMP_Text bestScore_T;

    [SerializeField] private Animator animator;

    private void Setting(int score)
    {
        int bestScore = DataManager.PlayData.bestScore;
        if (bestScore <= score)
        {
            renewedBestScore.SetActive(true);
        }
        GameHistory gameHistory = new GameHistory(DateTime.Now.ToString("MM"), DateTime.Now.ToString("dd"), DateTime.Now.ToString("hh"), score, InGameManager.difficulty);
        DataManager.SaveGameHistory(gameHistory);
        score_T.text = $"내 점수\n<size=200%>{score}</size>점";
        bestScore_T.text = $"최고 기록 : {DataManager.PlayData.bestScore}점";
    }

    public void ShowGameResult(int score)
    {
        Setting(score);
        resultUI.SetActive(true);
        animator.SetTrigger("Play");
    }

    public void LoadScene(string sceneName)
    {
        SceneLoadManager.LoadScene(sceneName);
    }
}
