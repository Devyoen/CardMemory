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

    private Suit suit;
    private Number number;
    private CardDirection cardDirection;
    private bool canInteraction = true;

    public Suit Suit => suit;
    public Number Number => number;
    public CardDirection CardDirection => cardDirection;
    public bool CanInteraction => canInteraction;

    public void SetCanInteraction(bool value)
    {
        canInteraction = value;
    }

    public void SetCard(Suit suit, Number number)
    {
        this.suit = suit;
        this.number = number;
        List<Sprite> cardImageList = InGameManager.instance.CardImageContainer.GetSpriteList(suit);
        frontImage.sprite = cardImageList[(int)number];
        backImage.sprite = InGameManager.instance.CardImageContainer.BackImageSprite;
        cardDirection = CardDirection.Front;
        animator.SetBool("IsFrontSide", true);
    }

    public void Flip()
    {
        Flip(CardDirection == CardDirection.Front ? CardDirection.Back : CardDirection.Front);
    }

    public void Flip(CardDirection direction)
    {
        if (!CanFlip())
            return;
        cardDirection = direction;
        animator.SetBool("IsFrontSide", direction == CardDirection.Front ? true : false);
    }

    public bool CanFlip() => InGameManager.instance.CanFlip && CanInteraction;
}
