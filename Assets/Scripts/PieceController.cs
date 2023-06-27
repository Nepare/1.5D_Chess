using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceController : MonoBehaviour
{
    public int X, Y;
    public string piece;
    private bool isMoving;
    private Transform modelTransformer;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MovePiece(X - 4, Y + 2, 1);
            StartCoroutine(MoveClockwise());
        }
    }

    IEnumerator MoveClockwise()
    {
        yield return new WaitUntil(() => !isMoving);
        MovePiece(X + 1, Y + 1, -1);
    }

    // ==================================================== LOGIC ============================================================

    private void Awake() {
        modelTransformer = transform.GetChild(0);
        isMoving = false;
    }

    public void AssignPiece(string pieceName)
    {
        HideAllModels();
        piece = pieceName;
        switch(piece)
        {
            case "king":
                modelTransformer = transform.GetChild(0);
                break;
            case "bishop":
                modelTransformer = transform.GetChild(1);
                break;
            case "knight":
                modelTransformer = transform.GetChild(2);
                break;
            case "pawn":
                modelTransformer = transform.GetChild(3);
                break;
            case "rook":
                modelTransformer = transform.GetChild(4);
                break;
            case "queen":
                modelTransformer = transform.GetChild(5);
                break;                
            default:
                modelTransformer = transform.GetChild(3);
                break;
        }
        modelTransformer.gameObject.SetActive(true);
    }

    public void SetInPlace(int coordX, int coordY)
    {
        Vector3 pos = new Vector3(0, 0, 7.5f - coordY);
        Vector3 rotation = new Vector3(0, 0, -90f + (90f * coordX));
        transform.localPosition = pos;
        transform.eulerAngles = rotation;
        
        X = coordX;
        Y = coordY;
    }

    public void EnableOutline()
    {
        modelTransformer.GetChild(0).GetComponent<Outline>().enabled = true;
    }

    public void DisableOutline()
    {
        modelTransformer.GetChild(0).GetComponent<Outline>().enabled = false;
    }

    private void HideAllModels()
    {
        for(int i=0; i<transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);
    }

    public void MovePiece(int newX, int newY, int dirX)
    {
        isMoving = true;
        int dX, dY = newY - Y;
        StartCoroutine(MoveIn3D(true));
        
        dX = newX - X;
        
        int numberOfFramesForAnimation = Mathf.Abs(dX) * 15;
        if (numberOfFramesForAnimation == 0) numberOfFramesForAnimation = 15;
        StartCoroutine(MoveAcrossBoard(dX, dY, dirX, numberOfFramesForAnimation));
        X = newX;
        while (X < 0) X += 4;
        X %= 4;
        Y = newY;
    }

    IEnumerator MoveIn3D(bool isMovingUp)
    {
        if (isMovingUp)
        {        
            float verticalVelocity = 0.065f;
            while (modelTransformer.localPosition.y - 0.5f < 0.75f)
            {
                modelTransformer.Translate(new Vector3(0, verticalVelocity, 0), Space.Self);
                if (verticalVelocity > 0.005) verticalVelocity -= 0.001f;
                yield return new WaitForFixedUpdate();
            }
        }   
        else
        {
            float verticalVelocity = 0f;
            while (modelTransformer.localPosition.y - 0.5f > 0f)
            {
                modelTransformer.Translate(new Vector3(0, -verticalVelocity, 0), Space.Self);
                if (verticalVelocity < 0.065) verticalVelocity += 0.005f;
                yield return new WaitForFixedUpdate();
            }
            isMoving = false;
        } 
    }

    IEnumerator MoveAcrossBoard(int dX, int dY, int dirX, int frames)
    {
        Vector3 startPos = modelTransformer.localPosition;
        Vector3 newPos = new Vector3(startPos.x, startPos.y, startPos.z - dY);

        int interpolationFramesCount = frames;
        int degreesPerFrame = Mathf.Abs(dX) * 90 / interpolationFramesCount;

        for (int i = 1; i <= interpolationFramesCount; i++)
        {
            float interpolationRatio = (float)i / interpolationFramesCount;
            Vector3 interpolatedPosition = Vector3.Lerp(new Vector3(startPos.x, modelTransformer.localPosition.y, startPos.z), new Vector3(newPos.x, modelTransformer.localPosition.y, newPos.z), interpolationRatio);
            transform.Rotate(new Vector3(0, 0, dirX * degreesPerFrame), Space.Self);
            modelTransformer.localPosition = interpolatedPosition;
            yield return new WaitForFixedUpdate();
        }
        StartCoroutine(MoveIn3D(false));
    }
}
