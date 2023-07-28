using UnityEngine.UI;
using UnityEngine;

public class CheckMeterController : MonoBehaviour
{
    private Quaternion startRotation;

    private void Awake() {
        startRotation = transform.rotation;

        GlobalEventManager.OnCheckShown += Show;
        GlobalEventManager.OnMoveMade += Hide;
    }

    private void Start() {
        Hide();
    }

    private void Hide()
    {
        gameObject.GetComponent<Text>().text = "";
    }

    private void Show()
    {
        gameObject.GetComponent<Text>().text = LanguageController.GetWord("HUD.Check");
    }

    public void KeepRotation()
    {
        transform.rotation = startRotation;
    }
}
