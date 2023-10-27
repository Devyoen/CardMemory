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
        SetCards();
    }

    private void SetCards()
    {
        Vector3 centerPoint = new Vector3(-((StageSize.x - 1) * CardIntervalX * 0.5f), -((StageSize.y - 1) * CardIntervalY * 0.5f), 0);

        int cardCount = StageSize.x * StageSize.y;
        List<CardType> cardTypes = CardType.GetRandomCardTypeList(cardCount);
        int count = 0;
        for (int y = 0; y < StageSize.y; y++)
        {
            for (int x = 0; x < StageSize.x; x++)
            {
                Vector3 cardPos = centerPoint + new Vector3(x * CardIntervalX, y * CardIntervalY, 0);
                CardType cardType = cardTypes[count];

                CreateCard(cardPos, cardType);
                count++;
            }
        }
    }

    private void CreateCard(Vector3 pos, CardType cardType)
    {
        Card card = Instantiate(cardPrefab, pos, Quaternion.identity);
        card.SetCard(cardType);
        flipAll += card.Flip;
    }
}
