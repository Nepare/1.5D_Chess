using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] public string[,] board;
    public GameObject pieces, piecePrefab;
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
            switch(el.GetName())
            {
                case "king":
                    modelTransformer = newPiece.transform.GetChild(0);
                    break;
                case "bishop":
                    modelTransformer = newPiece.transform.GetChild(1);
                    break;
                case "knight":
                    modelTransformer = newPiece.transform.GetChild(2);
                    break;
                case "pawn":
                    modelTransformer = newPiece.transform.GetChild(3);
                    break;
                case "rook":
                    modelTransformer = newPiece.transform.GetChild(4);
                    break;
                case "queen":
                    modelTransformer = newPiece.transform.GetChild(5);
                    break;                
            }
            modelTransformer.GetChild(0).GetComponent<Renderer>().material = el.GetIsWhite() ? whiteMaterial : blackMaterial;
            board[el.GetX(), el.GetY()] = el.GetName();
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
    }
}
