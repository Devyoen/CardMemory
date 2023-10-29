using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UIType
{
    Timer,
    Score,
}

public class InGameUIs : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Text timer_T;
    [SerializeField] private Text score_T;

    public void UpdateUI(UIType uiType, object value)
    {
        switch (uiType)
        {
            case UIType.Timer :
                UpdateTimerUI((float)value);
                break;
            case UIType.Score :
                UpdateScoreUI((int)value);
                break;
            default :
                Debug.Log("Error");
                break;
        }
    }

    private void UpdateTimerUI(float value)
    {
        timer_T.text = $"LeftTime : {value}";
    }

    private void UpdateScoreUI(int value)
    {
        score_T.text = $"{value}";
    }
}
