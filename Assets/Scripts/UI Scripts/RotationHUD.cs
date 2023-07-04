using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class RotationHUD : MonoBehaviour
{
    private BoardRotation boardRotator;
    public GameObject board; 
    private bool isMovingLeft = false, 
                 isMovingRight = false, 
                 isSpinningClockwise = false, 
                 isSpinningAntiClockwise = false, 
                 isSpinningForward = false, 
                 isSpinningBackward = false;

    private void OnEnable() {
        boardRotator = board.GetComponent<BoardRotation>();
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Button btnMoveLeft = root.Q<Button>("MoveLeft");
        Button btnMoveRight = root.Q<Button>("MoveRight");
        Button btnSpinForward = root.Q<Button>("SpinForward");
        Button btnSpinBackward = root.Q<Button>("SpinBack");
        Button btnSpinClockwise = root.Q<Button>("SpinClockwise");
        Button btnSpinAntiClockwise = root.Q<Button>("SpinAntiClockwise");
        Button btnDefault = root.Q<Button>("Default");

        btnDefault.clicked += DefaultPosition;
        btnSpinForward.RegisterCallback<PointerCaptureEvent>(SpinForward, TrickleDown.TrickleDown);
        btnSpinBackward.RegisterCallback<PointerCaptureEvent>(SpinBack, TrickleDown.TrickleDown);
        btnSpinClockwise.RegisterCallback<PointerCaptureEvent>(SpinClockwise, TrickleDown.TrickleDown);
        btnSpinAntiClockwise.RegisterCallback<PointerCaptureEvent>(SpinAntiClockwise, TrickleDown.TrickleDown);
        btnMoveLeft.RegisterCallback<PointerCaptureEvent>(MoveLeft, TrickleDown.TrickleDown);
        btnMoveRight.RegisterCallback<PointerCaptureEvent>(MoveRight, TrickleDown.TrickleDown);

        
        btnSpinForward.RegisterCallback<PointerCaptureOutEvent>(StopAll, TrickleDown.TrickleDown);
        btnSpinBackward.RegisterCallback<PointerCaptureOutEvent>(StopAll, TrickleDown.TrickleDown);
        btnSpinClockwise.RegisterCallback<PointerCaptureOutEvent>(StopAll, TrickleDown.TrickleDown);
        btnSpinAntiClockwise.RegisterCallback<PointerCaptureOutEvent>(StopAll, TrickleDown.TrickleDown);
        btnMoveLeft.RegisterCallback<PointerCaptureOutEvent>(StopAll, TrickleDown.TrickleDown);
        btnMoveRight.RegisterCallback<PointerCaptureOutEvent>(StopAll, TrickleDown.TrickleDown);
    }

    private void Update() {
        ControlBoard();    
    }

    private void ControlBoard()
    {
        if (isMovingLeft) boardRotator.MoveAlongBoard(0, 180, 1, false);
        if (isMovingRight) boardRotator.MoveAlongBoard(0, 180, -1, false);
        if (isSpinningBackward) boardRotator.RotateBoard(-0.1f);
        if (isSpinningForward) boardRotator.RotateBoard(0.1f);
        if (isSpinningClockwise) boardRotator.RotateCamera(1);
        if (isSpinningAntiClockwise) boardRotator.RotateCamera(-1);
    }

    private void DefaultPosition()
    {
        GlobalEventManager.SendCameraDefault();
    }

    private void SpinForward(PointerCaptureEvent evt)
    {
        isSpinningForward = true;
    }

    private void SpinBack(PointerCaptureEvent evt)
    {
        isSpinningBackward = true;
    }

    private void SpinClockwise(PointerCaptureEvent evt)
    {
        isSpinningClockwise = true;
    }

    private void SpinAntiClockwise(PointerCaptureEvent evt)
    {
        isSpinningAntiClockwise = true;
    }

    private void MoveLeft(PointerCaptureEvent evt)
    {
        isMovingLeft = true;
    }

    private void MoveRight(PointerCaptureEvent evt)
    {
        isMovingRight = true;
    }

    private void StopAll(PointerCaptureOutEvent evt)
    {
        isMovingLeft = false; 
        isMovingRight = false; 
        isSpinningClockwise = false;
        isSpinningAntiClockwise = false; 
        isSpinningForward = false; 
        isSpinningBackward = false;
    }
}
