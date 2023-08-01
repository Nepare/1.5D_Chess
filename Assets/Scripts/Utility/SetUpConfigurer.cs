using UnityEngine.UIElements;
using System.Linq;
using UnityEngine;

public class SetUpConfigurer : MonoBehaviour
{
    public static System.Collections.Generic.Dictionary<string, string> SETUP_CONFIGURATION = GetDefaultBoardConfiguration();
    private static System.Collections.Generic.Dictionary<string, string> current_config = new System.Collections.Generic.Dictionary<string, string>();
    public static string activePiece = "";
    public static string wkPos = "t0", bkPos = "t15";
    public static VisualElement root;

    public static System.Collections.Generic.Dictionary<string, string> GetDefaultBoardConfiguration()
    {
        System.Collections.Generic.Dictionary<string, string> default_conf = new System.Collections.Generic.Dictionary<string, string>();
        default_conf["l0"] = "wb";
        default_conf["t0"] = "wk";
        default_conf["r0"] = "wh";
        default_conf["b0"] = "wr";

        default_conf["l1"] = "wp";
        default_conf["t1"] = "wp";
        default_conf["r1"] = "wp";
        default_conf["b1"] = "wp";

        default_conf["e0"] = "wq";

        default_conf["l15"] = "bh";
        default_conf["t15"] = "bk";
        default_conf["r15"] = "bb";
        default_conf["b15"] = "br";

        default_conf["l14"] = "bp";
        default_conf["t14"] = "bp";
        default_conf["r14"] = "bp";
        default_conf["b14"] = "bp";

        default_conf["e1"] = "bq";

        return default_conf;
    }

    public static void UpdateSetUp()
    {
        SETUP_CONFIGURATION = current_config;
    }

    public static void SubscribeAllTiles()
    {
        current_config = SETUP_CONFIGURATION.ToDictionary(entry => entry.Key, entry => entry.Value);
        root.Q<Button>("e1").clicked += TileE0Clicked;
        root.Q<Button>("e2").clicked += TileE1Clicked;
        root.Q<Button>("l1").clicked += TileL0Clicked;
        root.Q<Button>("l2").clicked += TileL1Clicked;
        root.Q<Button>("l3").clicked += TileL2Clicked;
        root.Q<Button>("l4").clicked += TileL3Clicked;
        root.Q<Button>("l5").clicked += TileL4Clicked;
        root.Q<Button>("l6").clicked += TileL5Clicked;
        root.Q<Button>("l7").clicked += TileL6Clicked;
        root.Q<Button>("l8").clicked += TileL7Clicked;
        root.Q<Button>("l9").clicked += TileL8Clicked;
        root.Q<Button>("l10").clicked += TileL9Clicked;
        root.Q<Button>("l11").clicked += TileL10Clicked;
        root.Q<Button>("l12").clicked += TileL11Clicked;
        root.Q<Button>("l13").clicked += TileL12Clicked;
        root.Q<Button>("l14").clicked += TileL13Clicked;
        root.Q<Button>("l15").clicked += TileL14Clicked;
        root.Q<Button>("l16").clicked += TileL15Clicked;
        root.Q<Button>("t1").clicked += TileT0Clicked;
        root.Q<Button>("t2").clicked += TileT1Clicked;
        root.Q<Button>("t3").clicked += TileT2Clicked;
        root.Q<Button>("t4").clicked += TileT3Clicked;
        root.Q<Button>("t5").clicked += TileT4Clicked;
        root.Q<Button>("t6").clicked += TileT5Clicked;
        root.Q<Button>("t7").clicked += TileT6Clicked;
        root.Q<Button>("t8").clicked += TileT7Clicked;
        root.Q<Button>("t9").clicked += TileT8Clicked;
        root.Q<Button>("t10").clicked += TileT9Clicked;
        root.Q<Button>("t11").clicked += TileT10Clicked;
        root.Q<Button>("t12").clicked += TileT11Clicked;
        root.Q<Button>("t13").clicked += TileT12Clicked;
        root.Q<Button>("t14").clicked += TileT13Clicked;
        root.Q<Button>("t15").clicked += TileT14Clicked;
        root.Q<Button>("t16").clicked += TileT15Clicked;
        root.Q<Button>("r1").clicked += TileR0Clicked;
        root.Q<Button>("r2").clicked += TileR1Clicked;
        root.Q<Button>("r3").clicked += TileR2Clicked;
        root.Q<Button>("r4").clicked += TileR3Clicked;
        root.Q<Button>("r5").clicked += TileR4Clicked;
        root.Q<Button>("r6").clicked += TileR5Clicked;
        root.Q<Button>("r7").clicked += TileR6Clicked;
        root.Q<Button>("r8").clicked += TileR7Clicked;
        root.Q<Button>("r9").clicked += TileR8Clicked;
        root.Q<Button>("r10").clicked += TileR9Clicked;
        root.Q<Button>("r11").clicked += TileR10Clicked;
        root.Q<Button>("r12").clicked += TileR11Clicked;
        root.Q<Button>("r13").clicked += TileR12Clicked;
        root.Q<Button>("r14").clicked += TileR13Clicked;
        root.Q<Button>("r15").clicked += TileR14Clicked;
        root.Q<Button>("r16").clicked += TileR15Clicked;
        root.Q<Button>("b1").clicked += TileB0Clicked;
        root.Q<Button>("b2").clicked += TileB1Clicked;
        root.Q<Button>("b3").clicked += TileB2Clicked;
        root.Q<Button>("b4").clicked += TileB3Clicked;
        root.Q<Button>("b5").clicked += TileB4Clicked;
        root.Q<Button>("b6").clicked += TileB5Clicked;
        root.Q<Button>("b7").clicked += TileB6Clicked;
        root.Q<Button>("b8").clicked += TileB7Clicked;
        root.Q<Button>("b9").clicked += TileB8Clicked;
        root.Q<Button>("b10").clicked += TileB9Clicked;
        root.Q<Button>("b11").clicked += TileB10Clicked;
        root.Q<Button>("b12").clicked += TileB11Clicked;
        root.Q<Button>("b13").clicked += TileB12Clicked;
        root.Q<Button>("b14").clicked += TileB13Clicked;
        root.Q<Button>("b15").clicked += TileB14Clicked;
        root.Q<Button>("b16").clicked += TileB15Clicked;
    }

