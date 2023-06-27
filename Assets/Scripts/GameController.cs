using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private string[,] board;
    public GameObject pieces, piecePrefab, tiles;
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

    private void Awake() {
        board = new string[4, 16];
        SetupPieces();
        PlacePieces();
        GlobalEventManager.OnPieceSelected += SelectForMove;
        GlobalEventManager.OnSelectionCancel += CancelSelection;
        GlobalEventManager.OnTileSelected += SelectTile;
        GlobalEventManager.OnMoveableTileSelected += MoveToTile;
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

        for (int i = 0; i < pieces.transform.childCount; i++)
        {
            PieceController possibleRightPieceController = pieces.transform.GetChild(i).gameObject.GetComponent<PieceController>();
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
                
                if (oldContentOfEndTile == null) return;
                if ((oldContentOfEndTile[0] == 'w' && board[move.GetEndX(), move.GetEndY()][0] == 'b') || (oldContentOfEndTile[0] == 'b' && board[move.GetEndX(), move.GetEndY()][0] == 'w'))
                {
                    for (int i = 0; i < pieces.transform.childCount; i++)
                    {
                        if (pieces.transform.GetChild(i).gameObject == currentlyEnabledPiece) continue;
                        PieceController eatenPieceController = pieces.transform.GetChild(i).gameObject.GetComponent<PieceController>();
                        if (eatenPieceController.X == move.GetEndX() && eatenPieceController.Y == move.GetEndY())
                        {
                            eatenPieceController.gameObject.SetActive(false);
                            GlobalEventManager.SendPieceEaten(eatenPieceController.gameObject);
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
                runner.Run(currentX, currentY, -1,  1, 1, isCurrentWhite, board).ForEach(move => possibleMoves.Add(move));
                runner.Run(currentX, currentY,  0,  1, 1, isCurrentWhite, board).ForEach(move => possibleMoves.Add(move));
                runner.Run(currentX, currentY,  1,  1, 1, isCurrentWhite, board).ForEach(move => possibleMoves.Add(move));
                runner.Run(currentX, currentY, -1, -1, 1, isCurrentWhite, board).ForEach(move => possibleMoves.Add(move));
                runner.Run(currentX, currentY,  0, -1, 1, isCurrentWhite, board).ForEach(move => possibleMoves.Add(move));
                runner.Run(currentX, currentY,  1, -1, 1, isCurrentWhite, board).ForEach(move => possibleMoves.Add(move));
                runner.Run(currentX, currentY,  1,  0, 1, isCurrentWhite, board).ForEach(move => possibleMoves.Add(move));
                runner.Run(currentX, currentY, -1,  0, 1, isCurrentWhite, board).ForEach(move => possibleMoves.Add(move));
                break;
            case 'q':
                runner.Run(currentX, currentY, -1,  1, 15, isCurrentWhite, board).ForEach(move => possibleMoves.Add(move));
                runner.Run(currentX, currentY,  0,  1, 15, isCurrentWhite, board).ForEach(move => possibleMoves.Add(move));
                runner.Run(currentX, currentY,  1,  1, 15, isCurrentWhite, board).ForEach(move => possibleMoves.Add(move));
                runner.Run(currentX, currentY, -1, -1, 15, isCurrentWhite, board).ForEach(move => possibleMoves.Add(move));
                runner.Run(currentX, currentY,  0, -1, 15, isCurrentWhite, board).ForEach(move => possibleMoves.Add(move));
                runner.Run(currentX, currentY,  1, -1, 15, isCurrentWhite, board).ForEach(move => possibleMoves.Add(move));
                runner.Run(currentX, currentY,  1,  0, 15, isCurrentWhite, board).ForEach(move => possibleMoves.Add(move));
                runner.Run(currentX, currentY, -1,  0, 15, isCurrentWhite, board).ForEach(move => possibleMoves.Add(move));
                break;
            case 'b':
                runner.Run(currentX, currentY, -1,  1, 15, isCurrentWhite, board).ForEach(move => possibleMoves.Add(move));
                runner.Run(currentX, currentY,  1,  1, 15, isCurrentWhite, board).ForEach(move => possibleMoves.Add(move));
                runner.Run(currentX, currentY, -1, -1, 15, isCurrentWhite, board).ForEach(move => possibleMoves.Add(move));
                runner.Run(currentX, currentY,  1, -1, 15, isCurrentWhite, board).ForEach(move => possibleMoves.Add(move));
                break;
            case 'r':
                runner.Run(currentX, currentY, -1,  0,  3, isCurrentWhite, board).ForEach(move => possibleMoves.Add(move));
                runner.Run(currentX, currentY,  1,  0,  3, isCurrentWhite, board).ForEach(move => possibleMoves.Add(move));
                runner.Run(currentX, currentY,  0, -1, 15, isCurrentWhite, board).ForEach(move => possibleMoves.Add(move));
                runner.Run(currentX, currentY,  0,  1, 15, isCurrentWhite, board).ForEach(move => possibleMoves.Add(move));
                break;
            case 'h':
                runner.Run(currentX, currentY, -2,  1, 1, isCurrentWhite, board).ForEach(move => possibleMoves.Add(move));
                runner.Run(currentX, currentY, -2, -1, 1, isCurrentWhite, board).ForEach(move => possibleMoves.Add(move));
                runner.Run(currentX, currentY,  2,  1, 1, isCurrentWhite, board).ForEach(move => possibleMoves.Add(move));
                runner.Run(currentX, currentY,  2, -1, 1, isCurrentWhite, board).ForEach(move => possibleMoves.Add(move));
                runner.Run(currentX, currentY, -1,  2, 1, isCurrentWhite, board).ForEach(move => possibleMoves.Add(move));
                runner.Run(currentX, currentY, -1, -2, 1, isCurrentWhite, board).ForEach(move => possibleMoves.Add(move));
                runner.Run(currentX, currentY,  1,  2, 1, isCurrentWhite, board).ForEach(move => possibleMoves.Add(move));
                runner.Run(currentX, currentY,  1, -2, 1, isCurrentWhite, board).ForEach(move => possibleMoves.Add(move));
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
                    GameObject possibleTile = tiles.transform.GetChild(el.GetEndX()).GetChild(el.GetEndY()).gameObject;
                    activeTiles.Add(possibleTile);
                    possibleTile.GetComponent<TileController>().EnableEnemy();
                }
            }
        }
    }
}
