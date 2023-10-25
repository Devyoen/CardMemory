using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class Card
{
    public Suit Suit;
    public Number Number;
}
