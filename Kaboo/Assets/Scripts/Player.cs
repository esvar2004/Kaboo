using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public List<Card> hand;
    public List<GameObject> spritesHand;
    public int player_num;

    public Player(int player_num)
    {
        hand = new List<Card>();
        spritesHand = new List<GameObject>();
        this.player_num = player_num;
    }

    public void AddCard(Card card)
    {
        hand.Add(card);
    }

    public void AddCardSprite(GameObject cardSprite)
    {
        spritesHand.Add(cardSprite);
    }

    public void update()
    {

    }
}
