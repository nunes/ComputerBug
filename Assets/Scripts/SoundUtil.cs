using System;
using System.Collections;

using UnityEngine;

public static class SoundUtil
{

    public static IEnumerator FadeOutSound(AudioSource audioSource, float fadeTimeOut)
    {

        if (audioSource == null)
        {
            yield break;
        }

        float currentTime = 0;

        float start = audioSource.volume;

        while (currentTime < fadeTimeOut)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, 0.1f, currentTime / fadeTimeOut);
            yield return null;
        }
        yield break;
    }

}
