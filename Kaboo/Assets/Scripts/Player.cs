using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public List<Card> hand;
    public int player_num;

    public Player(int player_num)
    {
        hand = new List<Card>();
        this.player_num = player_num;
    }

    public void AddCard(Card card)
    {
        hand.Add(card);
    }

    public void update()
    {

    }
}
