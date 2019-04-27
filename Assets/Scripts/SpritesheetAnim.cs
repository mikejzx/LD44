using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 Animates an image with spritesheet (e.g leg movement)
*/

public class SpritesheetAnim : MonoBehaviour {

    public Sprite[] frames;
    public Image image;
    public float frameSpeed = 20.0f;

    private int curFrame = 0;
    private int frameCount = 0;
    private float timer = 0.0f;

    private void Start () {
        frameCount = frames.Length;
    }

    private void Update () {
        timer += Time.deltaTime;
        if (timer > 1.0f / frameSpeed) {
            timer = 0.0f;
            ++curFrame;
            if (curFrame >= frameCount) {
                curFrame = 0;
            }
        }

        image.sprite = frames[curFrame];
    }

    public void SetSpeed(float speed) {
        frameSpeed = speed;
    }
}
