using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keybinds : MonoBehaviour
{
    public static Dictionary<string, KeyCode> keybinds = DefaultKeybinds();

    public static Dictionary<string, KeyCode> DefaultKeybinds()
    {
        Dictionary<string, KeyCode> default_keys = new Dictionary<string, KeyCode>();

        default_keys["up"] = KeyCode.W;
        default_keys["left"] = KeyCode.A;
        default_keys["down"] = KeyCode.S;
        default_keys["right"] = KeyCode.D;
        default_keys["clockwise"] = KeyCode.E;
        default_keys["anticlockwise"] = KeyCode.Q;
        default_keys["toward"] = KeyCode.DownArrow;
        default_keys["backwards"] = KeyCode.UpArrow;
        default_keys["zoomout"] = KeyCode.Minus;
        default_keys["zoomin"] = KeyCode.Equals;

        default_keys["center"] = KeyCode.Space;
        default_keys["escape"] = KeyCode.Escape;
        default_keys["select"] = KeyCode.Mouse0;
        default_keys["spin"] = KeyCode.Mouse1;

        return default_keys;
    } 

    public static void ChangeKey(string keyString, KeyCode newKey)
    {
        keybinds[keyString] = newKey;
    }
}
