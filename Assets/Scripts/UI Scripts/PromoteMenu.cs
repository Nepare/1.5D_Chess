using UnityEngine;
using UnityEngine.UIElements;

public class PromoteMenu : MonoBehaviour
{
    private VisualElement root;
    private Button btnQueen, btnBishop, btnKnight, btnRook;
    private GameObject promotedPawn;
    private bool isPromotedPieceWhite;
    private string promotedCell;

    private void OnEnable() {
        root = GetComponent<UIDocument>().rootVisualElement;

        btnQueen = root.Q<Button>("promoteToQueen");
        btnBishop = root.Q<Button>("promoteToBishop");
        btnKnight = root.Q<Button>("promoteToKnight");
        btnRook = root.Q<Button>("promoteToRook");

        root.visible = false;
        btnQueen.clicked += PromoteToQueen;
        btnBishop.clicked += PromoteToBishop;
        btnKnight.clicked += PromoteToKnight;
        btnRook.clicked += PromoteToRook;

        GlobalEventManager.OnPawnPromoted += CallMenu;
    }

    private void CallMenu(GameObject promotedPiece, bool isWhite, string cell)
    {
        ChangeColorOfPromotion(isWhite);
        promotedPawn = promotedPiece;
        promotedCell = cell;
        isPromotedPieceWhite = isWhite;

        root.visible = true;
        EscapeMenu.isPaused = true;
        Time.timeScale = 0;
    }

    private void Continue()
    {
        root.visible = false;
        EscapeMenu.isPaused = false;
        Time.timeScale = 1;
    }

    private void ChangeColorOfPromotion(bool isWhite)
    {
        btnQueen.style.backgroundImage = UIBehaviour.GetTextureFromName((isWhite ? "w" : "b") + "q");
        btnBishop.style.backgroundImage = UIBehaviour.GetTextureFromName((isWhite ? "w" : "b") + "b");
        btnKnight.style.backgroundImage = UIBehaviour.GetTextureFromName((isWhite ? "w" : "b") + "h");
        btnRook.style.backgroundImage = UIBehaviour.GetTextureFromName((isWhite ? "w" : "b") + "r");
    }

    private void PromoteToQueen()
    {
        PromoteGeneral("queen", "q");
    }

    private void PromoteToBishop()
    {
        PromoteGeneral("bishop", "b");
    }

    private void PromoteToKnight()
    {
        PromoteGeneral("knight", "h");
    }

    private void PromoteToRook()
    {
        PromoteGeneral("rook", "r");
    }

    private void NullifyAllTemporaryPromotionData()
    {
        promotedPawn = null;
        promotedCell = null;
    }
    
    private void PromoteGeneral(string fullName, string shortName)
    {
        Continue();

        promotedPawn.GetComponent<PieceController>().AssignPiece(fullName);
        GlobalEventManager.SendPromotion(promotedCell, (isPromotedPieceWhite ? "w" : "b") + shortName);
        NullifyAllTemporaryPromotionData();
    }
}
