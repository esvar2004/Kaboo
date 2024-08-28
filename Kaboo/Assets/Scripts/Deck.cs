using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    private List<Card> cards;
    public List<Card> cardsInPlay;

    public Deck(List<Sprite> cardSprites)
    {
        cards = new List<Card>();
        cardsInPlay = new List<Card>();
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
        for (int i = 0; i < cards.Count; i++)
        {
            Card temp = cards[i];
            int randomIndex = Random.Range(i, cards.Count);
            cards[i] = cards[randomIndex];
            cards[randomIndex] = temp;
        }
    }

    public Card DrawCard()
    {
        if (cards.Count == 0) return null;
        Card drawnCard = cards[0];
        cards.RemoveAt(0);
        return drawnCard;
    }

    public void DistributeCards(List<Player> players, int cardsPerPlayer)
    {
        foreach (Player player in players)
        {
            for(int i = 0; i < cardsPerPlayer; i++)
            {
                Card card = DrawCard();
                cardsInPlay.Add(card);
                card.visibility[player.player_num] = true;
                card.clickable[player.player_num] = true;
                player.AddCard(card);
            }
        }
    }
}
