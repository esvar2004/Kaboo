using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Game : MonoBehaviour
{
    public GameObject cardPrefab; // 
    public List<Sprite> cardSprites; //List of All the Sprites
    public Sprite backOfCard; //Sprite Representing Back of the Card to Keep Hidden Cards Invisible
    public Button cardButton; // Button to Draw a Card
    public Button nextTurn; //Button to Shift to Next Player
    public Button startGame; //Button to Start the Game
    public List<GameObject> cardPrefabsInPlay; //Contains the actual GameObjects (cards) in play 
    private Deck deck; //Deck of Cards
    private List<Player> players; //List of Players
    public int currentPlayer; //Reference to the Current Player
    public List<int> pQueue; //Priority Queue Indicating Whose Turn is Next

    void Start()
    {
        deck = new Deck(cardSprites);
        players = new List<Player>();
        pQueue = new List<int>(){0, 1, 2, 3};
        currentPlayer = pQueue[0];

        for (int i = 0; i < 4; i++) // Create 4 players
        {
            players.Add(new Player(i));
        }

        if (startGame != null)
        {
            startGame.onClick.AddListener(() => {
                deck.DistributeCards(players, 4); // Distribute 4 cards to each player
                DisplayPlayerStartingHands();
                currVisibility(currentPlayer);
            });
        }

        if (cardButton != null)
        {
            cardButton.onClick.AddListener(() => DrawCard(players[currentPlayer]));
        }
        
        // if (nextTurn != null)
        // {
        //     nextTurn.onClick.AddListener(() => {
        //         int playerRemoved = pQueue[0];
        //         pQueue.RemoveAt(0);
        //         pQueue.Add(playerRemoved);
        //         currentPlayer = pQueue[0];
        //         currVisibility(currentPlayer);
        //     });
        // }
    }

    private void DrawCard(Player player)
    {
        Card card = deck.DrawCard();
        if (card != null)
        {
            GameObject cardGO = Instantiate(cardPrefab, transform.position, Quaternion.identity);

            if (cardGO.GetComponent<BoxCollider2D>() == null)
            {
                cardGO.AddComponent<BoxCollider2D>();
            }

            card.visibility[player.player_num] = true;

            CardClickHandler cardClickHandler = cardGO.AddComponent<CardClickHandler>();
            cardClickHandler.Initialize(card, pQueue, CardClickHandler.CardType.Center, () => OnCenterCardSwap(card, cardGO));

            cardGO.transform.SetParent(transform, false);
            cardGO.GetComponent<SpriteRenderer>().sprite = card.sprite;
            cardGO.GetComponent<SpriteRenderer>().sortingOrder = 1;
            cardGO.transform.localScale = new Vector3(6f, 6f, 1f);

            // if(CardClickHandler.centerCardSwapped)
            // {
            //     player.AddCard(card);
            //     player.AddCardSprite(cardGO);
            //     Debug.Log(player.hand);
            // }
        }
    }

    private void OnCenterCardSwap(Card card, GameObject cardGO)
    {
        GameObject selectedCard = CardClickHandler.currentlySelectedCard;
        Player player = players[currentPlayer];

        Array.Fill(selectedCard.GetComponent<CardClickHandler>().card.visibility, true);
        player.hand.Remove(selectedCard.GetComponent<CardClickHandler>().card);
        player.spritesHand.Remove(selectedCard);
        deck.cardsInPlay.Remove(selectedCard.GetComponent<CardClickHandler>().card);
        cardPrefabsInPlay.Remove(selectedCard);

        selectedCard = null;
        player.AddCard(card);
        player.AddCardSprite(cardGO);
        deck.cardsInPlay.Add(card);
        cardPrefabsInPlay.Add(cardGO);
        
        int playerRemoved = pQueue[0];
        pQueue.RemoveAt(0);
        pQueue.Add(playerRemoved);
        currentPlayer = pQueue[0];
        
        ProceedToNextPlayer();
    }

    private void ProceedToNextPlayer()
    {
        Player nextPlayer = players[currentPlayer];
        DrawCard(nextPlayer); // Draw a card for the next player

        // Update UI or visibility settings
        currVisibility(currentPlayer);
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

                players[i].spritesHand.Add(cardGO);

                if (cardGO.GetComponent<BoxCollider2D>() == null)
                {
                    cardGO.AddComponent<BoxCollider2D>();
                }

                CardClickHandler cardClickHandler = cardGO.AddComponent<CardClickHandler>();
                cardClickHandler.Initialize(players[i].hand[j], pQueue, CardClickHandler.CardType.Hand);

                cardPrefabsInPlay.Add(cardGO);
                cardGO.transform.SetParent(transform, false);
                cardGO.GetComponent<SpriteRenderer>().sprite = players[i].hand[j].sprite;
                cardGO.GetComponent<SpriteRenderer>().sortingOrder = 1;
                cardGO.transform.localScale = new Vector3(6f, 6f, 1f);
            }
        }
    }

    void currVisibility(int playerIndex)
    {
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
