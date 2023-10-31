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
    private Card[] cards = new Card[0];
    [SerializeField] private Card[] selectedCards = new Card[2];
    private List<CardType> curCardsType = new List<CardType>();

    public Card[] Cards => cards;
    public Card[] SelectedCards => selectedCards;
    [Header("Action")]
    private Action<CardDirection> flipAll;

    public Action onAllSelected;
    #endregion

    #region Methods
    public void Init()
    {
        RemoveCards();
        flipAll = null;
        InGameManager.instance.InitSuccessCount();
    }

    private void RemoveCards()
    {
        if (cards.Length > 0)
        {
            foreach (Card card in cards)
            {
                Destroy(card.gameObject);
            }
        }
        cards = new Card[0];
    }

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
            //card.Flip(CardDirection.Front);
            SoundManager.instance.Play(AudioClipName.Success);
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

    public void MoveAllCards(Vector3 targetPos, float time = 0.3f)
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

    public void ShuffleCards_Replay(Card[] _cards)
    {
        cards = _cards;
    }

    public void SetCards()
    {
        int cardCount = InGameManager.instance.StageSize.x * InGameManager.instance.StageSize.y;
        cards = new Card[cardCount];
        curCardsType = CardType.GetRandomCardTypeList(cardCount); //생성할 카드들의 타입을 랜덤하게 정함
        for (int i = 0; i < cardCount; i++)
        {
            cards[i] = CreateCard(curCardsType[i]);
        }
        PositionCards();
    }

    public void SetCards_Replay(List<CardType> cardTypes)
    {
        int cardCount = InGameManager.instance.StageSize.x * InGameManager.instance.StageSize.y;
        cards = new Card[cardCount];
        curCardsType = cardTypes;
        for (int i = 0; i < cardCount; i++)
        {
            cards[i] = CreateCard(curCardsType[i]);
        }
        PositionCards();
    }

    public void PositionCards()
    {
        Vector3 centerPoint = new Vector3(-((InGameManager.instance.StageSize.x - 1) * InGameManager.instance.CardIntervalX * 0.5f), -((InGameManager.instance.StageSize.y - 1) * InGameManager.instance.CardIntervalY * 0.5f), 0);
        Vector3 tempPos = centerPoint + new Vector3(-InGameManager.instance.CardIntervalX, (InGameManager.instance.StageSize.y - 1) * InGameManager.instance.CardIntervalY, 0);
        int count = 0;
        for (int y = 0; y < InGameManager.instance.StageSize.y; y++)
        {
            for (int x = 0; x < InGameManager.instance.StageSize.x; x++)
            {
                Vector3 cardPos = centerPoint + new Vector3(x * InGameManager.instance.CardIntervalX, y * InGameManager.instance.CardIntervalY, 0);
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
            yield return YieldCache.WaitForSeconds(0.15f);
        }
    }
    #endregion
}
