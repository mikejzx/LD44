using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

[System.Serializable]
public class GUIManager {
    public static GUIManager instance;
    public Text label_health;
    public Text label_wave;
    public Image img_healthfillbar;

    public void Initialise () => instance = this;

    public static void SetHealth(int newhealth) {
        instance.label_health.text = "Life: " + newhealth;
        instance.img_healthfillbar.fillAmount = Mathf.Clamp01(newhealth / 300.0f);
    }

    public static void SetWave(int newwave) => instance.label_wave.text = "Wave: " + newwave;
}
