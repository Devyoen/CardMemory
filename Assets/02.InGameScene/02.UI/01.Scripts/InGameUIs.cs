using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum UIType
{
    Timer,
    Score,
}

public class InGameUIs : MonoSington<InGameUIs>
{
    [Header("UI Components")]
    [SerializeField] private TMP_Text timer_T;
    [SerializeField] private TMP_Text score_T;

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
        timer_T.text = $"{Mathf.Round(value * 10) * 0.1f}s";
    }

    private void UpdateScoreUI(int value)
    {
        score_T.text = $"{value}";
    }
}
