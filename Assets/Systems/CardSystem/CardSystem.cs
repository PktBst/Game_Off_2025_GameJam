using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CardSystem : MonoBehaviour
{
    [Header("References")]
    public GameObject cardPrefab;
    public RectTransform cardHolder;
    public Transform DeckLocation;

    [Header("Spawn Animation Settings")]
    public float PopDistance = 800f;
    public float SwipeVariance = 300f;
    public float SpawnRotationVariance = 45f;

    [Header("Settings")]
    public int startingCardCount = 5;
    [Range(0, 2)] public float CardVisibility = 0.8f;
    [Range(0, 90)] public float MaxCardRotation = 10f;
    public float ArchHeight = 40f;

    private CardScript currentlySelectedCard;

    public void Init()
    {
        if (GameManager.Instance.InputActionAsset.FindAction("Click") != null)
            GameManager.Instance.InputActionAsset.FindAction("Click").performed += UseSelectedCard;

        for (int i = 0; i < startingCardCount; i++)
        {
            AddCard();
        }
    }

    public void AddCard(int specificIndex = -1)
    {
        GameObject newObj = Instantiate(cardPrefab, cardHolder);
        CardScript card = newObj.GetComponent<CardScript>();

        float startY = -PopDistance;
        float startX = Random.Range(-SwipeVariance, SwipeVariance);
        newObj.transform.localPosition = new Vector3(startX, startY, 0);

        // Randomize Initial Rotation 
        float randomTilt = Random.Range(-SpawnRotationVariance, SpawnRotationVariance);
        newObj.transform.localRotation = Quaternion.Euler(0, 0, randomTilt);

        if (specificIndex >= 0 && specificIndex < cardHolder.childCount)
        {
            newObj.transform.SetSiblingIndex(specificIndex);
        }

        card.PlaceableObjectData = GameManager.Instance.ObjectPlacementSystem.GetRandomPlaceableObject();
        card.Init();

        ReorganizeHand();
    }

    void UseSelectedCard(InputAction.CallbackContext ctx)
    {
        if (currentlySelectedCard == null) return;
        if (GameManager.Instance.GridSystem.CurrentTile == null) return;

        GameManager.Instance.ObjectPlacementSystem.SpawnPlaceableObjectAtTile(
            GameManager.Instance.GridSystem.CurrentTile,
            currentlySelectedCard.PlaceableObjectData.Type
            );

        CardScript cardToRemove = currentlySelectedCard;
        int oldIndex = cardToRemove.transform.GetSiblingIndex(); 

        DeselectAll();

        cardToRemove.transform.SetParent(null);
        Destroy(cardToRemove.gameObject);

        AddCard(oldIndex);
    }


    //Arranges all cards in arch
    private void ReorganizeHand()
    {
        int currentCount = cardHolder.childCount;
        if (currentCount == 0) return;

        Vector2 centrePoint = cardHolder.rect.center;
        RectTransform cardRect = cardPrefab.GetComponent<RectTransform>();
        float cardWidth = cardRect.rect.width;

        float totalXneeded = (cardWidth * CardVisibility) * (currentCount - 1);
        float startPosX = centrePoint.x - totalXneeded / 2;

        for (int i = 0; i < currentCount; i++)
        {
            Transform child = cardHolder.GetChild(i);
            CardScript cardScript = child.GetComponent<CardScript>();

            // Calculate Arch Position
            float xPos = startPosX + (i * cardWidth * CardVisibility);

            // Normalize index (0 to 1) for Arch, dont divide by zero
            float normalizedIndex = currentCount > 1 ? i / (float)(currentCount - 1) : 0.5f;

            float yOffset = Mathf.Sin(normalizedIndex * Mathf.PI) * ArchHeight;
            Vector3 newLocalPos = new Vector2(xPos, centrePoint.y + yOffset);

            // Calculate Rotation
            float currentRotation = Mathf.Lerp(MaxCardRotation, -MaxCardRotation, normalizedIndex);
            Quaternion newRot = Quaternion.Euler(0f, 0f, currentRotation);

            //send data to the card
            if (cardScript != null)
            {
                cardScript.UpdateBaseTransform(newLocalPos, newRot);
            }
        }
    }


    public void OnCardClicked(CardScript newlyClickedCard)
    {
        if (currentlySelectedCard != null && currentlySelectedCard != newlyClickedCard)
        {
            currentlySelectedCard.Deselect();
        }

        if (currentlySelectedCard == newlyClickedCard)
        {
            currentlySelectedCard.Deselect();
            currentlySelectedCard = null;
        }
        else
        {
            currentlySelectedCard = newlyClickedCard;
        }
    }

    public void DeselectAll()
    {
        if (currentlySelectedCard != null)
        {
            currentlySelectedCard.Deselect();
            currentlySelectedCard = null;
        }
    }
}