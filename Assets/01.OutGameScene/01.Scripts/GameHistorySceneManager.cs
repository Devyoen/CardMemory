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
                    difficultyText = "����";
                    break;
                case 1:
                    difficultyText = "����";
                    break;
                case 2:
                    difficultyText = "�����";
                    break;
                default:
                    Debug.Log("���� : ���̵� ���� ���");
                    break;
            }
            historyUI.Setting($"{gameHistory.month}�� {gameHistory.month}�� {gameHistory.hour}�� {difficultyText} {gameHistory.score}��");
        }
    }

    public void LoadMainScene()
    {
        SceneLoadManager.LoadScene("MainScene");
    }
}
