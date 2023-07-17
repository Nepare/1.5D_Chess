using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgePieceIdleAnimation : MonoBehaviour
{
    private int idleTimer;
    private float swaySpeed = 0.2f;
    private float frameTimePeriod = 0.001f;
    private bool isSwayingUp, disabledSwaying; 
    private Transform modelTransformer;

    private void Awake() {
        idleTimer = 500;
        isSwayingUp = true;
        disabledSwaying = false;

        string piece = gameObject.GetComponent<PieceController>().piece;
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

        modelTransformer.Translate(new Vector3(0, 0.5f, 0));
    }

    private void Start() {
        StartCoroutine(Swaying(idleTimer));
    }

    IEnumerator Swaying(int maxFrames)
    {
        int peakSpeedFrame = maxFrames / 2;
        while (!disabledSwaying)
        {
            while (idleTimer > 0 && !disabledSwaying)
            {
                float frameMovement = ((isSwayingUp ? 1 : -1) * swaySpeed * Time.deltaTime) * ((float)(-Mathf.Abs(idleTimer - peakSpeedFrame) + peakSpeedFrame) / peakSpeedFrame);
                modelTransformer.Translate(new Vector3(0, frameMovement, 0), Space.Self);
                idleTimer--;
                yield return new WaitForSeconds(frameTimePeriod);
            }
            idleTimer = maxFrames;
            isSwayingUp = !isSwayingUp;
        }
    }

    public void DisableSway()
    {
        disabledSwaying = true;
    }
}
