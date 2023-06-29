using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject piecePrefab;
    private List<FloatingPiece> floatingPieces;
    private List<string> pieceNames = new List<string>() {"king", "queen", "knight", "bishop", "rook", "pawn"};
    private float movingSpeed = 0.1f;
    private class FloatingPiece
    {
        public GameObject pieceObj;
        public float radius, theta, phi;
        public float direction;
        public FloatingPiece(GameObject _pieceObj, float _radius, float _theta, float _phi, float _direction)
        {
            pieceObj =_pieceObj;
            radius = _radius;
            theta = _theta;
            phi = _phi;
            direction = _direction;
        }

        public void SetTheta(float _theta) { theta = _theta; }
        public void SetPhi(float _phi) { phi = _phi; }        
    }

    private void Awake() {
        floatingPieces = new List<FloatingPiece>();
        AddPiecesToBackground(20);
    }

    private void Update() {
        for (int i=0; i<floatingPieces.Count; i++)
        {
            floatingPieces[i].SetTheta(floatingPieces[i].theta + floatingPieces[i].direction * Time.deltaTime * movingSpeed);
            floatingPieces[i].SetPhi(floatingPieces[i].phi + floatingPieces[i].direction * Time.deltaTime * movingSpeed);
            floatingPieces[i].pieceObj.transform.position = GetPolarCoords(floatingPieces[i].radius, floatingPieces[i].theta, floatingPieces[i].phi);
        }
    }

    private void AddPiecesToBackground(int count)
    {
        for (int i = 0; i < count; i++)
        {
            float r = (i + 3);
            float theta = Random.Range(0, 360), phi = Random.Range(0, 360);
            float direction = Random.Range(-1, 2);

            GameObject newPiece = Instantiate(piecePrefab, GetPolarCoords(r, theta, phi), Quaternion.identity);
            newPiece.GetComponent<PieceController>().AssignPiece(pieceNames[Random.Range(0, pieceNames.Count - 1)]);
            newPiece.AddComponent<PieceIdleRotation>();
            floatingPieces.Add(new FloatingPiece(newPiece, r, theta, phi, direction));
        }
    }

    private Vector3 GetPolarCoords(float radius, float theta, float phi)
    {
            float x, y, z;
            x = radius * Mathf.Sin(theta) * Mathf.Cos(phi);
            y = radius * Mathf.Sin(theta) * Mathf.Sin(phi);
            z = radius * Mathf.Cos(theta);
            return new Vector3(x, y, z);
    }
}
