using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEventManager : MonoBehaviour
{
    public static System.Action<GameObject> OnPieceSelected, OnTileSelected;
    public static System.Action OnSelectionCancel; 

    public static void SendPieceSelected(GameObject piece)
    {
        if (OnPieceSelected != null)
            OnPieceSelected.Invoke(piece);
    }

    public static void SendTileSelected(GameObject tile)
    {
        if (OnTileSelected != null)
            OnTileSelected.Invoke(tile);
    }

    public static void SendSelectionCancel()
    {
        if (OnSelectionCancel != null)
            OnSelectionCancel.Invoke();
    }
}
