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
        if (activePiece == "")
        {
            if (current_config.ContainsKey("l0")) current_config.Remove("l0");
            if (wkPos == "l0") wkPos = "";
            if (bkPos == "l0") bkPos = "";
        }
        else current_config["l0"] = activePiece;
        if (activePiece == "wk" && wkPos != "l0")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "l0";
        }
        if (activePiece == "bk" && bkPos != "l0")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "l0";
        }
        root.Q<Button>("l1").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileL1Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("l1")) current_config.Remove("l1");
            if (wkPos == "l1") wkPos = "";
            if (bkPos == "l1") bkPos = "";
        }
        else current_config["l1"] = activePiece;
        if (activePiece == "wk" && wkPos != "l1")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "l1";
        }
        if (activePiece == "bk" && bkPos != "l1")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "l1";
        }
        root.Q<Button>("l2").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileL2Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("l2")) current_config.Remove("l2");
            if (wkPos == "l2") wkPos = "";
            if (bkPos == "l2") bkPos = "";
        }
        else current_config["l2"] = activePiece;
        if (activePiece == "wk" && wkPos != "l2")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "l2";
        }
        if (activePiece == "bk" && bkPos != "l2")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "l2";
        }
        root.Q<Button>("l3").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileL3Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("l3")) current_config.Remove("l3");
            if (wkPos == "l3") wkPos = "";
            if (bkPos == "l3") bkPos = "";
        }
        else current_config["l3"] = activePiece;
        if (activePiece == "wk" && wkPos != "l3")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "l3";
        }
        if (activePiece == "bk" && bkPos != "l3")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "l3";
        }
        root.Q<Button>("l4").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileL4Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("l4")) current_config.Remove("l4");
            if (wkPos == "l4") wkPos = "";
            if (bkPos == "l4") bkPos = "";
        }
        else current_config["l4"] = activePiece;
        if (activePiece == "wk" && wkPos != "l4")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "l4";
        }
        if (activePiece == "bk" && bkPos != "l4")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "l4";
        }
        root.Q<Button>("l5").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileL5Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("l5")) current_config.Remove("l5");
            if (wkPos == "l5") wkPos = "";
            if (bkPos == "l5") bkPos = "";
        }
        else current_config["l5"] = activePiece;
        if (activePiece == "wk" && wkPos != "l5")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "l5";
        }
        if (activePiece == "bk" && bkPos != "l5")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "l5";
        }
        root.Q<Button>("l6").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileL6Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("l6")) current_config.Remove("l6");
            if (wkPos == "l6") wkPos = "";
            if (bkPos == "l6") bkPos = "";
        }
        else current_config["l6"] = activePiece;
        if (activePiece == "wk" && wkPos != "l6")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "l6";
        }
        if (activePiece == "bk" && bkPos != "l6")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "l6";
        }
        root.Q<Button>("l7").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileL7Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("l7")) current_config.Remove("l7");
            if (wkPos == "l7") wkPos = "";
            if (bkPos == "l7") bkPos = "";
        }
        else current_config["l7"] = activePiece;
        if (activePiece == "wk" && wkPos != "l7")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "l7";
        }
        if (activePiece == "bk" && bkPos != "l7")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "l7";
        }
        root.Q<Button>("l8").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileL8Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("l8")) current_config.Remove("l8");
            if (wkPos == "l8") wkPos = "";
            if (bkPos == "l8") bkPos = "";
        }
        else current_config["l8"] = activePiece;
        if (activePiece == "wk" && wkPos != "l8")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "l8";
        }
        if (activePiece == "bk" && bkPos != "l8")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "l8";
        }
        root.Q<Button>("l9").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileL9Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("l9")) current_config.Remove("l9");
            if (wkPos == "l9") wkPos = "";
            if (bkPos == "l9") bkPos = "";
        }
        else current_config["l9"] = activePiece;
        if (activePiece == "wk" && wkPos != "l9")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "l9";
        }
        if (activePiece == "bk" && bkPos != "l9")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "l9";
        }
        root.Q<Button>("l10").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileL10Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("l10")) current_config.Remove("l10");
            if (wkPos == "l10") wkPos = "";
            if (bkPos == "l10") bkPos = "";
        }
        else current_config["l10"] = activePiece;
        if (activePiece == "wk" && wkPos != "l10")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "l10";
        }
        if (activePiece == "bk" && bkPos != "l10")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "l10";
        }
        root.Q<Button>("l11").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileL11Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("l11")) current_config.Remove("l11");
            if (wkPos == "l11") wkPos = "";
            if (bkPos == "l11") bkPos = "";
        }
        else current_config["l11"] = activePiece;
        if (activePiece == "wk" && wkPos != "l11")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "l11";
        }
        if (activePiece == "bk" && bkPos != "l11")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "l11";
        }
        root.Q<Button>("l12").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileL12Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("l12")) current_config.Remove("l12");
            if (wkPos == "l12") wkPos = "";
            if (bkPos == "l12") bkPos = "";
        }
        else current_config["l12"] = activePiece;
        if (activePiece == "wk" && wkPos != "l12")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "l12";
        }
        if (activePiece == "bk" && bkPos != "l12")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "l12";
        }
        root.Q<Button>("l13").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileL13Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("l13")) current_config.Remove("l13");
            if (wkPos == "l13") wkPos = "";
            if (bkPos == "l13") bkPos = "";
        }
        else current_config["l13"] = activePiece;
        if (activePiece == "wk" && wkPos != "l13")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "l13";
        }
        if (activePiece == "bk" && bkPos != "l13")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "l13";
        }
        root.Q<Button>("l14").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileL14Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("l14")) current_config.Remove("l14");
            if (wkPos == "l14") wkPos = "";
            if (bkPos == "l14") bkPos = "";
        }
        else current_config["l14"] = activePiece;
        if (activePiece == "wk" && wkPos != "l14")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "l14";
        }
        if (activePiece == "bk" && bkPos != "l14")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "l14";
        }
        root.Q<Button>("l15").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileL15Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("l15")) current_config.Remove("l15");
            if (wkPos == "l15") wkPos = "";
            if (bkPos == "l15") bkPos = "";
        }
        else current_config["l15"] = activePiece;
        if (activePiece == "wk" && wkPos != "l15")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "l15";
        }
        if (activePiece == "bk" && bkPos != "l15")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "l15";
        }
        root.Q<Button>("l16").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileT0Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("t0")) current_config.Remove("t0");
            if (wkPos == "t0") wkPos = "";
            if (bkPos == "t0") bkPos = "";
        }
        else current_config["t0"] = activePiece;
        if (activePiece == "wk" && wkPos != "t0")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "t0";
        }
        if (activePiece == "bk" && bkPos != "t0")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "t0";
        }
        root.Q<Button>("t1").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileT1Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("t1")) current_config.Remove("t1");
            if (wkPos == "t1") wkPos = "";
            if (bkPos == "t1") bkPos = "";
        }
        else current_config["t1"] = activePiece;
        if (activePiece == "wk" && wkPos != "t1")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "t1";
        }
        if (activePiece == "bk" && bkPos != "t1")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "t1";
        }
        root.Q<Button>("t2").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileT2Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("t2")) current_config.Remove("t2");
            if (wkPos == "t2") wkPos = "";
            if (bkPos == "t2") bkPos = "";
        }
        else current_config["t2"] = activePiece;
        if (activePiece == "wk" && wkPos != "t2")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "t2";
        }
        if (activePiece == "bk" && bkPos != "t2")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "t2";
        }
        root.Q<Button>("t3").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileT3Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("t3")) current_config.Remove("t3");
            if (wkPos == "t3") wkPos = "";
            if (bkPos == "t3") bkPos = "";
        }
        else current_config["t3"] = activePiece;
        if (activePiece == "wk" && wkPos != "t3")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "t3";
        }
        if (activePiece == "bk" && bkPos != "t3")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "t3";
        }
        root.Q<Button>("t4").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileT4Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("t4")) current_config.Remove("t4");
            if (wkPos == "t4") wkPos = "";
            if (bkPos == "t4") bkPos = "";
        }
        else current_config["t4"] = activePiece;
        if (activePiece == "wk" && wkPos != "t4")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "t4";
        }
        if (activePiece == "bk" && bkPos != "t4")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "t4";
        }
        root.Q<Button>("t5").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileT5Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("t5")) current_config.Remove("t5");
            if (wkPos == "t5") wkPos = "";
            if (bkPos == "t5") bkPos = "";
        }
        else current_config["t5"] = activePiece;
        if (activePiece == "wk" && wkPos != "t5")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "t5";
        }
        if (activePiece == "bk" && bkPos != "t5")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "t5";
        }
        root.Q<Button>("t6").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileT6Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("t6")) current_config.Remove("t6");
            if (wkPos == "t6") wkPos = "";
            if (bkPos == "t6") bkPos = "";
        }
        else current_config["t6"] = activePiece;
        if (activePiece == "wk" && wkPos != "t6")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "t6";
        }
        if (activePiece == "bk" && bkPos != "t6")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "t6";
        }
        root.Q<Button>("t7").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileT7Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("t7")) current_config.Remove("t7");
            if (wkPos == "t7") wkPos = "";
            if (bkPos == "t7") bkPos = "";
        }
        else current_config["t7"] = activePiece;
        if (activePiece == "wk" && wkPos != "t7")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "t7";
        }
        if (activePiece == "bk" && bkPos != "t7")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "t7";
        }
        root.Q<Button>("t8").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileT8Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("t8")) current_config.Remove("t8");
            if (wkPos == "t8") wkPos = "";
            if (bkPos == "t8") bkPos = "";
        }
        else current_config["t8"] = activePiece;
        if (activePiece == "wk" && wkPos != "t8")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "t8";
        }
        if (activePiece == "bk" && bkPos != "t8")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "t8";
        }
        root.Q<Button>("t9").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileT9Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("t9")) current_config.Remove("t9");
            if (wkPos == "t9") wkPos = "";
            if (bkPos == "t9") bkPos = "";
        }
        else current_config["t9"] = activePiece;
        if (activePiece == "wk" && wkPos != "t9")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "t9";
        }
        if (activePiece == "bk" && bkPos != "t9")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "t9";
        }
        root.Q<Button>("t10").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileT10Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("t10")) current_config.Remove("t10");
            if (wkPos == "t10") wkPos = "";
            if (bkPos == "t10") bkPos = "";
        }
        else current_config["t10"] = activePiece;
        if (activePiece == "wk" && wkPos != "t10")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "t10";
        }
        if (activePiece == "bk" && bkPos != "t10")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "t10";
        }
        root.Q<Button>("t11").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileT11Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("t11")) current_config.Remove("t11");
            if (wkPos == "t11") wkPos = "";
            if (bkPos == "t11") bkPos = "";
        }
        else current_config["t11"] = activePiece;
        if (activePiece == "wk" && wkPos != "t11")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "t11";
        }
        if (activePiece == "bk" && bkPos != "t11")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "t11";
        }
        root.Q<Button>("t12").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileT12Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("t12")) current_config.Remove("t12");
            if (wkPos == "t12") wkPos = "";
            if (bkPos == "t12") bkPos = "";
        }
        else current_config["t12"] = activePiece;
        if (activePiece == "wk" && wkPos != "t12")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "t12";
        }
        if (activePiece == "bk" && bkPos != "t12")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "t12";
        }
        root.Q<Button>("t13").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileT13Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("t13")) current_config.Remove("t13");
            if (wkPos == "t13") wkPos = "";
            if (bkPos == "t13") bkPos = "";
        }
        else current_config["t13"] = activePiece;
        if (activePiece == "wk" && wkPos != "t13")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "t13";
        }
        if (activePiece == "bk" && bkPos != "t13")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "t13";
        }
        root.Q<Button>("t14").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileT14Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("t14")) current_config.Remove("t14");
            if (wkPos == "t14") wkPos = "";
            if (bkPos == "t14") bkPos = "";
        }
        else current_config["t14"] = activePiece;
        if (activePiece == "wk" && wkPos != "t14")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "t14";
        }
        if (activePiece == "bk" && bkPos != "t14")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "t14";
        }
        root.Q<Button>("t15").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileT15Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("t15")) current_config.Remove("t15");
            if (wkPos == "t15") wkPos = "";
            if (bkPos == "t15") bkPos = "";
        }
        else current_config["t15"] = activePiece;
        if (activePiece == "wk" && wkPos != "t15")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "t15";
        }
        if (activePiece == "bk" && bkPos != "t15")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "t15";
        }
        root.Q<Button>("t16").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileR0Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("r0")) current_config.Remove("r0");
            if (wkPos == "r0") wkPos = "";
            if (bkPos == "r0") bkPos = "";
        }
        else current_config["r0"] = activePiece;
        if (activePiece == "wk" && wkPos != "r0")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "r0";
        }
        if (activePiece == "bk" && bkPos != "r0")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "r0";
        }
        root.Q<Button>("r1").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileR1Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("r1")) current_config.Remove("r1");
            if (wkPos == "r1") wkPos = "";
            if (bkPos == "r1") bkPos = "";
        }
        else current_config["r1"] = activePiece;
        if (activePiece == "wk" && wkPos != "r1")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "r1";
        }
        if (activePiece == "bk" && bkPos != "r1")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "r1";
        }
        root.Q<Button>("r2").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileR2Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("r2")) current_config.Remove("r2");
            if (wkPos == "r2") wkPos = "";
            if (bkPos == "r2") bkPos = "";
        }
        else current_config["r2"] = activePiece;
        if (activePiece == "wk" && wkPos != "r2")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "r2";
        }
        if (activePiece == "bk" && bkPos != "r2")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "r2";
        }
        root.Q<Button>("r3").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileR3Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("r3")) current_config.Remove("r3");
            if (wkPos == "r3") wkPos = "";
            if (bkPos == "r3") bkPos = "";
        }
        else current_config["r3"] = activePiece;
        if (activePiece == "wk" && wkPos != "r3")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "r3";
        }
        if (activePiece == "bk" && bkPos != "r3")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "r3";
        }
        root.Q<Button>("r4").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileR4Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("r4")) current_config.Remove("r4");
            if (wkPos == "r4") wkPos = "";
            if (bkPos == "r4") bkPos = "";
        }
        else current_config["r4"] = activePiece;
        if (activePiece == "wk" && wkPos != "r4")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "r4";
        }
        if (activePiece == "bk" && bkPos != "r4")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "r4";
        }
        root.Q<Button>("r5").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileR5Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("r5")) current_config.Remove("r5");
            if (wkPos == "r5") wkPos = "";
            if (bkPos == "r5") bkPos = "";
        }
        else current_config["r5"] = activePiece;
        if (activePiece == "wk" && wkPos != "r5")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "r5";
        }
        if (activePiece == "bk" && bkPos != "r5")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "r5";
        }
        root.Q<Button>("r6").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileR6Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("r6")) current_config.Remove("r6");
            if (wkPos == "r6") wkPos = "";
            if (bkPos == "r6") bkPos = "";
        }
        else current_config["r6"] = activePiece;
        if (activePiece == "wk" && wkPos != "r6")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "r6";
        }
        if (activePiece == "bk" && bkPos != "r6")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "r6";
        }
        root.Q<Button>("r7").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileR7Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("r7")) current_config.Remove("r7");
            if (wkPos == "r7") wkPos = "";
            if (bkPos == "r7") bkPos = "";
        }
        else current_config["r7"] = activePiece;
        if (activePiece == "wk" && wkPos != "r7")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "r7";
        }
        if (activePiece == "bk" && bkPos != "r7")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "r7";
        }
        root.Q<Button>("r8").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileR8Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("r8")) current_config.Remove("r8");
            if (wkPos == "r8") wkPos = "";
            if (bkPos == "r8") bkPos = "";
        }
        else current_config["r8"] = activePiece;
        if (activePiece == "wk" && wkPos != "r8")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "r8";
        }
        if (activePiece == "bk" && bkPos != "r8")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "r8";
        }
        root.Q<Button>("r9").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileR9Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("r9")) current_config.Remove("r9");
            if (wkPos == "r9") wkPos = "";
            if (bkPos == "r9") bkPos = "";
        }
        else current_config["r9"] = activePiece;
        if (activePiece == "wk" && wkPos != "r9")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "r9";
        }
        if (activePiece == "bk" && bkPos != "r9")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "r9";
        }
        root.Q<Button>("r10").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileR10Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("r10")) current_config.Remove("r10");
            if (wkPos == "r10") wkPos = "";
            if (bkPos == "r10") bkPos = "";
        }
        else current_config["r10"] = activePiece;
        if (activePiece == "wk" && wkPos != "r10")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "r10";
        }
        if (activePiece == "bk" && bkPos != "r10")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "r10";
        }
        root.Q<Button>("r11").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileR11Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("r11")) current_config.Remove("r11");
            if (wkPos == "r11") wkPos = "";
            if (bkPos == "r11") bkPos = "";
        }
        else current_config["r11"] = activePiece;
        if (activePiece == "wk" && wkPos != "r11")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "r11";
        }
        if (activePiece == "bk" && bkPos != "r11")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "r11";
        }
        root.Q<Button>("r12").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileR12Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("r12")) current_config.Remove("r12");
            if (wkPos == "r12") wkPos = "";
            if (bkPos == "r12") bkPos = "";
        }
        else current_config["r12"] = activePiece;
        if (activePiece == "wk" && wkPos != "r12")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "r12";
        }
        if (activePiece == "bk" && bkPos != "r12")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "r12";
        }
        root.Q<Button>("r13").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileR13Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("r13")) current_config.Remove("r13");
            if (wkPos == "r13") wkPos = "";
            if (bkPos == "r13") bkPos = "";
        }
        else current_config["r13"] = activePiece;
        if (activePiece == "wk" && wkPos != "r13")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "r13";
        }
        if (activePiece == "bk" && bkPos != "r13")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "r13";
        }
        root.Q<Button>("r14").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileR14Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("r14")) current_config.Remove("r14");
            if (wkPos == "r14") wkPos = "";
            if (bkPos == "r14") bkPos = "";
        }
        else current_config["r14"] = activePiece;
        if (activePiece == "wk" && wkPos != "r14")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "r14";
        }
        if (activePiece == "bk" && bkPos != "r14")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "r14";
        }
        root.Q<Button>("r15").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileR15Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("r15")) current_config.Remove("r15");
            if (wkPos == "r15") wkPos = "";
            if (bkPos == "r15") bkPos = "";
        }
        else current_config["r15"] = activePiece;
        if (activePiece == "wk" && wkPos != "r15")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "r15";
        }
        if (activePiece == "bk" && bkPos != "r15")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "r15";
        }
        root.Q<Button>("r16").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileB0Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("b0")) current_config.Remove("b0");
            if (wkPos == "b0") wkPos = "";
            if (bkPos == "b0") bkPos = "";
        }
        else current_config["b0"] = activePiece;
        if (activePiece == "wk" && wkPos != "b0")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "b0";
        }
        if (activePiece == "bk" && bkPos != "b0")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "b0";
        }
        root.Q<Button>("b1").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileB1Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("b1")) current_config.Remove("b1");
            if (wkPos == "b1") wkPos = "";
            if (bkPos == "b1") bkPos = "";
        }
        else current_config["b1"] = activePiece;
        if (activePiece == "wk" && wkPos != "b1")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "b1";
        }
        if (activePiece == "bk" && bkPos != "b1")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "b1";
        }
        root.Q<Button>("b2").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileB2Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("b2")) current_config.Remove("b2");
            if (wkPos == "b2") wkPos = "";
            if (bkPos == "b2") bkPos = "";
        }
        else current_config["b2"] = activePiece;
        if (activePiece == "wk" && wkPos != "b2")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "b2";
        }
        if (activePiece == "bk" && bkPos != "b2")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "b2";
        }
        root.Q<Button>("b3").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileB3Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("b3")) current_config.Remove("b3");
            if (wkPos == "b3") wkPos = "";
            if (bkPos == "b3") bkPos = "";
        }
        else current_config["b3"] = activePiece;
        if (activePiece == "wk" && wkPos != "b3")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "b3";
        }
        if (activePiece == "bk" && bkPos != "b3")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "b3";
        }
        root.Q<Button>("b4").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileB4Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("b4")) current_config.Remove("b4");
            if (wkPos == "b4") wkPos = "";
            if (bkPos == "b4") bkPos = "";
        }
        else current_config["b4"] = activePiece;
        if (activePiece == "wk" && wkPos != "b4")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "b4";
        }
        if (activePiece == "bk" && bkPos != "b4")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "b4";
        }
        root.Q<Button>("b5").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileB5Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("b5")) current_config.Remove("b5");
            if (wkPos == "b5") wkPos = "";
            if (bkPos == "b5") bkPos = "";
        }
        else current_config["b5"] = activePiece;
        if (activePiece == "wk" && wkPos != "b5")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "b5";
        }
        if (activePiece == "bk" && bkPos != "b5")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "b5";
        }
        root.Q<Button>("b6").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileB6Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("b6")) current_config.Remove("b6");
            if (wkPos == "b6") wkPos = "";
            if (bkPos == "b6") bkPos = "";
        }
        else current_config["b6"] = activePiece;
        if (activePiece == "wk" && wkPos != "b6")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "b6";
        }
        if (activePiece == "bk" && bkPos != "b6")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "b6";
        }
        root.Q<Button>("b7").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileB7Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("b7")) current_config.Remove("b7");
            if (wkPos == "b7") wkPos = "";
            if (bkPos == "b7") bkPos = "";
        }
        else current_config["b7"] = activePiece;
        if (activePiece == "wk" && wkPos != "b7")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "b7";
        }
        if (activePiece == "bk" && bkPos != "b7")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "b7";
        }
        root.Q<Button>("b8").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileB8Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("b8")) current_config.Remove("b8");
            if (wkPos == "b8") wkPos = "";
            if (bkPos == "b8") bkPos = "";
        }
        else current_config["b8"] = activePiece;
        if (activePiece == "wk" && wkPos != "b8")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "b8";
        }
        if (activePiece == "bk" && bkPos != "b8")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "b8";
        }
        root.Q<Button>("b9").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileB9Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("b9")) current_config.Remove("b9");
            if (wkPos == "b9") wkPos = "";
            if (bkPos == "b9") bkPos = "";
        }
        else current_config["b9"] = activePiece;
        if (activePiece == "wk" && wkPos != "b9")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "b9";
        }
        if (activePiece == "bk" && bkPos != "b9")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "b9";
        }
        root.Q<Button>("b10").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileB10Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("b10")) current_config.Remove("b10");
            if (wkPos == "b10") wkPos = "";
            if (bkPos == "b10") bkPos = "";
        }
        else current_config["b10"] = activePiece;
        if (activePiece == "wk" && wkPos != "b10")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "b10";
        }
        if (activePiece == "bk" && bkPos != "b10")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "b10";
        }
        root.Q<Button>("b11").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileB11Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("b11")) current_config.Remove("b11");
            if (wkPos == "b11") wkPos = "";
            if (bkPos == "b11") bkPos = "";
        }
        else current_config["b11"] = activePiece;
        if (activePiece == "wk" && wkPos != "b11")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "b11";
        }
        if (activePiece == "bk" && bkPos != "b11")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "b11";
        }
        root.Q<Button>("b12").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileB12Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("b12")) current_config.Remove("b12");
            if (wkPos == "b12") wkPos = "";
            if (bkPos == "b12") bkPos = "";
        }
        else current_config["b12"] = activePiece;
        if (activePiece == "wk" && wkPos != "b12")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "b12";
        }
        if (activePiece == "bk" && bkPos != "b12")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "b12";
        }
        root.Q<Button>("b13").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileB13Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("b13")) current_config.Remove("b13");
            if (wkPos == "b13") wkPos = "";
            if (bkPos == "b13") bkPos = "";
        }
        else current_config["b13"] = activePiece;
        if (activePiece == "wk" && wkPos != "b13")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "b13";
        }
        if (activePiece == "bk" && bkPos != "b13")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "b13";
        }
        root.Q<Button>("b14").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileB14Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("b14")) current_config.Remove("b14");
            if (wkPos == "b14") wkPos = "";
            if (bkPos == "b14") bkPos = "";
        }
        else current_config["b14"] = activePiece;
        if (activePiece == "wk" && wkPos != "b14")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "b14";
        }
        if (activePiece == "bk" && bkPos != "b14")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "b14";
        }
        root.Q<Button>("b15").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }

    private static void TileB15Clicked()
    {
        if (activePiece == "")
        {
            if (current_config.ContainsKey("b15")) current_config.Remove("b15");
            if (wkPos == "b15") wkPos = "";
            if (bkPos == "b15") bkPos = "";
        }
        else current_config["b15"] = activePiece;
        if (activePiece == "wk" && wkPos != "b15")
        {
            if (wkPos != "")
            {
                current_config.Remove(wkPos);
                root.Q<Button>(wkPos[0] + (System.Convert.ToInt32(wkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            wkPos = "b15";
        }
        if (activePiece == "bk" && bkPos != "b15")
        {
            if (bkPos != "")
            {
                current_config.Remove(bkPos);
                root.Q<Button>(bkPos[0] + (System.Convert.ToInt32(bkPos.Substring(1)) + 1).ToString()).style.backgroundImage = null;
            }
            bkPos = "b15";
        }
        root.Q<Button>("b16").style.backgroundImage = UIBehaviour.GetTextureFromName(activePiece);
        activePiece = "";
        root.Q<Label>("lblWarnings").text = "";
        UIBehaviour.UnchooseAllPieces();
    }
}
