using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMovementPatterns : MonoBehaviour
{
    public List<Runner.TileAccess> GetAllPossibleMovesKing(int pieceX, int pieceY, bool isWhite, string[,] board)
    {
        List<Runner.TileAccess> possibleMoves = new List<Runner.TileAccess>();

        Runner runner = GetComponent<Runner>();
        runner.Run(pieceX, pieceY, -1,  1, 1, isWhite, board).ForEach(move => possibleMoves.Add(move));
        runner.Run(pieceX, pieceY,  0,  1, 1, isWhite, board).ForEach(move => possibleMoves.Add(move));
        runner.Run(pieceX, pieceY,  1,  1, 1, isWhite, board).ForEach(move => possibleMoves.Add(move));
        runner.Run(pieceX, pieceY, -1, -1, 1, isWhite, board).ForEach(move => possibleMoves.Add(move));
        runner.Run(pieceX, pieceY,  0, -1, 1, isWhite, board).ForEach(move => possibleMoves.Add(move));
        runner.Run(pieceX, pieceY,  1, -1, 1, isWhite, board).ForEach(move => possibleMoves.Add(move));
        runner.Run(pieceX, pieceY,  1,  0, 1, isWhite, board).ForEach(move => possibleMoves.Add(move));
        runner.Run(pieceX, pieceY, -1,  0, 1, isWhite, board).ForEach(move => possibleMoves.Add(move));

        return possibleMoves;
    }

    public List<Runner.TileAccess> GetAllPossibleMovesQueen(int pieceX, int pieceY, bool isWhite, string[,] board)
    {
        List<Runner.TileAccess> possibleMoves = new List<Runner.TileAccess>();

        Runner runner = GetComponent<Runner>();
        runner.Run(pieceX, pieceY, -1,  1, 15, isWhite, board).ForEach(move => possibleMoves.Add(move));
        runner.Run(pieceX, pieceY,  0,  1, 15, isWhite, board).ForEach(move => possibleMoves.Add(move));
        runner.Run(pieceX, pieceY,  1,  1, 15, isWhite, board).ForEach(move => possibleMoves.Add(move));
        runner.Run(pieceX, pieceY, -1, -1, 15, isWhite, board).ForEach(move => possibleMoves.Add(move));
        runner.Run(pieceX, pieceY,  0, -1, 15, isWhite, board).ForEach(move => possibleMoves.Add(move));
        runner.Run(pieceX, pieceY,  1, -1, 15, isWhite, board).ForEach(move => possibleMoves.Add(move));
        runner.Run(pieceX, pieceY,  1,  0, 15, isWhite, board).ForEach(move => possibleMoves.Add(move));
        runner.Run(pieceX, pieceY, -1,  0, 15, isWhite, board).ForEach(move => possibleMoves.Add(move));

        return possibleMoves;
    }

    public List<Runner.TileAccess> GetAllPossibleMovesBishop(int pieceX, int pieceY, bool isWhite, string[,] board)
    {
        List<Runner.TileAccess> possibleMoves = new List<Runner.TileAccess>();

        Runner runner = GetComponent<Runner>();
        runner.Run(pieceX, pieceY, -1,  1, 15, isWhite, board).ForEach(move => possibleMoves.Add(move));
        runner.Run(pieceX, pieceY,  1,  1, 15, isWhite, board).ForEach(move => possibleMoves.Add(move));
        runner.Run(pieceX, pieceY, -1, -1, 15, isWhite, board).ForEach(move => possibleMoves.Add(move));
        runner.Run(pieceX, pieceY,  1, -1, 15, isWhite, board).ForEach(move => possibleMoves.Add(move));

        return possibleMoves;
    }

    public List<Runner.TileAccess> GetAllPossibleMovesRook(int pieceX, int pieceY, bool isWhite, string[,] board)
    {
        List<Runner.TileAccess> possibleMoves = new List<Runner.TileAccess>();

        Runner runner = GetComponent<Runner>();
        runner.Run(pieceX, pieceY, -1,  0,  3, isWhite, board).ForEach(move => possibleMoves.Add(move));
        runner.Run(pieceX, pieceY,  1,  0,  3, isWhite, board).ForEach(move => possibleMoves.Add(move));
        runner.Run(pieceX, pieceY,  0, -1, 15, isWhite, board).ForEach(move => possibleMoves.Add(move));
        runner.Run(pieceX, pieceY,  0,  1, 15, isWhite, board).ForEach(move => possibleMoves.Add(move));

        return possibleMoves;
    }

    public List<Runner.TileAccess> GetAllPossibleMovesKnight(int pieceX, int pieceY, bool isWhite, string[,] board)
    {
        List<Runner.TileAccess> possibleMoves = new List<Runner.TileAccess>();

        Runner runner = GetComponent<Runner>();
        runner.Run(pieceX, pieceY, -2,  1, 1, isWhite, board).ForEach(move => possibleMoves.Add(move));
        runner.Run(pieceX, pieceY, -2, -1, 1, isWhite, board).ForEach(move => possibleMoves.Add(move));
        runner.Run(pieceX, pieceY,  2,  1, 1, isWhite, board).ForEach(move => possibleMoves.Add(move));
        runner.Run(pieceX, pieceY,  2, -1, 1, isWhite, board).ForEach(move => possibleMoves.Add(move));
        runner.Run(pieceX, pieceY, -1,  2, 1, isWhite, board).ForEach(move => possibleMoves.Add(move));
        runner.Run(pieceX, pieceY, -1, -2, 1, isWhite, board).ForEach(move => possibleMoves.Add(move));
        runner.Run(pieceX, pieceY,  1,  2, 1, isWhite, board).ForEach(move => possibleMoves.Add(move));
        runner.Run(pieceX, pieceY,  1, -2, 1, isWhite, board).ForEach(move => possibleMoves.Add(move));

        return possibleMoves;
    }
}
