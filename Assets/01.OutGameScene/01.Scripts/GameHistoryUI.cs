using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameHistoryUI : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    
    public void Setting(string description)
    {
        text.text = description;
    }
}
