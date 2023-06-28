using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEventManager : MonoBehaviour
{
    public static System.Action<GameObject> OnPieceSelected, OnTileSelected, OnMoveableTileSelected, OnPieceEaten;
    public static System.Action<string> OnPlayerChecked;
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
    
    public static void SendMoveableTileSelected(GameObject tile)
    {
        if (OnMoveableTileSelected != null)
            OnMoveableTileSelected.Invoke(tile);
    }

    public static void SendSelectionCancel()
    {
        if (OnSelectionCancel != null)
            OnSelectionCancel.Invoke();
    }

    public static void SendPieceEaten(GameObject piece)
    {
        if (OnPieceEaten != null)
            OnPieceEaten.Invoke(piece);
    }

    public static void SendPlayerChecked(string dangerPiece)
    {
        if (OnPlayerChecked != null)
            OnPlayerChecked.Invoke(dangerPiece);
    }
}
