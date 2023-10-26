using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardImageContainer", menuName = "Scriptable Object/CardImageContainer")]
public class CardImageContainer : ScriptableObject
{
    [SerializeField] private List<Sprite> spade = new List<Sprite>();
    [SerializeField] private List<Sprite> heart = new List<Sprite>();
    [SerializeField] private List<Sprite> diamond = new List<Sprite>();
    [SerializeField] private List<Sprite> club = new List<Sprite>();
    [SerializeField] private Sprite backImageSprite;

    public List<Sprite> Spade => spade;
    public List<Sprite> Heart => heart;
    public List<Sprite> Diamond => diamond;
    public List<Sprite> Club => club;
    public Sprite BackImageSprite => backImageSprite;

    public List<Sprite> GetSpriteList(Suit suit)
    {
        switch (suit)
        {
            case Suit.Spade:
                return Spade;
            case Suit.Heart:
                return Heart;
            case Suit.Diamond:
                return Diamond;
            case Suit.Club:
                return Club;
            default:
                return null;
        }
    }
}
