using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    private VisualElement root;
    private Label lblResult, lblPlayer;
    private VisualElement pieceRight, pieceLeft;
    [SerializeField] private Texture2D whiteQueen, blackQueen;

    private void OnEnable() {
    root = GetComponent<UIDocument>().rootVisualElement;

    Button btnRestart = root.Q<Button>("Restart");
    Button btnTitleScreen = root.Q<Button>("MainMenu");
    lblResult = root.Q<Label>("lbl_result");
    lblPlayer = root.Q<Label>("lbl_player");
    pieceLeft = root.Q<VisualElement>("piece_container1");
    pieceRight = root.Q<VisualElement>("piece_container2");
    root.visible = false;

    btnRestart.clicked += Restart;
    btnTitleScreen.clicked += TitleScreen;
    
    GlobalEventManager.OnPlayerCheckmated += HandleCheckmate;
    GlobalEventManager.OnPlayerStalemated += HandleStalemate;
    }

    private void Restart()
    {
        EscapeMenu.isPaused = false;
        Time.timeScale = 1;
        GlobalEventManager.UnsubscribeAll();
        SceneManager.LoadScene(1);
    }

    private void TitleScreen()
    {        
        EscapeMenu.isPaused = false;
        Time.timeScale = 1;
        GlobalEventManager.UnsubscribeAll();
        SceneManager.LoadScene(0);
    }

    private void OpenGameOverMenu()
    {
        root.visible = true;
        EscapeMenu.isPaused = true;
        Time.timeScale = 0;
    }

    private void HandleCheckmate(string message)
    {
        lblResult.text = LanguageController.GetWord("GameOver.Checkmate");
        if (message[0] == 'w')
        {
            lblPlayer.text = LanguageController.GetWord("GameOver.BlackWon");
            pieceLeft.style.backgroundImage = new StyleBackground(Background.FromTexture2D(blackQueen));
            pieceRight.style.backgroundImage = new StyleBackground(Background.FromTexture2D(blackQueen));
        }
        else
        {
            lblPlayer.text = LanguageController.GetWord("GameOver.WhiteWon");
            pieceLeft.style.backgroundImage = new StyleBackground(Background.FromTexture2D(whiteQueen));
            pieceRight.style.backgroundImage = new StyleBackground(Background.FromTexture2D(whiteQueen));
        }
        OpenGameOverMenu();
    }

    private void HandleStalemate(string message)
    {
        lblResult.text = LanguageController.GetWord("GameOver.Stalemate");
        if (message[0] == 'w')
        {
            lblPlayer.text = LanguageController.GetWord("GameOver.WhiteStalemate");
            pieceLeft.style.backgroundImage = new StyleBackground(Background.FromTexture2D(blackQueen));
            pieceRight.style.backgroundImage = new StyleBackground(Background.FromTexture2D(whiteQueen));
        }
        else
        {
            lblPlayer.text = LanguageController.GetWord("GameOver.BlackStalemate");
            pieceLeft.style.backgroundImage = new StyleBackground(Background.FromTexture2D(whiteQueen));
            pieceRight.style.backgroundImage = new StyleBackground(Background.FromTexture2D(blackQueen));
        }
        OpenGameOverMenu();
    }
}
