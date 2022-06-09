using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TableSlot : MonoBehaviour, IDropHandler
{
    private TablePiece heldPiece;

    private RectTransform slotRectTransform;

    private void Awake()
    {
        slotRectTransform = GetComponent<RectTransform>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        var dragObject = eventData.pointerDrag;
        if (dragObject != null)
        {
            dragObject.GetComponent<RectTransform>().position = slotRectTransform.position;
            heldPiece = dragObject.GetComponent<TablePiece>();
            heldPiece.SetSlot(this);
        }
        Debug.Log(heldPiece);
    }

    public TablePiece GetPiece()
    {
        return heldPiece;
    }

    public void Clear()
    {
        heldPiece = null;
    }
}

