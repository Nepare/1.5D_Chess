using UnityEngine.UI;
using UnityEngine;

public class CheckMeterController : MonoBehaviour
{
    private Quaternion startRotation;

    private void Awake() {
        Hide();

        transform.Rotate(new Vector3(0, 0, 180));
        startRotation = transform.rotation;

        GlobalEventManager.OnCheckShown += Show;
        GlobalEventManager.OnMoveMade += Hide;
    }

    private void Hide()
    {
        gameObject.GetComponent<Text>().text = "";
    }

    private void Show()
    {
        gameObject.GetComponent<Text>().text = "CHECKED!";
    }

    public void KeepRotation()
    {
        transform.rotation = startRotation;
    }
}
