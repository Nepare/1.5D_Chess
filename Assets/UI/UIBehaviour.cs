using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIBehaviour : MonoBehaviour
{   
    private void OnEnable() {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Button buttonStart = root.Q<Button>("ButtonStart");
        Button buttonQuit = root.Q<Button>("ButtonQuit");

        buttonQuit.clicked += ExitGame;
        buttonStart.clicked += MoveToGame;
    }

    private void MoveToGame()
    {
        SceneManager.LoadScene(1);
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}
