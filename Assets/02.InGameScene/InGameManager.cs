using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InGameManager : MonoSington<InGameManager>
{
    [SerializeField] private CardImageContainer cardImageContainer;
    [SerializeField] private Vector2Int stageSize = new Vector2Int();
    [SerializeField] private Card cardPrefab;
    [SerializeField] private float cardIntervalX = 1f;
    [SerializeField] private float cardIntervalY = 1f;
    [SerializeField] private bool canFlip = true;

    private Action<CardDirection> flipAll;

    public CardImageContainer CardImageContainer => cardImageContainer;
    public Vector2Int StageSize => stageSize;
    public float CardIntervalX => cardIntervalX;
    public float CardIntervalY => cardIntervalY;
    public bool CanFlip => canFlip;

    private void Start()
    {
        Vector3 middlePoint = new Vector3(-((StageSize.x - 1) * CardIntervalX * 0.5f), -((StageSize.y - 1) * CardIntervalY * 0.5f), 0);
        Debug.Log(middlePoint);
        for (int y = 0; y < StageSize.y; y++)
        {
            for (int x = 0; x < StageSize.x; x++)
            {
                Card card = Instantiate(cardPrefab, middlePoint + new Vector3(x * CardIntervalX, y * CardIntervalY, 0), Quaternion.identity);
                card.SetCard(Suit.Spade, Number.Ace);
                flipAll += card.Flip;
            }
        }
    }
}
