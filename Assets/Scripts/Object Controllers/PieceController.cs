using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceController : MonoBehaviour
{
    public int X, Y;
    public string piece;
    private Transform modelTransformer;

    // ==================================================== LOGIC ============================================================

    private void Awake() {
        modelTransformer = transform.GetChild(0);
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
        Vector3 pos, rotation;
        if (coordX == -1)
        {
            pos = new Vector3(0, 0, 7.5f * coordY);
            rotation = new Vector3(180 - 90 * coordY, 0, 0);
        }
        else
        {
            pos = new Vector3(0, 0, 7.5f - coordY);
            rotation = new Vector3(0, 0, -90f + (90f * coordX));
        }
        transform.localPosition = pos;
        transform.localEulerAngles = rotation;
        
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
        if(X == -1)
        {
            bool whiteSide = (Y == 1 ? true : false);
            gameObject.GetComponent<EdgePieceIdleAnimation>().DisableSway();
            StartCoroutine(EnterBoard(whiteSide, newX));
            X = newX;
            Y = newY;
            return;
        }

        //ANIMATION STARTS HERE
        int dX, dY = newY - Y;
        StartCoroutine(MoveIn3D(true));
        
        dX = newX - X;
        
        int numberOfFramesForAnimation = ((30 - (Mathf.Abs(dX) - 1))/2) * Mathf.Abs(dX); //arithmetic progression to accelerate animation
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
            StartCoroutine(CorrectYLevelPosition(15));
            // ANIMATION FINISHES HERE
        } 
    }

    IEnumerator EnterBoard(bool whiteSide, int destinationTile)
    {
        Vector3 deltaRotation = Vector3.zero;
        switch (destinationTile)
        {
            case 0:
                deltaRotation = new Vector3(0, 0, -90);
                break;
            case 1:
                deltaRotation = new Vector3(-90 * (whiteSide ? 1 : -1), 0, 0);
                break;
            case 2:
                deltaRotation = new Vector3(0, 0, 90);
                break;
            case 3:
                deltaRotation = new Vector3(90 * (whiteSide ? 1 : -1), 0, 0);
                break;
        }

        float frameCount = 20f;
        Vector3 degreesPerFrame = deltaRotation / frameCount;
        
        for (int i = 0; i < frameCount; i++)
        {
            transform.Rotate(degreesPerFrame, Space.Self);
            yield return new WaitForFixedUpdate();
        }

        StartCoroutine(MoveIn3D(false));
        SetInPlace(destinationTile, whiteSide ? 0 : 15);
        GetComponent<EdgePieceIdleAnimation>().enabled = false;
    }

    IEnumerator MoveAcrossBoard(int dX, int dY, int dirX, int frames)
    {
        Vector3 startPos = modelTransformer.localPosition;
        Vector3 newPos = new Vector3(startPos.x, startPos.y, startPos.z - dY);

        int interpolationFramesCount = frames;
        float degreesPerFrame = Mathf.Abs(dX) * 90f / interpolationFramesCount;

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

    IEnumerator CorrectYLevelPosition(int framesCount)
    {
        Vector3 startPosition = modelTransformer.localPosition;
        for (int i=0; i<framesCount; i++)
        {
            modelTransformer.localPosition = Vector3.Lerp(startPosition, new Vector3(startPosition.x, 0.5f, startPosition.z), (float)i/framesCount);
            yield return new WaitForFixedUpdate();
        }
    }
}
