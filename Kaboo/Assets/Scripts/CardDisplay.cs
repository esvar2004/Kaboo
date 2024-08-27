using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public GameObject cardPrefab;
    public List<Sprite> cardSprites;
    private Deck deck;
    public Button cardButton;
    public int sortingOrder;

    void Start()
    {
        deck = new Deck(cardSprites);

        if (cardButton != null)
        {
            cardButton.onClick.AddListener(DrawCard);
        }

        sortingOrder = 1;
    }

    void DrawCard()
    {
        Card card = deck.DrawCard();
        if (card != null)
        {
            GameObject cardGO = Instantiate(cardPrefab, transform.position, Quaternion.identity);
            cardGO.transform.SetParent(transform, false);
            cardGO.GetComponent<SpriteRenderer>().sprite = card.sprite;
            cardGO.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder++;
            cardGO.transform.localScale = new Vector3(8f, 8f, 1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
