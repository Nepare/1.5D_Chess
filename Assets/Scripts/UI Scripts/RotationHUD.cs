using UnityEngine;
using UnityEngine.UIElements;

public class RotationHUD : MonoBehaviour
{
    private BoardRotation boardRotator;
    private VisualElement hudZone;
    Button btnToggle;
    public GameObject board; 
    private bool isMovingLeft = false, 
                 isMovingRight = false, 
                 isSpinningClockwise = false, 
                 isSpinningAntiClockwise = false, 
                 isSpinningForward = false, 
                 isSpinningBackward = false,
                 isZoomingIn = false,
                 isZoomingOut = false,
                 isVisibleHUD = true;

    private void OnEnable() {
        boardRotator = board.GetComponent<BoardRotation>();
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        hudZone = root.Q<VisualElement>("HUDZone");

        Button btnMoveLeft = root.Q<Button>("btn_left");
        Button btnMoveRight = root.Q<Button>("btn_right");
        Button btnSpinForward = root.Q<Button>("btn_towards");
        Button btnSpinBackward = root.Q<Button>("btn_back");
        Button btnSpinClockwise = root.Q<Button>("btn_clockwise");
        Button btnSpinAntiClockwise = root.Q<Button>("btn_anticlockwise");
        Button btnZoomIn = root.Q<Button>("btn_zoomin");
        Button btnZoomOut = root.Q<Button>("btn_zoomout");
        Button btnDefault = root.Q<Button>("btn_center");
        btnToggle = root.Q<Button>("btn_toggle");

        btnDefault.clicked += DefaultPosition;
        btnToggle.clicked += ToggleVisibility;
        btnSpinForward.RegisterCallback<PointerCaptureEvent>(SpinForward, TrickleDown.TrickleDown);
        btnSpinBackward.RegisterCallback<PointerCaptureEvent>(SpinBack, TrickleDown.TrickleDown);
        btnSpinClockwise.RegisterCallback<PointerCaptureEvent>(SpinClockwise, TrickleDown.TrickleDown);
        btnSpinAntiClockwise.RegisterCallback<PointerCaptureEvent>(SpinAntiClockwise, TrickleDown.TrickleDown);
        btnMoveLeft.RegisterCallback<PointerCaptureEvent>(MoveLeft, TrickleDown.TrickleDown);
        btnMoveRight.RegisterCallback<PointerCaptureEvent>(MoveRight, TrickleDown.TrickleDown);
        btnZoomIn.RegisterCallback<PointerCaptureEvent>(ZoomIn, TrickleDown.TrickleDown);
        btnZoomOut.RegisterCallback<PointerCaptureEvent>(ZoomOut, TrickleDown.TrickleDown);
        
        btnSpinForward.RegisterCallback<PointerCaptureOutEvent>(StopAll, TrickleDown.TrickleDown);
        btnSpinBackward.RegisterCallback<PointerCaptureOutEvent>(StopAll, TrickleDown.TrickleDown);
        btnSpinClockwise.RegisterCallback<PointerCaptureOutEvent>(StopAll, TrickleDown.TrickleDown);
        btnSpinAntiClockwise.RegisterCallback<PointerCaptureOutEvent>(StopAll, TrickleDown.TrickleDown);
        btnMoveLeft.RegisterCallback<PointerCaptureOutEvent>(StopAll, TrickleDown.TrickleDown);
        btnMoveRight.RegisterCallback<PointerCaptureOutEvent>(StopAll, TrickleDown.TrickleDown);
        btnZoomIn.RegisterCallback<PointerCaptureOutEvent>(StopAll, TrickleDown.TrickleDown);
        btnZoomOut.RegisterCallback<PointerCaptureOutEvent>(StopAll, TrickleDown.TrickleDown);

        ToggleVisibility();
    }

    private void ControlBoard()
    {
        if (isMovingLeft) boardRotator.MoveAlongBoard(0, 180, 1, false);
        if (isMovingRight) boardRotator.MoveAlongBoard(0, 180, -1, false);
        if (isSpinningClockwise) boardRotator.RotateCamera(1);
        if (isSpinningAntiClockwise) boardRotator.RotateCamera(-1);
        if (isSpinningBackward) boardRotator.RotateBoard(-0.3f);
        if (isSpinningForward) boardRotator.RotateBoard(0.3f);
        if (isZoomingIn) boardRotator.Zoom(0.02f);
        if (isZoomingOut) boardRotator.Zoom(-0.02f);
    }

    private void Update() {
        ControlBoard();    
    }

    private void ZoomIn(PointerCaptureEvent evt)
    {
        isZoomingIn = true;
    }

    private void ZoomOut(PointerCaptureEvent evt)
    {
        isZoomingOut = true;
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
        isZoomingIn = false;
        isZoomingOut = false;
    }

    private void ToggleVisibility()
    {
        if (isVisibleHUD) 
        { 
            hudZone.visible = false;
            btnToggle.text = LanguageController.GetWord("HUD.ShowCameraControls");
        }
        else 
        {
            hudZone.visible = true;
            btnToggle.text = LanguageController.GetWord("HUD.HideCameraControls");;
        }
        isVisibleHUD = !isVisibleHUD;
    }
}
