using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] public string[,] board;

    private void Awake() {
        board = new string[4, 16];
    }
}
