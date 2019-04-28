using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour {

    public AudioSource bgmSource;

    private void Start () {
        bgmSource = GetComponent<AudioSource>();
        bgmSource.Play();
        bgmSource.loop = true;
    }
}
