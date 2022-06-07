using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MarketSlot : MonoBehaviour, IDropHandler
{
    private MarketPiece heldPiece;

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
            dragObject.GetComponent<RectTransform>().position = rectTransform.position;
            heldPiece = dragObject.GetComponent<MarketPiece>();
            heldPiece.SetSlot(this);
        }

        Debug.Log("Oasdasdas");
    }

    public MarketPiece GetPiece()
    {
        return heldPiece;
    }

    public void Clear()
    {
        heldPiece = null;
    }
}
