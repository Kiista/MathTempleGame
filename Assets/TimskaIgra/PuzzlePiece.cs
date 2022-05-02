using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PuzzlePiece : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private Image image;
    private CanvasGroup canvasGroup;
    private int myNumber;
    private PuzzleSlot mySlot;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        SetNumber(Random.Range(1, 7));
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;

        if (mySlot != null)
        {
            mySlot.Clear();
            mySlot = null;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
    }

    public void SetSlot(PuzzleSlot slot)
    {
        mySlot = slot;
    }

    public void SetNumber(int number)
    {
        myNumber = number;

        var index = number - 1;
        if (number >= 5) index--;

        image.sprite = sprites[index];
    }

    public int GetNumber()
    {
        return myNumber;
    }
}