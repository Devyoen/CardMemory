using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHistorySceneManager : MonoBehaviour
{
    [SerializeField] private Transform uiParent;
    [SerializeField] private GameHistoryUI historyUIPrefab;

    private void Start()
    {
        PlayData playData = DataManager.PlayData;
        foreach (GameHistory gameHistory in  playData.gameHistoryList)
        {
            GameHistoryUI historyUI = Instantiate(historyUIPrefab, uiParent);
            string difficultyText = "";
            switch (gameHistory.difficulty)
            {
                case 0:
                    difficultyText = "쉬움";
                    break;
                case 1:
                    difficultyText = "보통";
                    break;
                case 2:
                    difficultyText = "어려움";
                    break;
                default:
                    Debug.Log("오류 : 난이도 범위 벗어남");
                    break;
            }
            historyUI.Setting($"{gameHistory.month}월 {gameHistory.month}일 {gameHistory.hour}시 {difficultyText} {gameHistory.score}점");
        }
    }

    public void LoadMainScene()
    {
        SceneLoadManager.LoadScene("MainScene");
    }
}
