using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

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

        btnContinue.text = LanguageController.GetWord("Escape.Continue");
        btnRestart.text = LanguageController.GetWord("Escape.Restart");
        btnTitleScreen.text = LanguageController.GetWord("Escape.Quit");
    }

    private void Update() {
        if (Input.GetKeyDown(Keybinds.keybinds["escape"]))
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

    public void IncreasePieceCount(string promotedPiece)
    {
        Label increasingCountLabel = root.Q<Label>(promotedPiece + "_count");
        increasingCountLabel.text = (int.Parse(increasingCountLabel.text) + 1).ToString();
    }

    public void SetupPieces(List<string> names)
    {
        foreach (string name in names)
        {
            string finalName;
            if (name[1] == 'k' && name[2] == 'i')
                continue;
            else if (name[1] == 'k' && name[2] == 'n')
                finalName = name[0] + "h";
            else 
                finalName = name[0] + name[1].ToString();
            
            Label countLabel = root.Q<Label>(finalName + "_count");
            countLabel.text = (int.Parse(countLabel.text) + 1).ToString();
        }
    }
}
