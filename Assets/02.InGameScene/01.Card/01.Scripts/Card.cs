using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Suit
{
    Spade = 0,
    Heart = 1,
    Diamond = 2,
    Club = 3,
}

public enum Number
{
    Ace = 0,
    Two = 1,
    Three = 2,
    Four = 3,
    Five = 4,
    Six = 5,
    Seven = 6,
    Eight = 7,
    Nine = 8,
    Ten = 9,
    Jack = 10,
    Queen = 11,
    King = 12
}

[System.Serializable]
public struct CardType
{
    private Suit suit;
    private Number number;

    public Suit Suit => suit;
    public Number Number => number;

    public CardType(Suit suit, Number number)
    {
        this.suit = suit;
        this.number = number;
    }

    public override bool Equals(object? obj) => obj is CardType other && this.Equals(other);

    public override int GetHashCode() => (suit, number).GetHashCode();

    public bool Equals(CardType cardType) => cardType.Suit == this.Suit && cardType.Number == this.Number;

    public static bool operator ==(CardType type1, CardType type2) => type1.Equals(type2);
    public static bool operator !=(CardType type1, CardType type2) => !(type1 == type2);

    public static List<CardType> GetRandomCardTypeList(int size)
    {
        List<CardType> cards = new List<CardType>();//모든 종류의 카드가 들어간 리스트
        for (int i = 0; i < System.Enum.GetNames(typeof(Suit)).Length; i++)
        {
            for (int j = 0; j < System.Enum.GetNames(typeof(Number)).Length; j++)
            {
                cards.Add(new CardType((Suit)i, (Number)j));
            }
        }
        cards.GetShuffledList();//섞음
        List<CardType> result = new List<CardType>();
        for (int i = 0; i < (size / 2); i++)
        {
            result.Add(cards[i]);
            result.Add(cards[i]);//두장씩 넣어서 한 쌍을 만듦
        }
        return result;
    }
}

public enum CardDirection
{
    Front,
    Back
}

public class Card : MonoBehaviour
{
    [SerializeField] private SpriteRenderer frontImage;
    [SerializeField] private SpriteRenderer backImage;
    [SerializeField] private Animator animator;

    private CardType cardType;
    private CardDirection cardDirection;
    private Vector3 originPos;
    private bool canInteraction = true;

    public CardType CardType => cardType;
    public CardDirection CardDirection => cardDirection;
    public Vector3 OriginPos => originPos;
    public bool CanInteraction => canInteraction;

    private Coroutine MoveTo_corutine;

    public void SetCanInteraction(bool value)
    {
        canInteraction = value;
    }

    public void SetCard(CardType cardType)
    {
        this.cardType = cardType;
        List<Sprite> cardImageList = CardManager.instance.CardImageContainer.GetSpriteList(cardType.Suit);
        frontImage.sprite = cardImageList[(int)cardType.Number];
        backImage.sprite = CardManager.instance.CardImageContainer.BackImageSprite;
        cardDirection = CardDirection.Front;
        animator.SetBool("IsFrontSide", true);
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
        originPos = pos;
    }

    public void Flip()
    {
        Flip(CardDirection == CardDirection.Front ? CardDirection.Back : CardDirection.Front);
    }

    public void Flip(CardDirection direction)
    {
        cardDirection = direction;
        animator.SetBool("IsFrontSide", direction == CardDirection.Front ? true : false);
    }

    public void MoveTo(Vector3 pos, float time = 0.3f)
    {
        if (MoveTo_corutine != null)
            StopCoroutine(MoveTo_corutine);
        MoveTo_corutine = StartCoroutine(MoveTo_co(pos, time));
    }

    private IEnumerator MoveTo_co(Vector3 pos, float time)
    {
        Vector3 originPos = transform.position;
        for (float i = 0; i <= 1; i += Time.deltaTime / time)
        {
            transform.position = Vector3.Lerp(originPos, pos, i);
            yield return null;
        }
        transform.position = pos;
    }
}
