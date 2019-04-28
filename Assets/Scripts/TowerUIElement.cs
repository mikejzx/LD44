using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>A display of a tower UI element</summary>
public class TowerUIElement : MonoBehaviour {
    public TowerInfo myInfo;
    public Text costText;
    public Image iconImg;

    public bool available = false;

    private Button btn;

    public void Initialise () => btn = GetComponent<Button>();

    public void SetText (string s) => costText.text = s;

    public void SetIcon (Sprite s) => iconImg.sprite = s;

    public void SetAvailable (bool av) {
        available = av;
        btn.interactable = av;
        costText.color = av ? GameManager.COLOUR_TOWER_AVAILABLE : GameManager.COLOUR_TOWER_UNAVAILABLE;
    }
}
