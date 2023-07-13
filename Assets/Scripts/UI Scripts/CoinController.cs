using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    private Quaternion whiteSide, blackSide, whitePrepare, blackPrepare;
    private int flipFrames, chargeFrames;
    private bool isWhite = true, isPrepared = false;
    private float canvasWidth, canvasHeight;

    private void Awake() {
        flipFrames = 40; 
        chargeFrames = 10;

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

        RectTransform objectRectTransform = transform.GetComponentInParent<RectTransform>();
        canvasHeight = objectRectTransform.rect.height;
        canvasWidth = objectRectTransform.rect.width;
        transform.localPosition = new Vector3(canvasWidth / 2 - (transform.localScale.x * 1.25f), canvasHeight / 2 - (transform.localScale.y * 1.25f), 0);
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
    }
}
