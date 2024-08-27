using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    public GameObject cardPrefab;
    public List<Sprite> cardSprites;
    private Deck deck;

    void Start()
    {
        deck = new Deck(cardSprites);
        DrawCard();
    }

    void DrawCard()
    {
        Card card = deck.DrawCard();
        if (card != null)
        {
            GameObject cardGO = Instantiate(cardPrefab, transform.position, Quaternion.identity);
            cardGO.GetComponent<SpriteRenderer>().sprite = card.sprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
