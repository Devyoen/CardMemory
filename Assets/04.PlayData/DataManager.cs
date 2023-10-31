using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class PlayData
{
    public int bestScore;

    public List<GameHistory> gameHistoryList = new List<GameHistory>();
}

[System.Serializable]
public class GameHistory
{
    public string month;
    public string day;
    public string hour;
    public int score;
    public int difficulty;

    public GameHistory(string month, string day, string hour, int score, int difficulty)
    {
        this.month = month;
        this.day = day;
        this.hour = hour;
        this.score = score;
        this.difficulty = difficulty;
    }
}

public static class DataManager
{
    public static string path => $"{Application.persistentDataPath}/PlayData.json";

    private static PlayData playData;
    public static PlayData PlayData
    {
        get
        {
            if (File.Exists(path))
            {
                playData = Load();
                Save(playData);
                return playData;
            }
            else
            {
                return new PlayData();
            }
        }
    }

    public static PlayData Load()
    {
        string json = File.ReadAllText(path);
        PlayData data = JsonUtility.FromJson<PlayData>(json);
        return data;
    }

    public static void Save(PlayData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
    }

    public static void SaveGameHistory(GameHistory gameHistory)
    {
        PlayData data = PlayData;
        if (gameHistory.score > data.bestScore)
        {
            data.bestScore = gameHistory.score;
        }
        data.gameHistoryList.Add(gameHistory);
        while (data.gameHistoryList.Count > 10)
        {
            data.gameHistoryList.RemoveAt(data.gameHistoryList.Count);
        }
        Save(data);
    }
}
