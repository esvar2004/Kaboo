using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Suit {Hearts, Diamonds, Spades, Clubs}
public enum Rank {Ace, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King}

public class Card : MonoBehaviour
{
    // Start is called before the first frame update
    public Suit suit;
    public Rank rank;
    public Sprite sprite;

    public Card(Suit suit, Rank rank, Sprite sprite)
    {
        this.suit = suit;
        this.rank = rank;
        this.sprite = sprite;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
