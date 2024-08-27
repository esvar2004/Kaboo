using System.Collections.Generic;

public class Player
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
}
