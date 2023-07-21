using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIBehaviour : MonoBehaviour
{   
    private Button buttonStart, buttonQuit, buttonOptions, buttonApply, buttonDefault;
    DropdownField dropdownLang;
    VisualElement root;
    private bool optionsOpen;

    private void OnEnable() {
        root = GetComponent<UIDocument>().rootVisualElement;

        buttonStart = root.Q<Button>("ButtonStart");
        buttonOptions = root.Q<Button>("ButtonOptions");
        buttonQuit = root.Q<Button>("ButtonQuit");
        buttonApply = root.Q<Button>("Apply");
        buttonDefault = root.Q<Button>("Default");
        dropdownLang = root.Q<DropdownField>("langChooser");
        
        buttonQuit.clicked += ExitGame;
        buttonStart.clicked += MoveToGame;
        buttonOptions.clicked += OptionsToggle;
        buttonApply.clicked += ApplyChanges;
        buttonDefault.clicked += Default;
        
        dropdownLang.index = LanguageController.LANG_ID - 1;
        UpdateLanguage();
        CloseOptions();
    }

    private void MoveToGame()
    {
        SceneManager.LoadScene(1);
    }

    private void ExitGame()
    {
        Application.Quit();
    }

    private void ApplyChanges()
    {
        LanguageController.LANG_ID = dropdownLang.index + 1;
        UpdateLanguage();
    }

    private void Default()
    {
        LanguageController.LANG_ID = 1;
        dropdownLang.index = 0;
        UpdateLanguage();
    }

    private void OptionsToggle()
    {
        if (optionsOpen) CloseOptions();
        else OpenOptions();
    }

    private void OpenOptions()
    {
        VisualElement optionsZone = root.Q<VisualElement>("optionsZone");
        optionsZone.visible = true;
        optionsOpen = true;
    }

    public void CloseOptions()
    {
        VisualElement optionsZone = root.Q<VisualElement>("optionsZone");
        optionsZone.visible = false;
        optionsOpen = false;
    }

    public void UpdateLanguage()
    {
        buttonStart.text = LanguageController.GetWord("Menu.Start");
        buttonOptions.text = LanguageController.GetWord("Menu.Options");
        buttonQuit.text = LanguageController.GetWord("Menu.Quit");
        dropdownLang.label = LanguageController.GetWord("Menu.Language");
        buttonApply.text = LanguageController.GetWord("Menu.Apply");
        buttonDefault.text = LanguageController.GetWord("Menu.Default");
    }
}
