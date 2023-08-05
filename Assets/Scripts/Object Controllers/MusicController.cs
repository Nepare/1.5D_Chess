using System.Collections;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    private bool isMusicNormal;
    public GameObject checkFilter;
    public static bool checkChange = true;

    private void Awake() {
        isMusicNormal = true;
        checkFilter.SetActive(false);
        GlobalEventManager.OnCheckShown += ChangeMusicToCheck;
    }

    private void Start() {
        StartCoroutine(PlayMenuMusic(270f));
    }

    IEnumerator PlayMenuMusic(float repeatTimer)
    {
        while (true)
        {
            gameObject.GetComponent<AudioManager>().Play("defaultTheme");
            yield return new WaitForSecondsRealtime(repeatTimer);
        }
    }

    IEnumerator PlayCheckMusic(float repeatTimer)
    {
        while (true)
        {
            gameObject.GetComponent<AudioManager>().Play("checkTheme");
            yield return new WaitForSecondsRealtime(repeatTimer);
        }
    }

    private void ChangeMusicToCheck()
    {
        if (!checkChange) return;
        
        isMusicNormal = false;
        checkFilter.SetActive(true);
        StopCoroutine("PlayMenuMusic");
        gameObject.GetComponent<AudioManager>().Stop("defaultTheme");
        StartCoroutine(PlayCheckMusic(65));
    }

    public void ChangeMusicToNormal()
    {
        if (isMusicNormal) return;
        else isMusicNormal = true;

        checkFilter.SetActive(false);
        StopCoroutine("PlayCheckMusic");
        gameObject.GetComponent<AudioManager>().Stop("checkTheme");
        StartCoroutine(PlayMenuMusic(270f));
    }
}
