using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceController : MonoBehaviour
{
    public int X, Y;
    private bool isMoving;
    private Transform modelTransformer;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MovePiece(1, 2, 1);
            StartCoroutine(MoveClockwise());
        }
    }

    IEnumerator MoveClockwise()
    {
        yield return new WaitUntil(() => !isMoving);
        MovePiece(2, 1, -1);
    }

    // ==================================================== LOGIC ============================================================

    private void Awake() {
        modelTransformer = transform.GetChild(0);
        isMoving = false;
    }

    public void MovePiece(int newX, int newY, int dirX)
    {
        isMoving = true;
        int dX, dY = newY - Y;
        StartCoroutine(MoveIn3D(true));
        
        if (dirX > 0 && (newX % 4) < X)
            dX = newX + 4 - X;
        else if (dirX < 0 && (newX % 4) > X)
            dX = newX - 4 - X;
        else
            dX = newX - X;
        
        StartCoroutine(MoveAcrossBoard(dX, dY, dirX, Mathf.Abs(dX) * 15));
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