    private static void TileClickBehaviour(string tileName)
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey(tileName)) current_config.Remove(tileName);
            if (wkPos == tileName) wkPos = "";
            if (bkPos == tileName) bkPos = "";
        }
        else current_config[tileName] = activePiece;
        if (activePiece == "wk" && wkPos != tileName)
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = tileName;
        }
        if (activePiece == "bk" && bkPos != tileName)
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = tileName;
        }
        if ((activePiece == "bp" && tileName.Substring(1) == "0") || (activePiece == "wp" && tileName.Substring(1) == "15"))
        {
            root.Q<Label>("lblWarnings").text = LanguageController.GetWord("Menu.BoardControlPawnPromotion");
            return;
        }
        root.Q<Button>(tileName[0] + (System.Convert.ToInt32(tileName.Substring(1)) + 1).ToString()).style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileE0Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("e0")) current_config.Remove("e0");
        }
        else if (activePiece[1] == 'k') 
        {
            root.Q<Label>("lblWarnings").text = LanguageController.GetWord("Menu.BoardControlKingEdge");
            return;
        }
        else if (activePiece == "bp")
        {
            root.Q<Label>("lblWarnings").text = LanguageController.GetWord("Menu.BoardControlPawnEdge");
            return;
        }
        else 
        {
            current_config["e0"] = activePiece;
        }
        root.Q<Button>("e1").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileE1Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("e1")) current_config.Remove("e1");
        }
        else if (activePiece[1] == 'k') 
        {
            root.Q<Label>("lblWarnings").text = LanguageController.GetWord("Menu.BoardControlKingEdge");
            return;
        }
        else if (activePiece == "wp")
        {
            root.Q<Label>("lblWarnings").text = LanguageController.GetWord("Menu.BoardControlPawnEdge");
            return;
        }
        else 
        {
            current_config["e1"] = activePiece;
        }
        root.Q<Button>("e2").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

        private static void TileL0Clicked()
    {
        TileClickBehaviour("l0");
    }

    private static void TileL1Clicked()
    {
        TileClickBehaviour("l1");
    }

    private static void TileL2Clicked()
    {
        TileClickBehaviour("l2");
    }

    private static void TileL3Clicked()
    {
        TileClickBehaviour("l3");
    }

    private static void TileL4Clicked()
    {
        TileClickBehaviour("l4");
    }

    private static void TileL5Clicked()
    {
        TileClickBehaviour("l5");
    }

    private static void TileL6Clicked()
    {
        TileClickBehaviour("l6");
    }

    private static void TileL7Clicked()
    {
        TileClickBehaviour("l7");
    }

    private static void TileL8Clicked()
    {
        TileClickBehaviour("l8");
    }

    private static void TileL9Clicked()
    {
        TileClickBehaviour("l9");
    }

    private static void TileL10Clicked()
    {
        TileClickBehaviour("l10");
    }

    private static void TileL11Clicked()
    {
        TileClickBehaviour("l11");
    }

    private static void TileL12Clicked()
    {
        TileClickBehaviour("l12");
    }

    private static void TileL13Clicked()
    {
        TileClickBehaviour("l13");
    }

    private static void TileL14Clicked()
    {
        TileClickBehaviour("l14");
    }

    private static void TileL15Clicked()
    {
        TileClickBehaviour("l15");
    }

    private static void TileT0Clicked()
    {
        TileClickBehaviour("t0");
    }

    private static void TileT1Clicked()
    {
        TileClickBehaviour("t1");
    }

    private static void TileT2Clicked()
    {
        TileClickBehaviour("t2");
    }

    private static void TileT3Clicked()
    {
        TileClickBehaviour("t3");
    }

    private static void TileT4Clicked()
    {
        TileClickBehaviour("t4");
    }

    private static void TileT5Clicked()
    {
        TileClickBehaviour("t5");
    }

    private static void TileT6Clicked()
    {
        TileClickBehaviour("t6");
    }

    private static void TileT7Clicked()
    {
        TileClickBehaviour("t7");
    }

    private static void TileT8Clicked()
    {
        TileClickBehaviour("t8");
    }

    private static void TileT9Clicked()
    {
        TileClickBehaviour("t9");
    }

    private static void TileT10Clicked()
    {
        TileClickBehaviour("t10");
    }

    private static void TileT11Clicked()
    {
        TileClickBehaviour("t11");
    }

    private static void TileT12Clicked()
    {
        TileClickBehaviour("t12");
    }

    private static void TileT13Clicked()
    {
        TileClickBehaviour("t13");
    }

    private static void TileT14Clicked()
    {
        TileClickBehaviour("t14");
    }

    private static void TileT15Clicked()
    {
        TileClickBehaviour("t15");
    }

    private static void TileR0Clicked()
    {
        TileClickBehaviour("r0");
    }

    private static void TileR1Clicked()
    {
        TileClickBehaviour("r1");
    }

    private static void TileR2Clicked()
    {
        TileClickBehaviour("r2");
    }

    private static void TileR3Clicked()
    {
        TileClickBehaviour("r3");
    }

    private static void TileR4Clicked()
    {
        TileClickBehaviour("r4");
    }

    private static void TileR5Clicked()
    {
        TileClickBehaviour("r5");
    }

    private static void TileR6Clicked()
    {
        TileClickBehaviour("r6");
    }

    private static void TileR7Clicked()
    {
        TileClickBehaviour("r7");
    }

    private static void TileR8Clicked()
    {
        TileClickBehaviour("r8");
    }

    private static void TileR9Clicked()
    {
        TileClickBehaviour("r9");
    }

    private static void TileR10Clicked()
    {
        TileClickBehaviour("r10");
    }

    private static void TileR11Clicked()
    {
        TileClickBehaviour("r11");
    }

    private static void TileR12Clicked()
    {
        TileClickBehaviour("r12");
    }

    private static void TileR13Clicked()
    {
        TileClickBehaviour("r13");
    }

    private static void TileR14Clicked()
    {
        TileClickBehaviour("r14");
    }

    private static void TileR15Clicked()
    {
        TileClickBehaviour("r15");
    }

    private static void TileB0Clicked()
    {
        TileClickBehaviour("b0");
    }

    private static void TileB1Clicked()
    {
        TileClickBehaviour("b1");
    }

    private static void TileB2Clicked()
    {
        TileClickBehaviour("b2");
    }

    private static void TileB3Clicked()
    {
        TileClickBehaviour("b3");
    }

    private static void TileB4Clicked()
    {
        TileClickBehaviour("b4");
    }

    private static void TileB5Clicked()
    {
        TileClickBehaviour("b5");
    }

    private static void TileB6Clicked()
    {
        TileClickBehaviour("b6");
    }

    private static void TileB7Clicked()
    {
        TileClickBehaviour("b7");
    }

    private static void TileB8Clicked()
    {
        TileClickBehaviour("b8");
    }

    private static void TileB9Clicked()
    {
        TileClickBehaviour("b9");
    }

    private static void TileB10Clicked()
    {
        TileClickBehaviour("b10");
    }

    private static void TileB11Clicked()
    {
        TileClickBehaviour("b11");
    }

    private static void TileB12Clicked()
    {
        TileClickBehaviour("b12");
    }

    private static void TileB13Clicked()
    {
        TileClickBehaviour("b13");
    }

    private static void TileB14Clicked()
    {
        TileClickBehaviour("b14");
    }

    private static void TileB15Clicked()
    {
        TileClickBehaviour("b15");
    }
}
