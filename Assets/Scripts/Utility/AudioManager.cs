using UnityEngine.Audio;
using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSound[] sounds;
    
    private void Awake() {

        foreach (AudioSound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    public void Play(string soundName, float startVolume, float highVolume, float endVolume, int fadeTimer, int timeToFadeOut)
    {
        AudioSound s = System.Array.Find(sounds, sound => sound.name == soundName);
        if (s != null)
        {
            s.source.Play();
            StartCoroutine(Fade(soundName, startVolume, highVolume, fadeTimer, 0));
            if (timeToFadeOut != 0)
                StartCoroutine(Fade(soundName, highVolume, endVolume, fadeTimer, timeToFadeOut));
        }
    }

    public void Stop(string soundName)
    {
        AudioSound s = System.Array.Find(sounds, sound => sound.name == soundName);
        if (s != null)
        {
            s.source.Stop();
        }
    }

    IEnumerator Fade(string soundName, float startVolume, float endVolume, int fadeTimer, float secondsToActivate)
    {
        yield return new WaitForSecondsRealtime(secondsToActivate);
        AudioSound s = System.Array.Find(sounds, sound => sound.name == soundName);
        int currentTimer = 0;
        if (s != null)
        {
            s.source.volume = startVolume;
            float incrementVolume = (endVolume - startVolume) / fadeTimer;
            while (currentTimer < fadeTimer)
            {
                s.source.volume += incrementVolume;

                currentTimer++;
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
