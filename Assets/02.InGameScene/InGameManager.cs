using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum PlayerState
{
    canAct,
    canNotAct
}

public class InGameManager : MonoSington<InGameManager>
{
    #region Values
    [Header ("Commons")]
    [SerializeField] private Card cardPrefab;
    [SerializeField] private CardImageContainer cardImageContainer;
    public CardImageContainer CardImageContainer => cardImageContainer;

    [Header ("GameSetting")]
    [SerializeField] private Vector2Int stageSize = new Vector2Int();
    public Vector2Int StageSize => stageSize;

    [Space (10f)]
    [SerializeField] private float cardIntervalX = 1f;
    [SerializeField] private float cardIntervalY = 1f;
    public float CardIntervalX => cardIntervalX;
    public float CardIntervalY => cardIntervalY;

    [Space (10f)]
    [SerializeField] private Vector3 cardsMixedPos;

    [Header("Info")]
    [SerializeField] private int score = 0;
    [SerializeField] private float timer = 0;
    public int Score => score;
    public float Timer => timer;

    //상태 변수들
    [Header ("StateValues")]
    private PlayerState playerState;
    public PlayerState PlayerState => playerState;

    //기타
    private Card[] cards;
    [SerializeField] private Stack<Card> selectedCards = new Stack<Card>();
    private Action<CardDirection> flipAll;
    private List<CardType> curCardsType = new List<CardType>();
    #endregion

    #region SetValueMethods
    public void SetPlayerState(PlayerState state)
    {
        playerState = state;
    }

    public void AddScore(int value)
    {
        score += value;
    }
    #endregion

    private void Start()
    {
        StartCoroutine(GamePlayControl_co());
    }

    private IEnumerator GamePlayControl_co()
    {
        yield return SetGame_co();
        yield return StartGame_co();
    }

    private IEnumerator SetGame_co()
    {
        SetPlayerState(PlayerState.canNotAct);
        SetCards();
        yield return YieldCache.WaitForSeconds(2f);
        FlipAllCards(CardDirection.Back);
        yield return YieldCache.WaitForSeconds(1f);
        MoveAllCards(cardsMixedPos);
        ShuffleCards();
        PositionCards(cards);
        yield return YieldCache.WaitForSeconds(2f);
        yield return MoveAllCardsToOrigin_co();
        yield return YieldCache.WaitForSeconds(0.5f);
    }

    private void SetCards()
    {
        int cardCount = StageSize.x * StageSize.y;
        cards = new Card[cardCount];
        curCardsType = CardType.GetRandomCardTypeList(cardCount); //생성할 카드들의 타입을 랜덤하게 정함
        for (int i = 0; i < cardCount; i++)
        {
            cards[i] = CreateCard(curCardsType[i]);
        }
        PositionCards(cards);
    }

    private IEnumerator MoveAllCardsToOrigin_co()
    {
        foreach (Card card in cards)
        {
            card.MoveTo(card.OriginPos);
            yield return YieldCache.WaitForSeconds(0.3f);
        }
    }

    private IEnumerator StartGame_co()
    {
        SetPlayerState(PlayerState.canAct);
        while (true)
        {
            yield return SelectCard_co();
            yield return CheakCardMatching_co();
        }
    }

    private IEnumerator SelectCard_co()
    {
        selectedCards.Clear();
        SetPlayerState(PlayerState.canAct);
        while (selectedCards.Count < 2)
        {
            yield return null;
        }
    }

    private void EndCardSelect()
    {
        SetPlayerState(PlayerState.canNotAct);
    }

    private IEnumerator CheakCardMatching_co()
    {
        yield return YieldCache.WaitForSeconds(0.5f);
        Card[] _cards = new Card[2];
        _cards[0] = selectedCards.Pop();
        _cards[1] = selectedCards.Pop();
        if (_cards[0].CardType == _cards[1].CardType)
        {
            foreach (Card card in _cards)
            {
                card.Flip(CardDirection.Front);
                card.SetCanInteraction(false);
            }
            AddScore(100);
        }
        else
        {
            foreach (Card card in _cards)
            {
                card.Flip(CardDirection.Back);
                card.SetCanInteraction(true);
            }
        }
        yield return YieldCache.WaitForSeconds(0.5f);
    }

    #region Methods

    public void SelectCard(Card card)
    {
        selectedCards.Push(card);
        card.SetCanInteraction(false);
        if (selectedCards.Count >= 2)
        {
            EndCardSelect();
        }
    }

    private Card CreateCard(CardType cardType)
    {
        Card card = Instantiate(cardPrefab);
        card.SetCard(cardType);
        flipAll += card.Flip;
        return card;
    }

    private void ShuffleCards()
    {
        cards = cards.GetShuffledList();
    }

    private void FlipAllCards(CardDirection cardDirection)
    {
        flipAll?.Invoke(cardDirection);
    }

    private void PositionCards(Card[] cards)
    {
        Vector3 centerPoint = new Vector3(-((StageSize.x - 1) * CardIntervalX * 0.5f), -((StageSize.y - 1) * CardIntervalY * 0.5f), 0);
        cardsMixedPos = centerPoint + new Vector3(-CardIntervalX, (StageSize.y - 1) * CardIntervalY, 0);
        int count = 0;
        for (int y = 0; y < StageSize.y; y++)
        {
            for (int x = 0; x < StageSize.x; x++)
            {
                Vector3 cardPos = centerPoint + new Vector3(x * CardIntervalX, y * CardIntervalY, 0);
                cards[count].SetPosition(cardPos);
                count++;
            }
        }
    }

    private void MoveAllCards(Vector3 targetPos, float time = 0.5f)
    {
        Debug.Log("MoveAllCards");
        foreach (Card card in cards)
        {
            card.MoveTo(targetPos, time);
        }
    }
    #endregion
}
