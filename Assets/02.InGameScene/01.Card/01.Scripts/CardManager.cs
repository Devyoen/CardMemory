using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CardManager : MonoSington<CardManager>
{
    #region Values
    [Header("Setting")]
    [SerializeField] private Card cardPrefab;
    [SerializeField] private CardImageContainer cardImageContainer;

    public CardImageContainer CardImageContainer => cardImageContainer;

    [Header("Common")]
    private Card[] cards;
    [SerializeField] private Card[] selectedCards = new Card[2];
    private List<CardType> curCardsType = new List<CardType>();
    private InGameManager inGameManager;

    public Card[] Cards => cards;
    public Card[] SelectedCards => selectedCards;
    [Header("Action")]
    private Action<CardDirection> flipAll;

    public Action onAllSelected;
    #endregion

    private void Start()
    {
        inGameManager = InGameManager.instance;
    }

    #region Methods
    public void SelectCard(Card card)
    {
        card.SetCanInteraction(false);
        if (selectedCards[0] == null)
        {
            selectedCards[0] = card;
        }
        else if (selectedCards[1] == null)
        {
            selectedCards[1] = card;
            onAllSelected?.Invoke();
        }
        else
        {
            Debug.Log("오류 : 선택 가능 횟수 초과");
        }
    }

    public void SucceededFlip()
    {
        foreach (Card card in selectedCards)
        {
            card.Flip(CardDirection.Front);
            card.SetCanInteraction(false);
            Debug.Log("Succed");
        }
    }

    public void FailedFlip()
    {
        foreach (Card card in selectedCards)
        {
            card.Flip(CardDirection.Back);
            card.SetCanInteraction(true);
            Debug.Log("Failed");
        }
    }

    public void ResetSelectedCards()
    {
        selectedCards = new Card[2];
    }

    public Card CreateCard(CardType cardType)
    {
        Card card = Instantiate(cardPrefab);
        card.SetCard(cardType);
        flipAll += card.Flip;
        return card;
    }

    public void FlipAllCards(CardDirection cardDirection)
    {
        flipAll?.Invoke(cardDirection);
    }

    public void MoveAllCards(Vector3 targetPos, float time = 0.5f)
    {
        foreach (Card card in cards)
        {
            card.MoveTo(targetPos, time);
        }
    }

    public void ShuffleCards()
    {
        cards = cards.GetShuffledList();
    }

    public void SetCards()
    {
        int cardCount = inGameManager.StageSize.x * inGameManager.StageSize.y;
        cards = new Card[cardCount];
        curCardsType = CardType.GetRandomCardTypeList(cardCount); //생성할 카드들의 타입을 랜덤하게 정함
        for (int i = 0; i < cardCount; i++)
        {
            cards[i] = CreateCard(curCardsType[i]);
        }
        PositionCards();
    }

    public void PositionCards()
    {
        Vector3 centerPoint = new Vector3(-((inGameManager.StageSize.x - 1) * inGameManager.CardIntervalX * 0.5f), -((inGameManager.StageSize.y - 1) * inGameManager.CardIntervalY * 0.5f), 0);
        Vector3 tempPos = centerPoint + new Vector3(-inGameManager.CardIntervalX, (inGameManager.StageSize.y - 1) * inGameManager.CardIntervalY, 0);
        int count = 0;
        for (int y = 0; y < inGameManager.StageSize.y; y++)
        {
            for (int x = 0; x < inGameManager.StageSize.x; x++)
            {
                Vector3 cardPos = centerPoint + new Vector3(x * inGameManager.CardIntervalX, y * inGameManager.CardIntervalY, 0);
                cards[count].SetPosition(cardPos);
                count++;
            }
        }
    }

    public IEnumerator MoveAllCardsToOrigin_co()
    {
        foreach (Card card in cards)
        {
            card.MoveTo(card.OriginPos);
            yield return YieldCache.WaitForSeconds(0.3f);
        }
    }
    #endregion
}
