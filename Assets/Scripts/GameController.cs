using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private string[,] board;
    private string[] boardEdges;
    public GameObject menu, gameOverMenu;
    public GameObject pieces, piecePrefab, tiles;
    private List<PieceController> pieceControllers = new List<PieceController>();
    private GameObject currentlyEnabledTile, currentlyEnabledPiece;
    private List<Runner.TileAccess> possibleMoves = new List<Runner.TileAccess>();
    private List<GameObject> activeTiles = new List<GameObject>();
    public Material whiteMaterial, blackMaterial;
    private bool WhitesTurnToMove;

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
        boardEdges = new string[2];
        menu.SetActive(true);
        gameOverMenu.SetActive(true);
        SetupPieces();
        PlacePieces();
        GlobalEventManager.OnPieceSelected += SelectForMove;
        GlobalEventManager.OnSelectionCancel += CancelSelection;
        GlobalEventManager.OnTileSelected += SelectTile;
        GlobalEventManager.OnMoveableTileSelected += MoveToTile;
        GlobalEventManager.OnPieceEaten += HandleEatenPiece;

        GlobalEventManager.OnPlayerChecked    += HandleCheck;
        GlobalEventManager.OnPlayerCheckmated += HandleCheckmate;
        GlobalEventManager.OnPlayerStalemated += HandleStalemate;

        for (int i = 0; i < pieces.transform.childCount; i++)
        {
            pieceControllers.Add(pieces.transform.GetChild(i).gameObject.GetComponent<PieceController>());
        }
        WhitesTurnToMove = true;
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
            if (el.GetX() != -1)
                board[el.GetX(), el.GetY()] = el.GetIsWhite() ? "w" + pieceSymbol.ToString() : "b" + pieceSymbol.ToString();
            else 
            {
                boardEdges[el.GetY() == 1 ? 0 : 1] = el.GetIsWhite() ? "w" + pieceSymbol.ToString() : "b" + pieceSymbol.ToString();
                newPiece.AddComponent<EdgePieceIdleAnimation>();
            }
        }
    }

    private void SetupPieces()
    {
        if (SetUpConfigurer.SETUP_CONFIGURATION.Keys.Count == 0) 
            SetUpConfigurer.SETUP_CONFIGURATION = SetUpConfigurer.GetDefaultBoardConfiguration();

        foreach (var tileName in SetUpConfigurer.SETUP_CONFIGURATION.Keys)
        {
            int x_coord = 0, y_coord = 0;
            if (tileName[0] == 'l') x_coord = 0;
            if (tileName[0] == 't') x_coord = 1;
            if (tileName[0] == 'r') x_coord = 2;
            if (tileName[0] == 'b') x_coord = 3;
            if (tileName[0] == 'e') x_coord = -1;

            y_coord = System.Convert.ToInt32(tileName.Substring(1));
            if (x_coord == -1) y_coord = (y_coord == 0) ? 1 : -1;

            bool isWhite = (SetUpConfigurer.SETUP_CONFIGURATION[tileName][0] == 'w') ? true : false;
            string pieceName = "";
            switch (SetUpConfigurer.SETUP_CONFIGURATION[tileName][1])
            {
                case 'k': pieceName = "king"; break;
                case 'q': pieceName = "queen"; break;
                case 'b': pieceName = "bishop"; break;
                case 'h': pieceName = "knight"; break;
                case 'r': pieceName = "rook"; break;
                case 'p': pieceName = "pawn"; break;
                default: pieceName = "pawn"; break;
            }
            pieceList.Add(new pieceStruct(pieceName, x_coord, y_coord, isWhite));
        }

        List<string> pieceNames = new List<string>();
        foreach (var piece in pieceList)
            pieceNames.Add((piece.GetIsWhite() ? "w" : "b") + piece.GetName());
        menu.GetComponent<EscapeMenu>().SetupPieces(pieceNames);
    }

    private void SelectForMove(GameObject selectedPiece)
    {
        if (EscapeMenu.isPaused) return;
        PieceController selectedPieceController = selectedPiece.GetComponent<PieceController>();
        selectedPieceController.EnableOutline();
        currentlyEnabledPiece = selectedPiece;
        if (selectedPieceController.X != -1)
        {
            currentlyEnabledTile = tiles.transform.GetChild(selectedPieceController.X).GetChild(selectedPieceController.Y).gameObject;
            currentlyEnabledTile.GetComponent<TileController>().EnableCurrent();
            HandleMaterialChange(board[selectedPieceController.X, selectedPieceController.Y][0] == 'w' ? true : false);
        }
        else HandleMaterialChange(boardEdges[selectedPieceController.Y == 1 ? 0 : 1][0] == 'w' ? true : false);
        CalculatePossibleMoves(selectedPieceController.X, selectedPieceController.Y);
    }

    private void SelectTile(GameObject selectedTile)
    {
        if (EscapeMenu.isPaused) return;
        currentlyEnabledTile = selectedTile;
        currentlyEnabledTile.GetComponent<TileController>().EnableCurrent();
        (int x, int y) coords = (currentlyEnabledTile.GetComponent<TileController>().X, currentlyEnabledTile.GetComponent<TileController>().Y);

        foreach (PieceController possibleRightPieceController in pieceControllers)
        {
            if (possibleRightPieceController.X == coords.x && possibleRightPieceController.Y == coords.y)
            {
                HandleMaterialChange(board[coords.x, coords.y][0] == 'w' ? true : false);
                currentlyEnabledPiece = possibleRightPieceController.gameObject;
                possibleRightPieceController.EnableOutline();
                CalculatePossibleMoves(coords.x, coords.y);
            }
        }
    }

    private void CancelSelection(bool trueCancel)
    {
        if (EscapeMenu.isPaused) return;
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
        if (EscapeMenu.isPaused) return;
        int dirX;
        int startX = currentlyEnabledPiece.GetComponent<PieceController>().X, startY = currentlyEnabledPiece.GetComponent<PieceController>().Y;
        int destinationX = selectedTile.GetComponent<TileController>().X, destinationY = selectedTile.GetComponent<TileController>().Y;

        Runner.TileAccess move;
        foreach (Runner.TileAccess possibleMove in possibleMoves)
        {
            if (!IsRightTurn(startX, startY)) return;

            if (possibleMove.GetEndX() == destinationX && possibleMove.GetEndY() == destinationY)
            {
                move = possibleMove;
                dirX = System.Convert.ToInt32(Mathf.Sign(move.GetHorizontalDistance()));
                
                string oldContentOfEndTile = board[move.GetEndX(), move.GetEndY()];
                if (startX != -1)
                {
                    board[move.GetEndX(), move.GetEndY()] = board[startX, startY];
                    board[startX, startY] = null;    
                }
                else
                {
                    board[move.GetEndX(), move.GetEndY()] = boardEdges[startY == 1 ? 0 : 1];
                    boardEdges[startY == 1 ? 0 : 1] = null;
                    startX = move.GetEndX();
                }

                GlobalEventManager.SendMoveMade();
                NextTurn();
                if (IsPlayerChecked(WhitesTurnToMove, board))
                {
                    if (IsPlayerDoomed(WhitesTurnToMove))
                    {
                        GlobalEventManager.SendPlayerCheckmated(WhitesTurnToMove ? "w" : "b");
                        return;
                    }
                    else
                    {
                        GlobalEventManager.SendCheckShow();
                    }
                }
                if (IsPlayerDoomed(WhitesTurnToMove))
                {
                    GlobalEventManager.SendPlayerStalemated(WhitesTurnToMove ? "w" : "b");
                    return;
                }

                currentlyEnabledPiece.GetComponent<PieceController>().MovePiece(startX + move.GetHorizontalDistance(), move.GetEndY(), dirX);
                if (oldContentOfEndTile == null) return;
                if ((oldContentOfEndTile[0] == 'w' && board[move.GetEndX(), move.GetEndY()][0] == 'b') || (oldContentOfEndTile[0] == 'b' && board[move.GetEndX(), move.GetEndY()][0] == 'w'))
                {
                    foreach (PieceController eatenPieceController in pieceControllers)
                    {
                        if (eatenPieceController.gameObject == currentlyEnabledPiece) continue;
                        if (eatenPieceController.X == move.GetEndX() && eatenPieceController.Y == move.GetEndY())
                        {
                            menu.GetComponent<EscapeMenu>().DecreasePieceCount(oldContentOfEndTile);
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
        char pieceType = 'u';
        if (currentX == -1)
        {
            int landingY;
            bool isEdgePieceWhite;

            if (boardEdges[currentY == 1 ? 0 : 1][0] == 'w') isEdgePieceWhite = true;
            else isEdgePieceWhite = false;

            pieceType = boardEdges[currentY == 1 ? 0 : 1][1];

            if (currentY == 1) landingY = 0;
            else landingY = 15;

            for (int i = 0; i < 4; i++)
            {
                List<Runner.TileAccess> potentialMove = runner.Run(i, landingY, 0, 0, 1, isEdgePieceWhite, board);
                if (potentialMove.Count > 0) possibleMoves.Add(potentialMove[0]);
            }
        }
        else
        {
            bool isCurrentWhite = board[currentX, currentY][0] == 'w' ? true :  false;
            pieceType = board[currentX, currentY][1];
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
                            int endX = pawnDiagonalMove.GetEndX(), endY = pawnDiagonalMove.GetEndY();
                            string[,] potentialBoard = board.Clone() as string[,];
                            potentialBoard[endX, endY] = potentialBoard[currentX, currentY];
                            potentialBoard[currentX, currentY] = null;
                            if (!IsMoveLegal(currentX, currentY, endX, endY)) continue;

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
        }

        if (possibleMoves.Count != 0)
        {
            foreach (Runner.TileAccess el in possibleMoves)
            {
                int endX = el.GetEndX(), endY = el.GetEndY();

                if (!IsMoveLegal(currentX, currentY, endX, endY)) continue;

                if (!el.GetHitsEnemy())
                {
                    GameObject possibleTile = tiles.transform.GetChild(endX).GetChild(endY).gameObject;
                    activeTiles.Add(possibleTile);
                    possibleTile.GetComponent<TileController>().EnableAccessible();
                }
                else
                {              
                    if (pieceType == 'p' || currentX == -1) continue;      
                    GameObject possibleTile = tiles.transform.GetChild(endX).GetChild(endY).gameObject;
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

    private bool IsPlayerChecked(bool isWhite, string[,] potentialBoard)
    {
        bool isChecked = false;
        char kingColor = isWhite ? 'w' : 'b';
        char enemyColor = !isWhite ? 'w' : 'b';
        int kingX = -1, kingY = -1;
        for (int i = 0; i < potentialBoard.GetLength(0); i++)
        {
            for (int j = 0; j < potentialBoard.GetLength(1); j++)
            {
                if (potentialBoard[i, j] != null)
                {
                    if (potentialBoard[i, j] == kingColor.ToString() + "k")
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

        checkPositionsKing = GetComponent<PieceMovementPatterns>().GetAllPossibleMovesKing(kingX, kingY, isWhite, potentialBoard);
        foreach (Runner.TileAccess possibleDangerTile in checkPositionsKing)
        {
            if (potentialBoard[possibleDangerTile.GetEndX(), possibleDangerTile.GetEndY()] == enemyColor + "k")
            {
                isChecked = true;
                GlobalEventManager.SendPlayerChecked(enemyColor + "k;" + possibleDangerTile.GetEndX().ToString() + ";" + 
                                                                                possibleDangerTile.GetEndY().ToString());
            }
        }

        checkPositionsQueen = GetComponent<PieceMovementPatterns>().GetAllPossibleMovesQueen(kingX, kingY, isWhite, potentialBoard);
        foreach (Runner.TileAccess possibleDangerTile in checkPositionsQueen)
        {
            if (potentialBoard[possibleDangerTile.GetEndX(), possibleDangerTile.GetEndY()] == enemyColor + "q")
            {
                isChecked = true;
                GlobalEventManager.SendPlayerChecked(enemyColor + "q;" + possibleDangerTile.GetEndX().ToString() + ";" + 
                                                                                possibleDangerTile.GetEndY().ToString());
            }
        }

        checkPositionsBishop = GetComponent<PieceMovementPatterns>().GetAllPossibleMovesBishop(kingX, kingY, isWhite, potentialBoard);
        foreach (Runner.TileAccess possibleDangerTile in checkPositionsBishop)
        {
            if (potentialBoard[possibleDangerTile.GetEndX(), possibleDangerTile.GetEndY()] == enemyColor + "b")
            {
                isChecked = true;
                GlobalEventManager.SendPlayerChecked(enemyColor + "b;" + possibleDangerTile.GetEndX().ToString() + ";" + 
                                                                                possibleDangerTile.GetEndY().ToString());
            }
        }

        checkPositionsRook = GetComponent<PieceMovementPatterns>().GetAllPossibleMovesRook(kingX, kingY, isWhite, potentialBoard);
        foreach (Runner.TileAccess possibleDangerTile in checkPositionsRook)
        {
            if (potentialBoard[possibleDangerTile.GetEndX(), possibleDangerTile.GetEndY()] == enemyColor + "r")
            {
                isChecked = true;
                GlobalEventManager.SendPlayerChecked(enemyColor + "r;" + possibleDangerTile.GetEndX().ToString() + ";" + 
                                                                                possibleDangerTile.GetEndY().ToString());
            }
        }

        checkPositionsKnight = GetComponent<PieceMovementPatterns>().GetAllPossibleMovesKnight(kingX, kingY, isWhite, potentialBoard);
        foreach (Runner.TileAccess possibleDangerTile in checkPositionsKnight)
        {
            if (potentialBoard[possibleDangerTile.GetEndX(), possibleDangerTile.GetEndY()] == enemyColor + "h")
            {
                isChecked = true;
                GlobalEventManager.SendPlayerChecked(enemyColor + "h;" + possibleDangerTile.GetEndX().ToString() + ";" + 
                                                                                possibleDangerTile.GetEndY().ToString());
            }
        }

        int direction = isWhite ? 1 : -1;
        runner.Run(kingX, kingY, -1, direction, 1, isWhite, potentialBoard).ForEach(move => checkPositionsPawn.Add(move));
        runner.Run(kingX, kingY, 1,  direction, 1, isWhite, potentialBoard).ForEach(move => checkPositionsPawn.Add(move));
        foreach (Runner.TileAccess possibleDangerTile in checkPositionsPawn)
        {
            if (potentialBoard[possibleDangerTile.GetEndX(), possibleDangerTile.GetEndY()] == enemyColor + "p")
            {
                isChecked = true;
                GlobalEventManager.SendPlayerChecked(enemyColor + "p;" + possibleDangerTile.GetEndX().ToString() + ";" + 
                                                                                possibleDangerTile.GetEndY().ToString());
            }
        }

        return isChecked;
    }
    
    private void NextTurn()
    {
        WhitesTurnToMove = !WhitesTurnToMove;
    }

    private bool IsRightTurn(int pieceX, int pieceY)
    {
        if (pieceX == -1)
        {
            if (boardEdges[pieceY == 1 ? 0 : 1] != null && boardEdges[pieceY == 1 ? 0 : 1][0] == 'w' && !WhitesTurnToMove)
                return false;
            if (boardEdges[pieceY == 1 ? 0 : 1] != null && boardEdges[pieceY == 1 ? 0 : 1][0] == 'b' && WhitesTurnToMove)
                return false;
            return true;
        }
        else
        {
            if (board[pieceX, pieceY] != null && board[pieceX, pieceY][0] == 'w' && !WhitesTurnToMove) 
            {
                return false;
            }    
            if (board[pieceX, pieceY] != null && board[pieceX, pieceY][0] == 'b' && WhitesTurnToMove) 
            {
                return false;
            }
            return true;
        }
    }

    private bool IsMoveLegal(int startX, int startY, int endX, int endY)
    {
        bool isLegal = true;
        bool isWhite;

        string[,] potentialBoard = board.Clone() as string[,];

        if (startX != -1)
        {
            isWhite = potentialBoard[startX, startY][0] == 'w' ? true : false;
            potentialBoard[endX, endY] = potentialBoard[startX, startY];
            potentialBoard[startX, startY] = null;
        }
        else
        {
            isWhite = boardEdges[startY == 1 ? 0 : 1][0] == 'w' ? true : false;
            potentialBoard[endX, endY] = boardEdges[startY == 1 ? 0 : 1];
        }

        if (IsPlayerChecked(isWhite, potentialBoard)) 
            isLegal = false;

        return isLegal;
    }

    private bool IsPlayerDoomed(bool isWhite)
    {
        char playerColor = isWhite ? 'w' : 'b';
        char enemyColor  = isWhite ? 'b' : 'w';
        Runner runner = GetComponent<Runner>();

        for (int pieceX = 0; pieceX < board.GetLength(0); pieceX++)
        {
            for (int pieceY = 0; pieceY < board.GetLength(1); pieceY++)
            {
                if (board[pieceX, pieceY] != null && board[pieceX, pieceY][0] == playerColor)
                {
                    List<Runner.TileAccess> tilesToExitOutOfCheck = new List<Runner.TileAccess>();
                    char pieceType = board[pieceX, pieceY][1];
                    switch (pieceType)
                    {
                        case 'k':
                            tilesToExitOutOfCheck = GetComponent<PieceMovementPatterns>().GetAllPossibleMovesKing(pieceX, pieceY, isWhite, board);
                            break;
                        case 'q':
                            tilesToExitOutOfCheck = GetComponent<PieceMovementPatterns>().GetAllPossibleMovesQueen(pieceX, pieceY, isWhite, board);
                            break;
                        case 'b':
                            tilesToExitOutOfCheck = GetComponent<PieceMovementPatterns>().GetAllPossibleMovesBishop(pieceX, pieceY, isWhite, board);
                            break;
                        case 'r':
                            tilesToExitOutOfCheck = GetComponent<PieceMovementPatterns>().GetAllPossibleMovesRook(pieceX, pieceY, isWhite, board);
                            break;
                        case 'h':
                            tilesToExitOutOfCheck = GetComponent<PieceMovementPatterns>().GetAllPossibleMovesKnight(pieceX, pieceY, isWhite, board);
                            break;
                        case 'p':
                            int lengthOfMove = 0;
                            int stepDirection = 0;
                            if (isWhite)
                            {
                                if (pieceY == 1)
                                    lengthOfMove = 2;
                                else 
                                    lengthOfMove = 1;
                                stepDirection = 1;
                            }
                            else
                            {
                                if (pieceY == 14)
                                    lengthOfMove = 2;
                                else
                                    lengthOfMove = 1;
                                stepDirection = -1;
                            }
                                
                            runner.Run(pieceX, pieceY, 0, stepDirection, lengthOfMove, isWhite, board).ForEach(move => tilesToExitOutOfCheck.Add(move));
                            for (int direction = -1; direction < 2; direction +=2)
                            {
                                List<Runner.TileAccess> pawnDiagonalMoves = runner.Run(pieceX, pieceY, direction, stepDirection, 1, isWhite, board);
                                Runner.TileAccess pawnDiagonalMove;
                                if (pawnDiagonalMoves.Count > 0)
                                    pawnDiagonalMove = pawnDiagonalMoves[0];
                                else continue;

                                if (pawnDiagonalMove.GetHitsEnemy())
                                {
                                    int endX = pawnDiagonalMove.GetEndX(), endY = pawnDiagonalMove.GetEndY();
                                    if (!IsMoveLegal(pieceX, pieceY, endX, endY)) continue;
                                    //if a pawn can attack so the player controlling it exits the check position
                                    return false; 
                                }
                            }
                            break;
                    }

                    if (tilesToExitOutOfCheck.Count != 0)
                    {
                        foreach (Runner.TileAccess potentialTileToEscapeCheck in tilesToExitOutOfCheck)
                        {
                            int endX = potentialTileToEscapeCheck.GetEndX(), endY = potentialTileToEscapeCheck.GetEndY();
                            if (!IsMoveLegal(pieceX, pieceY, endX, endY)) continue;
                            return false;
                        }
                    }
                }
            }
        }
        //if edge piece can interfere
        for (int edgeSide = 0; edgeSide < 2; edgeSide++)
        {
            if (boardEdges[edgeSide] != null && boardEdges[edgeSide][0] == playerColor)
            {
                for (int probableCheckExitPosition = 0; probableCheckExitPosition < 4; probableCheckExitPosition++)
                {
                    if(board[probableCheckExitPosition, edgeSide == 0 ? 0 : 15] == null)
                    {
                        if(IsMoveLegal(-1, edgeSide == 0 ? 1 : -1, probableCheckExitPosition, edgeSide == 0 ? 0 : 15))
                            return false;
                    }
                }
            }
        }

        return true;
    }

    private void HandleMaterialChange(bool WhiteSelected)
    {
        if (WhiteSelected == WhitesTurnToMove)
            GlobalEventManager.SendUseNormalMaterialsForHints();
        else
            GlobalEventManager.SendUseAltMaterialsForHints();
    }

    private void HandleCheck(string message)
    {
    }

    private void HandleCheckmate(string message)
    {
        if (message[0] == 'w')
        Debug.Log("THE BLACK HAVE WON");
        else
        Debug.Log("THE WHITE HAVE WON");
    }

    private void HandleStalemate(string message)
    {
        if (message[0] == 'w')
        Debug.Log("STALEMATE: THE WHITE CANNOT MOVE");
        else
        Debug.Log("STALEMATE: THE BLACK CANNOT MOVE");
    }
}
