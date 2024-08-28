using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class CardClickHandler : MonoBehaviour, IPointerClickHandler
{
    public enum CardType {Hand, Center};
    public CardType cardType;
    private Vector3 originalPosition;
    public static GameObject currentlySelectedCard;
    public Card card;
    private bool isSelected = false;
    private bool[] clickable;
    private List<int> pQueue;
    private Action onCenterCardClick;

    public void Initialize(Card card, List<int> pQueue, CardType cardType, Action onCenterCardClick = null)
    {
        this.card = card;
        this.clickable = card.clickable;
        this.pQueue = pQueue;
        this.cardType = cardType;
        this.onCenterCardClick = onCenterCardClick;
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
            transform.localPosition = new Vector3(originalPosition.x, originalPosition.y + 50f, originalPosition.z);
            isSelected = true;
            currentlySelectedCard = gameObject;
        }
    }

    private void CenterCardClick()
    {
        CardClickHandler selected = currentlySelectedCard.GetComponent<CardClickHandler>();
        transform.localPosition = new Vector3(selected.originalPosition.x, selected.originalPosition.y, originalPosition.z);
        currentlySelectedCard.transform.localPosition = new Vector3(-221f, -15f, originalPosition.z);

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
