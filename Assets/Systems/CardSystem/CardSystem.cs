using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CardSystem : MonoBehaviour
{
    [Header("References")]
    public bool DebugMode;
    public RectTransform LootCardHolder;
    public RectTransform ShopCardHolder;
    public TextMeshProUGUI CostPrefab;
    public GameObject cardPrefab;
    public RectTransform cardHolder;
    public Canvas LootSelectorCanvas;
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

    public bool IsShopOpen => ShopCardHolder == null ? false : ShopCardHolder.gameObject.activeInHierarchy;

    public void Init()
    {
        if (GameManager.Instance.InputActionAsset.FindAction("Click") != null)
            GameManager.Instance.InputActionAsset.FindAction("Click").performed += UseSelectedCard;

        for (int i = 0; i < startingCardCount; i++)
        {
            AddCard();
        }
    }

    public void AddCard(int specificIndex = -1, PlaceableObject_SO optionalObjData = null)
    {
        GameObject newObj = (optionalObjData)? AddCardByData(optionalObjData) : GenerateCanPopulateRandomCard();
        newObj.transform.SetParent(cardHolder);

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

        ReorganizeHand();
    }

    public GameObject AddCardByData(PlaceableObject_SO objData)
    {
        GameObject newObj = Instantiate(cardPrefab,cardHolder);
        CardScript card = newObj.GetComponent<CardScript>();
        card.PlaceableObjectData = objData;
        card.Init();
        return newObj;
    }

    [Button]
    public void openLootMenu()
    {
        LootSelectorCanvas.gameObject.SetActive(true);
        GenerateLoot();

    }

    [Button]
    public void openShopMenu()
    {
        ShopCardHolder.transform.parent.gameObject.SetActive(true);
        GenerateShop();
    }
    public GameObject GenerateCanPopulateRandomCard()
    {
        GameObject newObj = Instantiate(cardPrefab);
        CardScript card = newObj.GetComponent<CardScript>();
        card.PlaceableObjectData = GameManager.Instance.ObjectPlacementSystem.GetRandomPlaceableObject();
        card.Init();

        return newObj;
    }

    void UseSelectedCard(InputAction.CallbackContext ctx)
    {
        if (currentlySelectedCard == null) return;
        if (GameManager.Instance.GridSystem.CurrentTile == null) return;

       if( !GameManager.Instance.ObjectPlacementSystem.SpawnPlaceableObjectAtTile(
            GameManager.Instance.GridSystem.CurrentTile,
            currentlySelectedCard.PlaceableObjectData.Type
            ))
        {
            return;
        }

        CardScript cardToRemove = currentlySelectedCard;
        int oldIndex = cardToRemove.transform.GetSiblingIndex(); 

        DeselectAll();

        cardToRemove.transform.SetParent(null);
        Destroy(cardToRemove.gameObject);
        ReorganizeHand();
        if (DebugMode)AddCard(cardHolder.transform.childCount);
    }

    [Button]
    public void GenerateLoot(int AmountOfCards = 3)
    {
        foreach(Transform gm in LootCardHolder) Destroy(gm.gameObject);
        while (AmountOfCards-- >= 1)
        {
            GameObject newObj = GenerateCanPopulateRandomCard();
            newObj.transform.SetParent(LootCardHolder);
            newObj.GetComponent<CardScript>().IsLootSelectionCard = true;
        }

    }

    [Button]
    public void GenerateShop(int AmountOfCards = 6)
    {
        foreach (Transform gm in ShopCardHolder) Destroy(gm.gameObject);
        while (AmountOfCards-- >= 1)
        {
            GameObject newObj = GenerateCanPopulateRandomCard();
            newObj.transform.SetParent(ShopCardHolder);
            newObj.GetComponent<CardScript>().IsLootSelectionCard = true;
            TextMeshProUGUI temp = Instantiate(CostPrefab, newObj.GetComponent<RectTransform>().position + new Vector3(0, 200f, 0), Quaternion.identity);
            temp.SetText(newObj.GetComponent<CardScript>().PlaceableObjectData.Cost.ToString());
            temp.transform.SetParent(newObj.transform);
        }

    }

    [Button]
    public void closeShop()
    {
        ShopCardHolder.transform.parent.gameObject.SetActive(false);
    }

    private bool isDeckUp = true;
    public bool IsDeckUp=> isDeckUp;
    private float fixedOffset = 200;


    public void ShowDeck()
    {
        if (isDeckUp) return;
        StopAllCoroutines();
        StartCoroutine(MoveDeck(true, 0.35f));
    }

    public void HideDeck()
    {
        if (!isDeckUp) return;
        StopAllCoroutines();
        StartCoroutine(MoveDeck(false, 0.35f));
    }
    public void ToggleAndMoveDeck()
    {
        if (isDeckUp)
            HideDeck();
        else
            ShowDeck();
    }
    IEnumerator MoveDeck(bool show, float duration)
    {
        RectTransform rect = cardHolder.GetComponent<RectTransform>();

        Vector3 startPos = rect.position;
        Vector3 targetPos = show
            ? startPos + new Vector3(0, fixedOffset, 0)
            : startPos - new Vector3(0, fixedOffset, 0);

        if (show)
            cardHolder.gameObject.SetActive(true);

        float elapsed = 0f;
        while (elapsed < duration)
        {
            rect.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        rect.position = targetPos;
        isDeckUp = show;

        if (!show)
            cardHolder.gameObject.SetActive(false);
    }

    //public void ToggleAndMoveDeck()
    //{
    //    StopAllCoroutines();
    //    StartCoroutine(ToggleHideUnhideAndMove(0.35f));
    //}

    //IEnumerator ToggleHideUnhideAndMove(float moveDuration)
    //{
    //    RectTransform rect = cardHolder.GetComponent<RectTransform>();
    //    Vector3 startPos = rect.position;
    //    Vector3 targetPos;

    //    if (!isDeckUp)
    //    {
    //        // Move Up: Current position + offset
    //        targetPos = startPos + new Vector3(0, fixedOffset, 0);
    //        cardHolder.gameObject.SetActive(true);
    //    }
    //    else
    //    {
    //        // Move Down: Current position - offset
    //        targetPos = startPos - new Vector3(0, fixedOffset, 0);
    //    }

    //    float elapsedTime = 0f;
    //    while (elapsedTime < moveDuration)
    //    {
    //        rect.position = Vector3.Lerp(startPos, targetPos, elapsedTime / moveDuration);
    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }

    //    rect.position = targetPos;
    //    isDeckUp = !isDeckUp; 

    //    if (!isDeckUp)
    //    {
    //        cardHolder.gameObject.SetActive(false);
    //    }
    //}



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