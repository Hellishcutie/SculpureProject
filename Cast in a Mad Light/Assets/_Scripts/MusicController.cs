using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource source1;
    public AudioSource source2;
    public float musicVolume = 1.0f;

    private AudioSource currentSource;
    private AudioSource otherSource;
    private Coroutine crossFadeCoroutine = null;

    void Start()
    {
        currentSource = source1;
        otherSource = source2;

        currentSource.volume = musicVolume;
        currentSource.Play();
    }

    public void CrossFadeTracks(AudioClip newClip, float duration)
    {
        if(crossFadeCoroutine == null)
        {
            otherSource.clip = newClip;
            crossFadeCoroutine = StartCoroutine(CoCrossFadeAudio(currentSource, otherSource, duration));
        }
    }

    private IEnumerator CoCrossFadeAudio(AudioSource outSource, AudioSource inSource, float duration)
    {
        Debug.Log("Crossfading!");
        float timer = 0f;
        otherSource.Play();
        while(timer <= duration)
        {
            timer += Time.deltaTime;
            float lerp = timer / duration;

            outSource.volume = Mathf.Lerp(musicVolume, 0, lerp);
            inSource.volume = Mathf.Lerp(0, musicVolume, lerp);

            yield return null;
        }
        var swap = currentSource;
        currentSource = otherSource;
        otherSource = swap;
        otherSource.Stop();

        crossFadeCoroutine = null;
    }
}
