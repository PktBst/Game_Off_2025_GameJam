using System;
using UnityEngine;

public class CardSystem : MonoBehaviour
{
    public GameObject cardPrefab;
    public RectTransform cardHolder;
    public int numberOfCards = 5;
    [Range(0, 1)] public float CardVisibility;
    [Range(0,10)] public float MaxCardRotation;

    private Vector2 centrePoint;

    private void Start()
    {
        Init();
    }
    public void Init()
    {
        SpawnCards();
    }

    void SpawnCards()
    {
        centrePoint = cardHolder.rect.center;
        RectTransform CardRect = cardPrefab.GetComponent<RectTransform>();

        float totalXneeded = (CardRect.rect.x * CardVisibility) * numberOfCards;
        float startPosX = centrePoint.x - totalXneeded / 2; 
        Vector2 StartPoint = new Vector2 (startPosX, centrePoint.y);
        float rotationStep = (MaxCardRotation *2)/ numberOfCards;
        for (int i = 0; i < numberOfCards; i++)
        {
            RectTransform card = Instantiate(cardPrefab, cardHolder).GetComponent<RectTransform>();
            card.anchoredPosition = StartPoint;
            card.localRotation = Quaternion.Euler(0f, 0f,  (rotationStep *2 * i) - MaxCardRotation );
            StartPoint += new Vector2(CardRect.rect.x * CardVisibility, centrePoint.y);
            card.SetAsFirstSibling();

        }
    }
}
