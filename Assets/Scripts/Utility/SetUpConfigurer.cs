using UnityEngine;

public class SetUpConfigurer : MonoBehaviour
{
    public static System.Collections.Generic.Dictionary<string, string> SETUP_CONFIGURATION = new System.Collections.Generic.Dictionary<string, string>();

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
}
