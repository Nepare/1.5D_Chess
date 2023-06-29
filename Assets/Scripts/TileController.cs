using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public int X, Y;
    public bool isAccessible, isCurrent, isEnemy, isAltUsed;
    private GameObject accessibilityTexture, currentTexture, enemyTexture;
    [SerializeField] public Material accessibilityMaterial_1, currentMaterial_1, enemyMaterial_1, accessibilityMaterial_2, currentMaterial_2, enemyMaterial_2;

    private void Awake() {
        GlobalEventManager.OnUseAltMaterialsForHints += SwitchToAltMaterial;
        GlobalEventManager.OnUseNormalMaterialsForHints += SwitchToNormalMaterial;
        isAccessible = false;
        isCurrent = false;
        isEnemy = false;
        isAltUsed = false;
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

    public void SwitchToAltMaterial()
    {
        accessibilityTexture.GetComponent<Renderer>().material = accessibilityMaterial_2;
        currentTexture.GetComponent<Renderer>().material = currentMaterial_2;
        enemyTexture.GetComponent<Renderer>().material = enemyMaterial_2;
        isAltUsed = true;
    }

    public void SwitchToNormalMaterial()
    {
        accessibilityTexture.GetComponent<Renderer>().material = accessibilityMaterial_1;
        currentTexture.GetComponent<Renderer>().material = currentMaterial_1;
        enemyTexture.GetComponent<Renderer>().material = enemyMaterial_1;
        isAltUsed = false;
    }

    public void SwitchMaterial()
    {
        if (isAltUsed) SwitchToNormalMaterial();
        else SwitchToAltMaterial();
    }
}
