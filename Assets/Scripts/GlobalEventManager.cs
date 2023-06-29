using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEventManager : MonoBehaviour
{
    public static System.Action<GameObject> OnPieceSelected, OnTileSelected, OnMoveableTileSelected, OnPieceEaten;
    public static System.Action<string> OnPlayerChecked, OnPlayerCheckmated, OnPlayerStalemated;
    public static System.Action OnSelectionCancel, OnCameraDefault, OnUseAltMaterialsForHints, OnUseNormalMaterialsForHints; 

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

    public static void SendPlayerChecked(string message)
    {
        if (OnPlayerChecked != null)
            OnPlayerChecked.Invoke(message);
    }

    public static void SendPlayerCheckmated(string message)
    {
        if (OnPlayerCheckmated != null)
            OnPlayerCheckmated.Invoke(message);
    }

    public static void SendPlayerStalemated(string message)
    {
        if (OnPlayerStalemated != null)
            OnPlayerStalemated.Invoke(message);
    }

    public static void SendCameraDefault()
    {
        if (OnCameraDefault != null)
            OnCameraDefault.Invoke();
    }

    public static void SendUseAltMaterialsForHints()
    {
        if (OnUseAltMaterialsForHints != null)
            OnUseAltMaterialsForHints.Invoke();
    }

    public static void SendUseNormalMaterialsForHints()
    {
        if (OnUseNormalMaterialsForHints != null)
            OnUseNormalMaterialsForHints.Invoke();
    }
}
