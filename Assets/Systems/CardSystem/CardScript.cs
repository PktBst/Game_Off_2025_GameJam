using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardScript : MonoBehaviour,
    IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI References")]
    [SerializeField] public bool IsLootSelectionCard;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI NameText;
    [SerializeField] private Image img;

    [Header("Animation Settings")]
    [SerializeField] private float hoverOffset = 50f;
    [SerializeField] private float scaleOnHover = 1.1f;
    [SerializeField] private float smoothTime = 0.1f;

    [Header("Data")]
    public PlaceableObject_SO PlaceableObjectData;
    public bool isSelected = false;
    private bool isHovering = false;

    // The "Resting" position calculated by the System
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    // The actual target we move towards (affected by hover/select)
    private Vector3 targetPosition;
    private Vector3 currentPosVelocity;

    private Vector3 originalScale;
    private Vector3 targetScale;
    private Vector3 currentScaleVelocity;

    private Canvas cardCanvas;
    private int defaultSortingOrder = 0;
    private bool needsUpdate = false;

    private void Awake()
    {
        if (!TryGetComponent(out cardCanvas))
            cardCanvas = gameObject.AddComponent<Canvas>();

        if (!TryGetComponent(out GraphicRaycaster gr))
            gameObject.AddComponent<GraphicRaycaster>();

        originalScale = transform.localScale;
        targetScale = originalScale;

        originalPosition = transform.localPosition;
        targetPosition = originalPosition;
        originalRotation = transform.localRotation;
    }

    private void Start()
    {
        if (cardCanvas != null)
            defaultSortingOrder = cardCanvas.sortingOrder;
    }

    private void OnEnable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.TickSystem.Subscribe(UpdateCardAnimation);
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.TickSystem.Unsubscribe(UpdateCardAnimation);
    }

    public void Init()
    {
        if (PlaceableObjectData != null)
            NameText.text = PlaceableObjectData.Type.ToString();
    }

    public void UpdateBaseTransform(Vector3 newPos, Quaternion newRot)
    {
        originalPosition = newPos;
        originalRotation = newRot;

        if (!isHovering && !isSelected)
        {
            targetPosition = originalPosition;
            transform.localRotation = originalRotation;
        }

        needsUpdate = true;
    }

    private void UpdateCardAnimation()
    {
        if (!needsUpdate) return;

        // Smooth Move
        transform.localPosition = Vector3.SmoothDamp(
            transform.localPosition,
            targetPosition,
            ref currentPosVelocity,
            smoothTime
        );

        // Smooth Scale
        transform.localScale = Vector3.SmoothDamp(
            transform.localScale,
            targetScale,
            ref currentScaleVelocity,
            smoothTime
        );

        // Rotation smooth
        if (!isHovering && !isSelected)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, originalRotation, Time.deltaTime * 10f);
        }
        else
        {
            // When hovering, straighten the card
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, Time.deltaTime * 15f);
        }

        // Snap check to stop calculation if close enough
        if ((transform.localPosition - targetPosition).sqrMagnitude < 0.001f &&
            (transform.localScale - targetScale).sqrMagnitude < 0.001f)
        {
            transform.localPosition = targetPosition;
            transform.localScale = targetScale;
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        UpdateVisualState();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        UpdateVisualState();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsLootSelectionCard)
        {
            var cs = GameManager.Instance.CardSystem;
            cs.AddCard(cs.cardHolder.transform.childCount,PlaceableObjectData);
            cs.LootCardHolder.parent.gameObject.SetActive(false);
        }
        if (isSelected)
        {
            Deselect();
            GameManager.Instance.CardSystem.DeselectAll();
        }
        else
        {
            isSelected = true;
            UpdateVisualState();

            if (GameManager.Instance.CardSystem != null)
            {
                GameManager.Instance.CardSystem.OnCardClicked(this);
            }
        }
    }

    public void Deselect()
    {
        isSelected = false;
        UpdateVisualState();
    }

    private void UpdateVisualState()
    {
        if (IsLootSelectionCard) return;
        bool isActiveState = isHovering || isSelected;

        if (isActiveState)
        {
            // Active: High Sorting Order, Position Up, Scale Up
            if (cardCanvas != null)
            {
                cardCanvas.overrideSorting = true;
                cardCanvas.sortingOrder = 30000;
            }

            //move relative to the assigned arch position
            targetPosition = originalPosition + (Vector3.up * hoverOffset);
            targetScale = originalScale * scaleOnHover;
        }
        else
        {
            //Inactive: Default Sorting, Return to Arch
            if (cardCanvas != null)
            {
                cardCanvas.overrideSorting = false;
                cardCanvas.sortingOrder = defaultSortingOrder;
            }

            targetPosition = originalPosition;
            targetScale = originalScale;
        }

        needsUpdate = true;
    }
}