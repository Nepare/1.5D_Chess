using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIBehaviour : MonoBehaviour
{   
    private Button buttonStart, buttonQuit, buttonOptions, buttonApply, buttonDefault, buttonSkybox1, buttonSkybox2, buttonSkybox3;
    private Button buttonManageBoard, buttonManageControls, buttonDefaultBoard;
    private static Button[] buttonPieces;
    DropdownField dropdownLang;
    private VisualElement root, boardManagement, controlManagement;
    private Label lblInstructions, lblWarnings;
    private bool optionsOpen;
    private int skyboxChosen = 0;
    private System.Collections.Generic.Dictionary<string, string> boardConfig;

    private void OnEnable() {
        root = GetComponent<UIDocument>().rootVisualElement;
        buttonPieces = new Button[12];

        buttonStart = root.Q<Button>("ButtonStart");
        buttonOptions = root.Q<Button>("ButtonOptions");
        buttonQuit = root.Q<Button>("ButtonQuit");
        buttonApply = root.Q<Button>("Apply");
        buttonDefault = root.Q<Button>("Default");
        dropdownLang = root.Q<DropdownField>("langChooser");

        buttonSkybox1 = root.Q<Button>("skybox1");
        buttonSkybox2 = root.Q<Button>("skybox2");
        buttonSkybox3 = root.Q<Button>("skybox3");

        buttonManageBoard = root.Q<Button>("ButtonManageBoard");
        buttonManageControls = root.Q<Button>("ButtonManageControls");
        buttonDefaultBoard = root.Q<Button>("buttonDefaultBoard");

        boardManagement = root.Q<VisualElement>("boardManagement");
        controlManagement = root.Q<VisualElement>("controlManagement");
        lblInstructions = root.Q<Label>("lblInstructions");
        lblWarnings = root.Q<Label>("lblWarnings");

        SetUpConfigurer.root = root;

        for (int i = 0; i < 12; i++)
        {
            string pieceName = (i < 6) ? "w" : "b";
            if (i % 6 == 0) pieceName = pieceName + "k";
            if (i % 6 == 1) pieceName = pieceName + "q";
            if (i % 6 == 2) pieceName = pieceName + "b";
            if (i % 6 == 3) pieceName = pieceName + "h";
            if (i % 6 == 4) pieceName = pieceName + "r";
            if (i % 6 == 5) pieceName = pieceName + "p";

            buttonPieces[i] = root.Q<Button>(pieceName);
        }
        
        buttonQuit.clicked += ExitGame;
        buttonStart.clicked += MoveToGame;
        buttonOptions.clicked += OptionsToggle;
        buttonApply.clicked += ApplyChanges;
        buttonDefault.clicked += Default;

        buttonSkybox1.clicked += EnableSkybox1;
        buttonSkybox2.clicked += EnableSkybox2;
        buttonSkybox3.clicked += EnableSkybox3;

        buttonManageBoard.clicked += ManageBoard;
        buttonManageControls.clicked += ManageControls;
        buttonDefaultBoard.clicked += DefaultBoard;

        buttonPieces[0].clicked += ChoosePieceWk;
        buttonPieces[1].clicked += ChoosePieceWq;
        buttonPieces[2].clicked += ChoosePieceWb;
        buttonPieces[3].clicked += ChoosePieceWh;
        buttonPieces[4].clicked += ChoosePieceWr;
        buttonPieces[5].clicked += ChoosePieceWp;
        buttonPieces[6].clicked += ChoosePieceBk;
        buttonPieces[7].clicked += ChoosePieceBq;
        buttonPieces[8].clicked += ChoosePieceBb;
        buttonPieces[9].clicked += ChoosePieceBh;
        buttonPieces[10].clicked += ChoosePieceBr;
        buttonPieces[11].clicked += ChoosePieceBp;

        
        dropdownLang.index = LanguageController.LANG_ID - 1;
        UpdateLanguage();
        CloseOptions();
        SetUpBoardVisuals(SetUpConfigurer.SETUP_CONFIGURATION);
        SetUpConfigurer.SubscribeAllTiles();

        switch (LanguageController.SKYBOX_ID)
        {
            case 0: EnableSkybox1(); break;
            case 1: EnableSkybox2(); break;
            case 2: EnableSkybox3(); break;
        }
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
        LanguageController.SKYBOX_ID = skyboxChosen;
        UpdateLanguage();
        if (SetUpConfigurer.bkPos != "" && SetUpConfigurer.wkPos != "")
            SetUpConfigurer.UpdateSetUp();
        else
            root.Q<Label>("lblWarnings").text = LanguageController.GetWord("Menu.BoardControlKingAbsent");
    }

    private void Default()
    {
        LanguageController.LANG_ID = 1;
        LanguageController.SKYBOX_ID = 1;
        dropdownLang.index = 0;
        UpdateLanguage();
        EnableSkybox1();
        DefaultBoard();
    }

    private void DefaultBoard()
    {
        ClearBoardVisuals();
        SetUpBoardVisuals(SetUpConfigurer.GetDefaultBoardConfiguration());
        SetUpConfigurer.wkPos = "t0";
        SetUpConfigurer.bkPos = "t15";
        SetUpConfigurer.SETUP_CONFIGURATION = SetUpConfigurer.GetDefaultBoardConfiguration();
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
        ManageBoard();
    }

    public void CloseOptions()
    {
        VisualElement optionsZone = root.Q<VisualElement>("optionsZone");
        optionsZone.visible = false;
        controlManagement.visible = false;
        boardManagement.visible = false;

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
        buttonManageBoard.text = LanguageController.GetWord("Menu.ManageBoard");
        buttonManageControls.text = LanguageController.GetWord("Menu.ManageControls");
        buttonDefaultBoard.text = LanguageController.GetWord("Menu.DefaultBoard");
        lblInstructions.text = LanguageController.GetWord("Menu.BoardControlHint");
    }

    private void DisableAllSkyboxes()
    {
        buttonSkybox1.style.borderTopWidth = 0;
        buttonSkybox1.style.borderBottomWidth = 0;
        buttonSkybox1.style.borderRightWidth = 0;
        buttonSkybox1.style.borderLeftWidth = 0;

        buttonSkybox2.style.borderTopWidth = 0;
        buttonSkybox2.style.borderBottomWidth = 0;
        buttonSkybox2.style.borderRightWidth = 0;
        buttonSkybox2.style.borderLeftWidth = 0;

        buttonSkybox3.style.borderTopWidth = 0;
        buttonSkybox3.style.borderBottomWidth = 0;
        buttonSkybox3.style.borderRightWidth = 0;
        buttonSkybox3.style.borderLeftWidth = 0;
    }

    private void EnableSkybox1()
    {
        DisableAllSkyboxes();
        buttonSkybox1.style.borderTopWidth = 2;
        buttonSkybox1.style.borderBottomWidth = 2;
        buttonSkybox1.style.borderRightWidth = 2;
        buttonSkybox1.style.borderLeftWidth = 2;

        skyboxChosen = 0;
    }

    private void EnableSkybox2()
    {
        DisableAllSkyboxes();
        buttonSkybox2.style.borderTopWidth = 2;
        buttonSkybox2.style.borderBottomWidth = 2;
        buttonSkybox2.style.borderRightWidth = 2;
        buttonSkybox2.style.borderLeftWidth = 2;

        skyboxChosen = 1;
    }

    private void EnableSkybox3()
    {
        DisableAllSkyboxes();
        buttonSkybox3.style.borderTopWidth = 2;
        buttonSkybox3.style.borderBottomWidth = 2;
        buttonSkybox3.style.borderRightWidth = 2;
        buttonSkybox3.style.borderLeftWidth = 2;

        skyboxChosen = 2;
    }

    private void ManageBoard()
    {
        controlManagement.visible = false;
        boardManagement.visible = true;
        buttonManageBoard.style.borderTopWidth = 2;
        buttonManageBoard.style.borderBottomWidth = 2;
        buttonManageBoard.style.borderRightWidth = 2;
        buttonManageBoard.style.borderLeftWidth = 2;

        buttonManageControls.style.borderTopWidth = 0;
        buttonManageControls.style.borderBottomWidth = 0;
        buttonManageControls.style.borderRightWidth = 0;
        buttonManageControls.style.borderLeftWidth = 0;
    }

    private void ManageControls()
    {
        controlManagement.visible = true;
        boardManagement.visible = false;

        buttonManageBoard.style.borderTopWidth = 0;
        buttonManageBoard.style.borderBottomWidth = 0;
        buttonManageBoard.style.borderRightWidth = 0;
        buttonManageBoard.style.borderLeftWidth = 0;

        buttonManageControls.style.borderTopWidth = 2;
        buttonManageControls.style.borderBottomWidth = 2;
        buttonManageControls.style.borderRightWidth = 2;
        buttonManageControls.style.borderLeftWidth = 2;
    }


    private void ChoosePieceWk()
    {
        UnchooseAllPieces();
        Borderify(buttonPieces[0], 2);
        SetUpConfigurer.activePiece = "wk";
    }

    private void ChoosePieceBk()
    {
        UnchooseAllPieces();
        Borderify(buttonPieces[6], 2);
        SetUpConfigurer.activePiece = "bk";
    }

    private void ChoosePieceWq()
    {
        UnchooseAllPieces();
        Borderify(buttonPieces[1], 2);
        SetUpConfigurer.activePiece = "wq";
    }

    private void ChoosePieceBq()
    {
        UnchooseAllPieces();
        Borderify(buttonPieces[7], 2);
        SetUpConfigurer.activePiece = "bq";
    }

    private void ChoosePieceWb()
    {
        UnchooseAllPieces();
        Borderify(buttonPieces[2], 2);
        SetUpConfigurer.activePiece = "wb";
    }

    private void ChoosePieceBb()
    {
        UnchooseAllPieces();
        Borderify(buttonPieces[8], 2);
        SetUpConfigurer.activePiece = "bb";
    }

    private void ChoosePieceWh()
    {
        UnchooseAllPieces();
        Borderify(buttonPieces[3], 2);
        SetUpConfigurer.activePiece = "wh";
    }

    private void ChoosePieceBh()
    {
        UnchooseAllPieces();
        Borderify(buttonPieces[9], 2);
        SetUpConfigurer.activePiece = "bh";
    }

    private void ChoosePieceWr()
    {
        UnchooseAllPieces();
        Borderify(buttonPieces[4], 2);
        SetUpConfigurer.activePiece = "wr";
    }

    private void ChoosePieceBr()
    {
        UnchooseAllPieces();
        Borderify(buttonPieces[10], 2);
        SetUpConfigurer.activePiece = "br";
    }

    private void ChoosePieceWp()
    {
        UnchooseAllPieces();
        Borderify(buttonPieces[5], 2);
        SetUpConfigurer.activePiece = "wp";
    }

    private void ChoosePieceBp()
    {
        UnchooseAllPieces();
        Borderify(buttonPieces[11], 2);
        SetUpConfigurer.activePiece = "bp";
    }


    private static void Borderify(Button button, int edgeWidth)
    {
        button.style.borderBottomWidth = edgeWidth;
        button.style.borderTopWidth = edgeWidth;
        button.style.borderLeftWidth = edgeWidth;
        button.style.borderRightWidth = edgeWidth;
    }

    public static void UnchooseAllPieces()
    {
        for (int i = 0; i < buttonPieces.Length; i++)
        {
            Borderify(buttonPieces[i], 0);
        }
    }

    private void SetUpBoardVisuals(System.Collections.Generic.Dictionary<string, string> setup_dict)
    {
        foreach (var tileName in setup_dict.Keys)
        {
            Texture2D currentTexture = GetTextureFromName(setup_dict[tileName]);
            string new_y_coord = (System.Convert.ToInt32(tileName.Substring(1)) + 1).ToString();
            Button currentTile = root.Q<Button>(tileName[0] + new_y_coord);
            currentTile.style.backgroundImage = currentTexture;
        }
    }

    private void ClearBoardVisuals()
    {
        root.Q<Button>("e1").style.backgroundImage = null;
        root.Q<Button>("e2").style.backgroundImage = null;
        for (int i = 1; i <= 16; i++)
        {
            root.Q<Button>("l" + i.ToString()).style.backgroundImage = null;
            root.Q<Button>("t" + i.ToString()).style.backgroundImage = null;
            root.Q<Button>("r" + i.ToString()).style.backgroundImage = null;
            root.Q<Button>("b" + i.ToString()).style.backgroundImage = null;
        }
    }

    public static Texture2D GetTextureFromName(string pieceName)
    {
        Texture2D currentTexture = Resources.Load<Texture2D>("escape_" + pieceName);
        return currentTexture;
    }
}
