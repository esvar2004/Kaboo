using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Game : MonoBehaviour
{
    public GameObject cardPrefab;
    public List<Sprite> cardSprites; //List of All the Sprites
    public Sprite backOfCard; //Sprite Representing Back of the Card to Keep Hidden Cards Invisible
    public Button cardButton; // Button to Draw a Card
    public Button startGame; //Button to Start the Game
    public Button discardButton; //Button to Discard the Card
    public Button winnerButton; //Button to Calculate Winner
    public Image discardPile;
    public List<GameObject> cardPrefabsInPlay; //Contains the actual GameObjects (cards) in play 
    private Deck deck; //Deck of Cards
    private List<Player> players; //List of Players
    public int currentPlayer; //Reference to the Current Player
    public List<int> pQueue; //Priority Queue Indicating Whose Turn is Next
    private bool drawn; //Checks if the player has started their turn.

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
                startPlayerTurn(players[currentPlayer]);
                startGame.gameObject.SetActive(false);
            });
        }

        cardButton.gameObject.SetActive(false);
        discardButton.gameObject.SetActive(false);
        winnerButton.onClick.AddListener(() => calculateWinner());
    }

    private void startPlayerTurn(Player player)
    {
        currVisibility(player.player_num);
        cardButton.gameObject.SetActive(true);
        cardButton.onClick.RemoveAllListeners();
        cardButton.onClick.AddListener(() => DrawCard(player));
    }

    private void DrawCard(Player player)
    {
        if(player.hasDrawnCard == true)
            return;

        Card card = deck.DrawCard();
        if (card != null)
        {
            GameObject cardGO = Instantiate(cardPrefab, transform.position, Quaternion.identity);

            if (cardGO.GetComponent<BoxCollider2D>() == null)
            {
                cardGO.AddComponent<BoxCollider2D>();
            }


            //Handles discarding logic.
            discardButton.onClick.RemoveAllListeners();
            discardButton.gameObject.SetActive(true);
            if (discardButton != null)
            {
                discardButton.onClick.AddListener(() => DiscardCard(card, cardGO));
            }

            card.visibility[player.player_num] = true;

            //Handles swapping logic.
            CardClickHandler cardClickHandler = cardGO.AddComponent<CardClickHandler>();
            cardClickHandler.Initialize(card, pQueue, CardClickHandler.CardType.Center, discardPile, () => OnCenterCardSwap(card, cardGO));

            cardGO.transform.SetParent(transform, false);
            cardGO.GetComponent<SpriteRenderer>().sprite = card.sprite;
            cardGO.GetComponent<SpriteRenderer>().sortingOrder = 1;
            cardGO.transform.localScale = new Vector3(6f, 6f, 1f);

            player.hasDrawnCard = true;
        }
    }

    //Have to assign power-ups here.
    private void DiscardCard(Card card, GameObject cardGO)
    {
        Array.Fill(card.visibility, true);

        cardGO.transform.SetParent(discardPile.transform, false);
        RectTransform objectRectTransform = cardGO.GetComponent<RectTransform>();
        RectTransform imageRectTransform = discardPile.GetComponent<RectTransform>();
        objectRectTransform.sizeDelta = imageRectTransform.sizeDelta;
        objectRectTransform.position = imageRectTransform.position;
        objectRectTransform.localScale = new Vector3(1f, 1.5f, 1f);

        discardButton.gameObject.SetActive(false);

        ProceedToNextPlayer();
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

        CardClickHandler.currentlySelectedCard = null;
        player.AddCard(card);
        player.AddCardSprite(cardGO);
        deck.cardsInPlay.Add(card);
        cardPrefabsInPlay.Add(cardGO);

        discardButton.gameObject.SetActive(false);
        
        //Proceeds to the next player's turn.
        ProceedToNextPlayer();
    }

    private void ProceedToNextPlayer()
    {
        //Controls the movement of the game.
        int playerRemoved = pQueue[0];
        players[playerRemoved].hasDrawnCard = false;
        pQueue.RemoveAt(0);
        pQueue.Add(playerRemoved);
        currentPlayer = pQueue[0];

        Player nextPlayer = players[currentPlayer];
        startPlayerTurn(nextPlayer);
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
                cardClickHandler.Initialize(players[i].hand[j], pQueue, CardClickHandler.CardType.Hand, discardPile);

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

    private void calculateWinner()
    {
        int player_winner = 1;
        int winning_hand = 100;
        for(int i = 0; i < players.Count; i++)
        {
            int hand_value = 0;
            for(int j = 0; j < players[i].hand.Count; j++)
            {
                hand_value += players[i].hand[j].value;   
            }

            if(hand_value < winning_hand)
            {
                player_winner = i + 1;
                winning_hand = hand_value;
            }

            Debug.Log("Player " + (i + 1) + ":" + hand_value);
        }
        Debug.Log("Winner: " + (player_winner) + ":" + winning_hand);
    }

    void Update()
    {

    }
}
