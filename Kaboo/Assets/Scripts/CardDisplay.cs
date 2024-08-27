using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public GameObject cardPrefab;
    public List<Sprite> cardSprites;
    public Sprite backOfCard;
    public Button cardButton;
    public Button nextTurn;
    public List<GameObject> cardPrefabsInPlay;
    private Deck deck;
    private List<Player> players;
    public int sortingOrder;
    public int currentPlayer;

    void Start()
    {
        
        deck = new Deck(cardSprites);
        players = new List<Player>();
        sortingOrder = 1;

        for (int i = 0; i < 4; i++) // Create 4 players
        {
            players.Add(new Player(i));
        }

        deck.DistributeCards(players, 4); // Distribute 4 cards to each player

        DisplayPlayerStartingHands();

        if (cardButton != null)
        {
            cardButton.onClick.AddListener(DrawCard);
        }

        currVisibility(currentPlayer);

        if (nextTurn != null)
        {
            nextTurn.onClick.AddListener(() => currVisibility(++currentPlayer));
        }
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

    void DisplayPlayerStartingHands()
    {
        // Define positions for each player's cards (center points)
        Vector3[] playerPositions = new Vector3[]
        {
            new Vector3(0f, 250f, 0),   // Top (Player 1)
            new Vector3(0f, -250f, 0),  // Bottom (Player 2)
            new Vector3(-450f, 0f, 0),  // Left (Player 3)
            new Vector3(450f, 0f, 0)    // Right (Player 4)
        };

        // Define offsets to position the 4 cards in a 2x2 grid relative to the player's position
        Vector3[,] cardOffsets = new Vector3[,]
        {
        { new Vector3(-50f, 65f, 0), new Vector3(50f, 65f, 0),    // Top row (left, right)
          new Vector3(-50f, -65f, 0), new Vector3(50f, -65f, 0) },// Bottom row (left, right)

        { new Vector3(-50f, 65f, 0), new Vector3(50f, 65f, 0),    // Top row (left, right)
          new Vector3(-50f, -65f, 0), new Vector3(50f, -65f, 0) },// Bottom row (left, right)

          { new Vector3(-50f, 65f, 0), new Vector3(50f, 65f, 0),    // Top row (left, right)
          new Vector3(-50f, -65f, 0), new Vector3(50f, -65f, 0) },// Bottom row (left, right)
          
          { new Vector3(-50f, 65f, 0), new Vector3(50f, 65f, 0),    // Top row (left, right)
          new Vector3(-50f, -65f, 0), new Vector3(50f, -65f, 0) },// Bottom row (left, right)
    };

        for (int i = 0; i < players.Count; i++)
        {
            Vector3 playerPosition = playerPositions[i];

            for (int j = 0; j < players[i].hand.Count; j++)
            {
                // Use the cardOffsets for 2x2 grid positioning
                Vector3 cardPosition = playerPosition + cardOffsets[i, j];
                GameObject cardGO = Instantiate(cardPrefab, cardPosition, Quaternion.identity);
                cardPrefabsInPlay.Add(cardGO);
                cardGO.transform.SetParent(transform, false);
                cardGO.GetComponent<SpriteRenderer>().sprite = players[i].hand[j].sprite;
                cardGO.GetComponent<SpriteRenderer>().sortingOrder = 1;
                cardGO.transform.localScale = new Vector3(6f, 6f, 1f);
            }
        }
    }

    void displayCard(int num, bool visibility){

    }

    void currVisibility(int playerIndex)
    {
        playerIndex = playerIndex % 4;
        for(int i = 0; i < deck.cardsInPlay.Count; i++)
        {
            if(deck.cardsInPlay[i].visibility[playerIndex])
                cardPrefabsInPlay[i].GetComponent<SpriteRenderer>().sprite = deck.cardsInPlay[i].sprite;
            else
                cardPrefabsInPlay[i].GetComponent<SpriteRenderer>().sprite = backOfCard;
        }
    }

    void Update()
    {

    }
}
