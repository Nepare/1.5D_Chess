using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastController : MonoBehaviour
{
    private RaycastHit _hit;
    private Camera _cam;

    private void Awake() {
        _cam = GetComponent<Camera>();
    }

    private void Update() {
        if (Input.GetKeyDown(Keybinds.keybinds["select"]))
        {
            Ray _ray = _cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_ray, out RaycastHit _Target))
            {
                if (_Target.transform.gameObject.CompareTag("Piece"))
                {
                    GlobalEventManager.SendSelectionCancel(false);
                    GameObject hitPiece = _Target.transform.parent.parent.gameObject;
                    GlobalEventManager.SendPieceSelected(hitPiece);
                }
                else if (_Target.transform.gameObject.CompareTag("MoveTile"))
                {
                    GameObject hitTile = _Target.transform.parent.gameObject;
                    GlobalEventManager.SendMoveableTileSelected(hitTile);
                    GlobalEventManager.SendSelectionCancel(false);
                }              
                else if (_Target.transform.gameObject.CompareTag("Tile"))
                {
                    GlobalEventManager.SendSelectionCancel(true);
                    GameObject hitTile = _Target.transform.parent.gameObject;
                    GlobalEventManager.SendTileSelected(hitTile);
                }               
            }
            else
            {
                GlobalEventManager.SendSelectionCancel(true);
            }
        }
    }
}
