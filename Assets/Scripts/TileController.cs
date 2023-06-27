using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public int X, Y;
    public bool isAccessible, isCurrent, isEnemy;
    private GameObject accessibilityTexture, currentTexture, enemyTexture;

    private void Awake() {
        isAccessible = false;
        isCurrent = false;
        isEnemy = false;
        accessibilityTexture = transform.GetChild(1).gameObject;
        accessibilityTexture.SetActive(false);
        currentTexture = transform.GetChild(2).gameObject;
        currentTexture.SetActive(false);
        enemyTexture = transform.GetChild(3).gameObject;
        enemyTexture.SetActive(false);
    }

    public void EnableAccessible()
    {
        isAccessible = true;
        accessibilityTexture.SetActive(true);
    }

    public void DisableAccessible()
    {
        isAccessible = false;
        accessibilityTexture.SetActive(false);
    }

    public void EnableCurrent()
    {
        isCurrent = true;
        currentTexture.SetActive(true);
    }

    public void DisableCurrent()
    {
        isCurrent = false;
        currentTexture.SetActive(false);
    }

    public void EnableEnemy()
    {
        isEnemy = true;
        enemyTexture.SetActive(true);
    }

    public void DisableEnemy()
    {
        isEnemy = false;
        enemyTexture.SetActive(false);
    }
}
