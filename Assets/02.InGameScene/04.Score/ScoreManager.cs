using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoSington<ScoreManager>
{
    private int score;
    public int Score => score;

    public void ScoreInit()
    {
        score = 0;
    }

    public void AddScore()
    {
        score += InGameManager.instance.GetDifficultyData().Score;
        InGameUIs.instance.UpdateUI(UIType.Score, Score);
    }
}
