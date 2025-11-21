using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardScript : MonoBehaviour,
    IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public bool isSelected = false;

    private Image img;
    private int originalSiblingIndex;

    void Start()
    {
        img = GetComponent<Image>();
        originalSiblingIndex = transform.GetSiblingIndex();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isSelected = !isSelected;
        img.color = isSelected ? Color.black : Color.yellow;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.SetSiblingIndex(originalSiblingIndex);
    }
}
