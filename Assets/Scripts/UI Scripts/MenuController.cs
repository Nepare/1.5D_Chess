using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject piecePrefab;
    [SerializeField] GameObject uiDoc;
    private List<FloatingPiece> floatingPieces;
    private List<string> pieceNames = new List<string>() { "king", "queen", "knight", "bishop", "rook", "pawn" };
    private float movingSpeed;
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
        movingSpeed = 0.1f;

        floatingPieces = new List<FloatingPiece>();
        LanguageController.ReadTable();
        AddPiecesToBackground(20);
    }

    private void Start() {
        StartCoroutine(PlayMenuMusic(270f));
    }

    private void Update() {
        for (int i=0; i<floatingPieces.Count; i++)
        {
            floatingPieces[i].SetTheta(floatingPieces[i].theta + floatingPieces[i].direction * Time.deltaTime * movingSpeed);
            floatingPieces[i].SetPhi(floatingPieces[i].phi + floatingPieces[i].direction * Time.deltaTime * movingSpeed);
            floatingPieces[i].pieceObj.transform.position = GetPolarCoords(floatingPieces[i].radius, floatingPieces[i].theta, floatingPieces[i].phi);
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            uiDoc.GetComponent<UIBehaviour>().CloseOptions();
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
            newPiece.GetComponent<PieceIdleRotation>().enabled = true;
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

    IEnumerator PlayMenuMusic(float repeatTimer)
    {
        while (true)
        {
            gameObject.GetComponent<AudioManager>().Play("MenuTheme", 0f, 0.2f, 0f, 5000, 240);
            yield return new WaitForSecondsRealtime(repeatTimer);
        }
    }
}
