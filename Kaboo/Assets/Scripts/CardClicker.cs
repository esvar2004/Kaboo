using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class CardClickHandler : MonoBehaviour, IPointerClickHandler
{
    public static GameObject currentlySelectedCard;
    public enum CardType {Hand, Center};
    public CardType cardType;
    private Vector3 originalPosition;
    public Image discardPile;
    public Card card;
    private bool isSelected = false;
    private bool[] clickable;
    private List<int> pQueue;
    private Action onCenterCardClick;

    public void Initialize(Card card, List<int> pQueue, CardType cardType, Image discardPile, Action onCenterCardClick = null)
    {
        this.card = card;
        this.clickable = card.clickable;
        this.pQueue = pQueue;
        this.cardType = cardType;
        this.onCenterCardClick = onCenterCardClick;
        this.discardPile = discardPile;
    }

    private void Start()
    {
        // Store the original position of the card
        originalPosition = transform.localPosition;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (cardType)
        {
            case CardType.Hand:
                HandCardClick();
                break;

            case CardType.Center:
                CenterCardClick();
                break;
        }
    }

    private void HandCardClick()
    {
        if (!isSelected && clickable[pQueue[0]])
        {
            if (currentlySelectedCard != null && currentlySelectedCard != gameObject)
            {
                CardClickHandler previousHandler = currentlySelectedCard.GetComponent<CardClickHandler>();
                if (previousHandler != null)
                {
                    previousHandler.DeselectCard();
                }
            }
            // If not selected, move the card up to indicate selection
            originalPosition = transform.localPosition;
            transform.localPosition = new Vector3(originalPosition.x, originalPosition.y + 50f, originalPosition.z);
            isSelected = true;
            currentlySelectedCard = gameObject;
        }
    }

    private void CenterCardClick()
    {
        //Places the swapped card in the place of the discarded card in the player's hand.
        CardClickHandler selected = currentlySelectedCard.GetComponent<CardClickHandler>();
        transform.localPosition = new Vector3(selected.originalPosition.x, selected.originalPosition.y, originalPosition.z);

        //Places the discarded card in the discard pile.
        currentlySelectedCard.transform.SetParent(discardPile.transform, false);
        RectTransform objectRectTransform = currentlySelectedCard.GetComponent<RectTransform>();
        RectTransform imageRectTransform = discardPile.GetComponent<RectTransform>();
        objectRectTransform.sizeDelta = imageRectTransform.sizeDelta;
        objectRectTransform.position = imageRectTransform.position;
        objectRectTransform.localScale = new Vector3(1f, 1.5f, 1f);

        cardType = CardClickHandler.CardType.Hand;
        clickable[pQueue[0]] = true;
        onCenterCardClick?.Invoke();
    }

    public static GameObject GetCurrentlySelectedCard()
    {
        return currentlySelectedCard;
    }

    public void DeselectCard()
    {
        isSelected = false;
        transform.localPosition = originalPosition;
    }
}
