using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    private Quaternion whiteSide, blackSide, whitePrepare, blackPrepare;
    private int flipFrames = 40, chargeFrames = 10;
    private bool isWhite = true, isPrepared = false;

    private void Awake() {
        whiteSide = transform.rotation;
        transform.Rotate(0, 0, 20);
        whitePrepare = transform.rotation;
        transform.Rotate(0, 0, -200);
        blackSide = transform.rotation;
        transform.Rotate(0, 0, 20);
        blackPrepare = transform.rotation;

        GlobalEventManager.OnSelectionCancel += CancelPreparationFunc;
        GlobalEventManager.OnMoveMade += FlipFunc;
        GlobalEventManager.OnPieceSelected += PrepareFunc;
    }

    private void Start() {
        transform.rotation = whiteSide;
    }

    private void CancelPreparationFunc(bool trueCancel)
    {
        if (trueCancel && isPrepared)
        {
            StartCoroutine(CancelPreparation(chargeFrames));    
            isPrepared = false;        
        }
    }

    private void FlipFunc()
    {
        StartCoroutine(Flip(flipFrames));
    }

    private void PrepareFunc(GameObject obj)
    {
        if (!isPrepared)
        {        
            StartCoroutine(PrepareForFlip(chargeFrames));
            isPrepared = true;
        }
    }

    IEnumerator PrepareForFlip(int frameCount)
    {
        int currentFrame = 0;
        while (isWhite && currentFrame < frameCount)
        {
            transform.rotation = Quaternion.Lerp(whiteSide, whitePrepare, (float)currentFrame / (float)frameCount);
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        while (!isWhite && currentFrame < frameCount)
        {
            transform.rotation = Quaternion.Lerp(blackSide, blackPrepare, (float)currentFrame / (float)frameCount);
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator CancelPreparation(int frameCount)
    {
        int currentFrame = 0;
        while (isWhite && currentFrame < frameCount)
        {
            transform.rotation = Quaternion.Lerp(whitePrepare, whiteSide, (float)currentFrame / (float)frameCount);
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        while (!isWhite && currentFrame < frameCount)
        {
            transform.rotation = Quaternion.Lerp(blackPrepare, blackSide, (float)currentFrame / (float)frameCount);
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator Flip(int frameCount)
    {
        isPrepared = false;
        int currentFrame = 0;
        while (isWhite && currentFrame < frameCount)
        {
            transform.rotation = Quaternion.Lerp(whitePrepare, blackSide, (float)currentFrame / (float)frameCount);
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        while (!isWhite && currentFrame < frameCount)
        {
            transform.rotation = Quaternion.Lerp(blackPrepare, whiteSide, (float)currentFrame / (float)frameCount);
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        if (currentFrame == frameCount) isWhite = !isWhite;
        Debug.Log("animation played! now is " + (isWhite ? "white" : "black"));
    }
}
