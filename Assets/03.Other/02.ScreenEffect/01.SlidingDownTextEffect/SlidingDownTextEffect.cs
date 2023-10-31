using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SlidingDownTextEffect : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private TMP_Text descriptionText;

    public void ShowEffect(string text)
    {
        descriptionText.text = text;
        animator.SetTrigger("Play");
    }
}
