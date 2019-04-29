using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour {

    public static AudioHandler instance;

    public AudioSource bgmSource;
    public AudioClip waveCompleteSound;
    public AudioClip waveBeginSound;
    public AudioClip clickSound;

    private static readonly float clickVol = 1.0f;

    private void Awake () => instance = this;

    private void Start () {
        bgmSource = GetComponent<AudioSource>();
        bgmSource.Play();
        bgmSource.loop = true;
    }

    public static void PlayWaveFinishSound () {
        instance.bgmSource.PlayOneShot(instance.waveCompleteSound, GameManager.VOLUME_WAVECOMPLETE);
    }

    public static void PlayWaveStartSound () {
        instance.bgmSource.PlayOneShot(instance.waveBeginSound, GameManager.VOLUME_WAVEBEGIN);
    }

    public static void Play2DSound(AudioClip clip, float vol) {
        instance.bgmSource.PlayOneShot(clip, vol);
    }

    public static void ClickSound() {
        instance.bgmSource.PlayOneShot(instance.clickSound, clickVol);
    }
}
