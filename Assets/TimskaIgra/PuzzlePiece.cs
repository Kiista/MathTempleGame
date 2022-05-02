using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    private bool dragging;

    private void Update()
    {
        if (!dragging)
            return;

        var mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

        transform.position = mousePosition;
    }

    private void OnMouseDown()
    {
        dragging = true;
        Debug.Log("Pressing");
    }
}
