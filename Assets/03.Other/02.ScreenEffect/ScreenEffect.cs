using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenEffect : MonoSington<ScreenEffect>
{
    [SerializeField] private SlidingDownTextEffect slidingDownTextEffect;

    public void SlidingDownText(string text)
    {
        slidingDownTextEffect.ShowEffect(text);
    }
}
