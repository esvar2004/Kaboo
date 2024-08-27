using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public List<Sprite> cardSprites;
    private Deck deck;

    void Start()
    {
        deck = new Deck(cardSprites);
    }
}

