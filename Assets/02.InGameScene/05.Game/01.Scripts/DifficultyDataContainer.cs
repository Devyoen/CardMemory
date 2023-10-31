using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DifficultyData
{
    [SerializeField] private float time;
    [SerializeField] private int score;

    public float Time => time;
    public int Score => score;
}

[CreateAssetMenu(fileName = "DifficultyDataContainer", menuName = "Scriptable Object/DifficultyDataContainer")]
public class DifficultyDataContainer : ScriptableObject
{ 
    [SerializeField] private List<DifficultyData> list = new List<DifficultyData>();
    public List<DifficultyData> List => list;
}
