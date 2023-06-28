using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private string[,] board;
    public GameObject pieces, piecePrefab, tiles;
    private List<PieceController> pieceControllers = new List<PieceController>();
    private GameObject currentlyEnabledTile, currentlyEnabledPiece;
    private List<Runner.TileAccess> possibleMoves = new List<Runner.TileAccess>();
    private List<GameObject> activeTiles = new List<GameObject>();
    public Material whiteMaterial, blackMaterial;
    private class pieceStruct 
    {
        string name;
        int x, y;
        bool isWhite;

        public pieceStruct(string name, int x, int y, bool isWhite)
        {
            this.name = name;
            this.x = x;
            this.y = y;
            this.isWhite = isWhite;
        }
        public string GetName() { return this.name; }
        public int GetX() { return this.x; }
        public int GetY() { return this.y; }
        public bool GetIsWhite() { return this.isWhite; }
    }

    private List<pieceStruct> pieceList = new List<pieceStruct>();

    private void Awake() 
    {
        board = new string[4, 16];
        SetupPieces();
        PlacePieces();
        GlobalEventManager.OnPieceSelected += SelectForMove;
        GlobalEventManager.OnSelectionCancel += CancelSelection;
        GlobalEventManager.OnTileSelected += SelectTile;
        GlobalEventManager.OnMoveableTileSelected += MoveToTile;
        GlobalEventManager.OnPieceEaten += HandleEatenPiece;
        GlobalEventManager.OnPlayerChecked += HandleCheck;

        for (int i = 0; i < pieces.transform.childCount; i++)
        {
            pieceControllers.Add(pieces.transform.GetChild(i).gameObject.GetComponent<PieceController>());
        }
    }

    private void PlacePieces()
    {
        foreach (pieceStruct el in pieceList)
        {
            GameObject newPiece = Instantiate(piecePrefab, Vector3.zero, Quaternion.identity);
            newPiece.GetComponent<PieceController>().AssignPiece(el.GetName());
            newPiece.GetComponent<PieceController>().SetInPlace(el.GetX(), el.GetY());
            newPiece.transform.parent = pieces.transform;
            Transform modelTransformer = newPiece.transform.GetChild(0);
            char pieceSymbol = 'u';
            switch(el.GetName())
            {
                case "king":
                    modelTransformer = newPiece.transform.GetChild(0);
                    pieceSymbol = 'k';
                    break;
                case "bishop":
                    modelTransformer = newPiece.transform.GetChild(1);
                    pieceSymbol = 'b';
                    break;
                case "knight":
                    modelTransformer = newPiece.transform.GetChild(2);
                    pieceSymbol = 'h';
                    break;
                case "pawn":
                    modelTransformer = newPiece.transform.GetChild(3);
                    pieceSymbol = 'p';
                    break;
                case "rook":
                    modelTransformer = newPiece.transform.GetChild(4);
                    pieceSymbol = 'r';
                    break;
                case "queen":
                    modelTransformer = newPiece.transform.GetChild(5);
                    pieceSymbol = 'q';
                    break;    
                default:
                    modelTransformer = newPiece.transform.GetChild(3);
                    pieceSymbol = 'u';
                    break;            
            }
            modelTransformer.GetChild(0).GetComponent<Renderer>().material = el.GetIsWhite() ? whiteMaterial : blackMaterial;
            board[el.GetX(), el.GetY()] = el.GetIsWhite() ? "w" + pieceSymbol.ToString() : "b" + pieceSymbol.ToString();
        }
    }

    private void SetupPieces()
    {
        pieceList.Add(new pieceStruct("king", 1, 0, true));
        pieceList.Add(new pieceStruct("king", 1, 15, false));

        pieceList.Add(new pieceStruct("bishop", 0, 0, true));
        pieceList.Add(new pieceStruct("bishop", 2, 15, false));

        pieceList.Add(new pieceStruct("knight", 2, 0, true));
        pieceList.Add(new pieceStruct("knight", 0, 15, false));

        pieceList.Add(new pieceStruct("rook", 3, 0, true));
        pieceList.Add(new pieceStruct("rook", 3, 15, false));

        pieceList.Add(new pieceStruct("pawn", 0, 1, true));
        pieceList.Add(new pieceStruct("pawn", 1, 1, true));
        pieceList.Add(new pieceStruct("pawn", 2, 1, true));
        pieceList.Add(new pieceStruct("pawn", 3, 1, true));
        pieceList.Add(new pieceStruct("pawn", 0, 14, false));
        pieceList.Add(new pieceStruct("pawn", 1, 14, false));
        pieceList.Add(new pieceStruct("pawn", 2, 14, false));
        pieceList.Add(new pieceStruct("pawn", 3, 14, false));

        // pieceList.Add(new pieceStruct("queen", 1, 3, true));
    }

    private void SelectForMove(GameObject selectedPiece)
    {
        PieceController selectedPieceController = selectedPiece.GetComponent<PieceController>();
        selectedPieceController.EnableOutline();
        currentlyEnabledPiece = selectedPiece;
        currentlyEnabledTile = tiles.transform.GetChild(selectedPieceController.X).GetChild(selectedPieceController.Y).gameObject;
        currentlyEnabledTile.GetComponent<TileController>().EnableCurrent();
        CalculatePossibleMoves(selectedPieceController.X, selectedPieceController.Y);
    }

    private void SelectTile(GameObject selectedTile)
    {
        currentlyEnabledTile = selectedTile;
        currentlyEnabledTile.GetComponent<TileController>().EnableCurrent();
        (int x, int y) coords = (currentlyEnabledTile.GetComponent<TileController>().X, currentlyEnabledTile.GetComponent<TileController>().Y);

        foreach (PieceController possibleRightPieceController in pieceControllers)
        {
            if (possibleRightPieceController.X == coords.x && possibleRightPieceController.Y == coords.y)
            {
                currentlyEnabledPiece = possibleRightPieceController.gameObject;
                possibleRightPieceController.EnableOutline();
                CalculatePossibleMoves(coords.x, coords.y);
            }
        }
    }

    private void CancelSelection()
    {
        if (currentlyEnabledPiece != null)
            currentlyEnabledPiece.GetComponent<PieceController>().DisableOutline();
        if (currentlyEnabledTile != null)
            currentlyEnabledTile.GetComponent<TileController>().DisableCurrent();
        foreach (GameObject activeTile in activeTiles)
        {
            TileController tileController = activeTile.GetComponent<TileController>();
            tileController.DisableAccessible();
            tileController.DisableEnemy();
        }
    }

    private void MoveToTile(GameObject selectedTile)
    {
        int dirX;
        int startX = currentlyEnabledPiece.GetComponent<PieceController>().X, startY = currentlyEnabledPiece.GetComponent<PieceController>().Y;
        int destinationX = selectedTile.GetComponent<TileController>().X, destinationY = selectedTile.GetComponent<TileController>().Y;

        Runner.TileAccess move;
        foreach (Runner.TileAccess possibleMove in possibleMoves)
        {
            if (possibleMove.GetEndX() == destinationX && possibleMove.GetEndY() == destinationY)
            {
                move = possibleMove;
                dirX = System.Convert.ToInt32(Mathf.Sign(move.GetHorizontalDistance()));
                
                string oldContentOfEndTile = board[move.GetEndX(), move.GetEndY()];
                board[move.GetEndX(), move.GetEndY()] = board[startX, startY];
                board[startX, startY] = null;
                currentlyEnabledPiece.GetComponent<PieceController>().MovePiece(startX + move.GetHorizontalDistance(), move.GetEndY(), dirX);
                //////////////////////////  CURRENTLY IN DEBUG
                IsPlayerChecked(false);
                //////////////////////////
                if (oldContentOfEndTile == null) return;
                if ((oldContentOfEndTile[0] == 'w' && board[move.GetEndX(), move.GetEndY()][0] == 'b') || (oldContentOfEndTile[0] == 'b' && board[move.GetEndX(), move.GetEndY()][0] == 'w'))
                {
                    foreach (PieceController eatenPieceController in pieceControllers)
                    {
                        if (eatenPieceController.gameObject == currentlyEnabledPiece) continue;
                        if (eatenPieceController.X == move.GetEndX() && eatenPieceController.Y == move.GetEndY())
                        {
                            GlobalEventManager.SendPieceEaten(eatenPieceController.gameObject);
                            break;
                        }
                    }
                }
                return;
            }
        }
    }

    private void CalculatePossibleMoves(int currentX, int currentY)
    {
        possibleMoves.Clear();

        Runner runner = GetComponent<Runner>();
        bool isCurrentWhite = board[currentX, currentY][0] == 'w' ? true :  false;
        char pieceType = board[currentX, currentY][1];
        switch (pieceType)
        {
            case 'k':
                possibleMoves = GetComponent<PieceMovementPatterns>().GetAllPossibleMovesKing(currentX, currentY, isCurrentWhite, board);
                break;
            case 'q':
                possibleMoves = GetComponent<PieceMovementPatterns>().GetAllPossibleMovesQueen(currentX, currentY, isCurrentWhite, board);
                break;
            case 'b':
                possibleMoves = GetComponent<PieceMovementPatterns>().GetAllPossibleMovesBishop(currentX, currentY, isCurrentWhite, board);
                break;
            case 'r':
                possibleMoves = GetComponent<PieceMovementPatterns>().GetAllPossibleMovesRook(currentX, currentY, isCurrentWhite, board);
                break;
            case 'h':
                possibleMoves = GetComponent<PieceMovementPatterns>().GetAllPossibleMovesKnight(currentX, currentY, isCurrentWhite, board);
                break;
            case 'p':
                int lengthOfMove = 0;
                int stepDirection = 0;
                if (isCurrentWhite)
                {
                    if (currentY == 1)
                        lengthOfMove = 2;
                    else 
                        lengthOfMove = 1;
                    stepDirection = 1;
                }
                else
                {
                    if (currentY == 14)
                        lengthOfMove = 2;
                    else
                        lengthOfMove = 1;
                    stepDirection = -1;
                }
                    
                runner.Run(currentX, currentY, 0, stepDirection, lengthOfMove, isCurrentWhite, board).ForEach(move => possibleMoves.Add(move));
                for (int direction = -1; direction < 2; direction +=2)
                {
                    List<Runner.TileAccess> pawnDiagonalMoves = runner.Run(currentX, currentY, direction, stepDirection, 1, isCurrentWhite, board);
                    Runner.TileAccess pawnDiagonalMove;
                    if (pawnDiagonalMoves.Count > 0)
                        pawnDiagonalMove = pawnDiagonalMoves[0];
                    else continue;
                    if (pawnDiagonalMove.GetHitsEnemy()) //if there is an enemy to the side
                    {
                        possibleMoves.Add(pawnDiagonalMove);
                        GameObject possibleTile = tiles.transform.GetChild(pawnDiagonalMove.GetEndX()).GetChild(pawnDiagonalMove.GetEndY()).gameObject;
                        activeTiles.Add(possibleTile);
                        possibleTile.GetComponent<TileController>().EnableEnemy();
                    }
                }
                break;
            default:
                break;
        }

        if (possibleMoves.Count != 0)
        {
            foreach (Runner.TileAccess el in possibleMoves)
            {
                if (!el.GetHitsEnemy())
                {
                    GameObject possibleTile = tiles.transform.GetChild(el.GetEndX()).GetChild(el.GetEndY()).gameObject;
                    activeTiles.Add(possibleTile);
                    possibleTile.GetComponent<TileController>().EnableAccessible();
                }
                else
                {              
                    if (pieceType == 'p') continue;      
                    GameObject possibleTile = tiles.transform.GetChild(el.GetEndX()).GetChild(el.GetEndY()).gameObject;
                    activeTiles.Add(possibleTile);
                    possibleTile.GetComponent<TileController>().EnableEnemy();
                }
            }
        }
    }

    private void HandleEatenPiece(GameObject eatenPiece)
    {
        pieceControllers.Remove(eatenPiece.GetComponent<PieceController>());
        eatenPiece.transform.parent = null;
        Destroy(eatenPiece);
    }

    private bool IsPlayerChecked(bool isWhite)
    {
        bool isChecked = false;
        char kingColor = isWhite ? 'w' : 'b';
        char enemyColor = !isWhite ? 'w' : 'b';
        int kingX = -1, kingY = -1;
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (board[i, j] != null)
                {
                    if (board[i, j] == kingColor.ToString() + "k")
                    {
                        kingX = i;
                        kingY = j;
                        break;
                    } 
                }
            }
        }

        Runner runner = GetComponent<Runner>();
        List<Runner.TileAccess> checkPositionsKnight = new List<Runner.TileAccess>();
        List<Runner.TileAccess> checkPositionsBishop = new List<Runner.TileAccess>();
        List<Runner.TileAccess> checkPositionsRook = new List<Runner.TileAccess>();
        List<Runner.TileAccess> checkPositionsPawn = new List<Runner.TileAccess>();
        List<Runner.TileAccess> checkPositionsQueen = new List<Runner.TileAccess>();
        List<Runner.TileAccess> checkPositionsKing = new List<Runner.TileAccess>();

        checkPositionsKing = GetComponent<PieceMovementPatterns>().GetAllPossibleMovesKing(kingX, kingY, isWhite, board);
        foreach (Runner.TileAccess possibleDangerTile in checkPositionsKing)
        {
            if (board[possibleDangerTile.GetEndX(), possibleDangerTile.GetEndY()] == enemyColor + "k")
            {
                isChecked = true;
                GlobalEventManager.SendPlayerChecked(enemyColor + "k;" + possibleDangerTile.GetEndX().ToString() + ";" + 
                                                                                possibleDangerTile.GetEndY().ToString());
            }
        }

        checkPositionsQueen = GetComponent<PieceMovementPatterns>().GetAllPossibleMovesQueen(kingX, kingY, isWhite, board);
        foreach (Runner.TileAccess possibleDangerTile in checkPositionsQueen)
        {
            if (board[possibleDangerTile.GetEndX(), possibleDangerTile.GetEndY()] == enemyColor + "q")
            {
                isChecked = true;
                GlobalEventManager.SendPlayerChecked(enemyColor + "q;" + possibleDangerTile.GetEndX().ToString() + ";" + 
                                                                                possibleDangerTile.GetEndY().ToString());
            }
        }

        checkPositionsBishop = GetComponent<PieceMovementPatterns>().GetAllPossibleMovesBishop(kingX, kingY, isWhite, board);
        foreach (Runner.TileAccess possibleDangerTile in checkPositionsBishop)
        {
            if (board[possibleDangerTile.GetEndX(), possibleDangerTile.GetEndY()] == enemyColor + "b")
            {
                isChecked = true;
                GlobalEventManager.SendPlayerChecked(enemyColor + "b;" + possibleDangerTile.GetEndX().ToString() + ";" + 
                                                                                possibleDangerTile.GetEndY().ToString());
            }
        }

        checkPositionsRook = GetComponent<PieceMovementPatterns>().GetAllPossibleMovesRook(kingX, kingY, isWhite, board);
        foreach (Runner.TileAccess possibleDangerTile in checkPositionsRook)
        {
            if (board[possibleDangerTile.GetEndX(), possibleDangerTile.GetEndY()] == enemyColor + "r")
            {
                isChecked = true;
                GlobalEventManager.SendPlayerChecked(enemyColor + "r;" + possibleDangerTile.GetEndX().ToString() + ";" + 
                                                                                possibleDangerTile.GetEndY().ToString());
            }
        }

        checkPositionsKnight = GetComponent<PieceMovementPatterns>().GetAllPossibleMovesKnight(kingX, kingY, isWhite, board);
        foreach (Runner.TileAccess possibleDangerTile in checkPositionsKnight)
        {
            if (board[possibleDangerTile.GetEndX(), possibleDangerTile.GetEndY()] == enemyColor + "h")
            {
                isChecked = true;
                GlobalEventManager.SendPlayerChecked(enemyColor + "h;" + possibleDangerTile.GetEndX().ToString() + ";" + 
                                                                                possibleDangerTile.GetEndY().ToString());
            }
        }

        int direction = isWhite ? 1 : -1;
        runner.Run(kingX, kingY, -1, direction, 1, isWhite, board).ForEach(move => checkPositionsPawn.Add(move));
        runner.Run(kingX, kingY, 1,  direction, 1, isWhite, board).ForEach(move => checkPositionsPawn.Add(move));
        foreach (Runner.TileAccess possibleDangerTile in checkPositionsPawn)
        {
            if (board[possibleDangerTile.GetEndX(), possibleDangerTile.GetEndY()] == enemyColor + "p")
            {
                isChecked = true;
                GlobalEventManager.SendPlayerChecked(enemyColor + "p;" + possibleDangerTile.GetEndX().ToString() + ";" + 
                                                                                possibleDangerTile.GetEndY().ToString());
            }
        }

        return isChecked;
    }

    private void HandleCheck(string message)
    {
        Debug.Log(message);
    }
}
