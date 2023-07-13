using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class EscapeMenu : MonoBehaviour
{
    public static bool isPaused;

    private VisualElement root;

    private void OnEnable() {
        root = GetComponent<UIDocument>().rootVisualElement;

        Button btnContinue = root.Q<Button>("btn_continue");
        Button btnRestart = root.Q<Button>("btn_restart");
        Button btnTitleScreen = root.Q<Button>("btn_titlescreen");
        Continue();

        btnContinue.clicked += Continue;
        btnRestart.clicked += Restart;
        btnTitleScreen.clicked += TitleScreen;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused) CallMenu();
            else Continue();
        }
    }

    private void CallMenu()
    {
        root.visible = true;
        isPaused = true;
        Time.timeScale = 0;
    }

    private void Continue()
    {
        root.visible = false;
        isPaused = false;
        Time.timeScale = 1;
    }

    private void Restart()
    {
        isPaused = false;
        Time.timeScale = 1;
        GlobalEventManager.UnsubscribeAll();
        SceneManager.LoadScene(1);
    }

    private void TitleScreen()
    {        
        isPaused = false;
        Time.timeScale = 1;
        GlobalEventManager.UnsubscribeAll();
        SceneManager.LoadScene(0);
    }

    public void DecreasePieceCount(string eatenPiece)
    {
        Label decreasingCountLabel = root.Q<Label>(eatenPiece + "_count");
        decreasingCountLabel.text = (int.Parse(decreasingCountLabel.text) - 1).ToString();
    }
}
