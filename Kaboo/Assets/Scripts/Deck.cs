using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    private List<Card> cards;

    public Deck(List<Sprite> cardSprites)
    {
        cards = new List<Card>();
        InitializeDeck(cardSprites);
        Shuffle();
    }

    private void InitializeDeck(List<Sprite> cardSprites)
    {
        int spriteIndex = 0;
        foreach (Suit suit in System.Enum.GetValues(typeof(Suit)))
        {
            foreach (Rank rank in System.Enum.GetValues(typeof(Rank)))
            {
                cards.Add(new Card(suit, rank, cardSprites[spriteIndex]));
                spriteIndex++;
            }
        }
    }

    public void Shuffle()
    {
        // Implement a shuffle algorithm like Fisher-Yates
    }

    public Card DrawCard()
    {
        if (cards.Count == 0) return null;
        Card drawnCard = cards[0];
        cards.RemoveAt(0);
        return drawnCard;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
