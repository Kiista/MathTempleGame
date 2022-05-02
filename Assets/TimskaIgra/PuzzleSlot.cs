using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzleSlot : MonoBehaviour, IDropHandler
{
    private PuzzlePiece heldPiece;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        var dragObject = eventData.pointerDrag;
        if (dragObject != null)
        {
            dragObject.GetComponent<RectTransform>().anchoredPosition = rectTransform.anchoredPosition;
            heldPiece = dragObject.GetComponent<PuzzlePiece>();
            heldPiece.SetSlot(this);
        }

        Debug.Log("Oasdasdas");
    }

    public PuzzlePiece GetPiece()
    {
        return heldPiece;
    }

    public void Clear()
    {
        heldPiece = null;
    }
}