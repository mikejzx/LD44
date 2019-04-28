using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
    This is used for towers that are being bought
*/
public class TowerDisplay : MonoBehaviour, IDragHandler {
    [HideInInspector] public bool validPosition = true;

    private RectTransform trans;
    private Image img;

    public void Initialise () {
        trans = transform.parent.GetComponent<RectTransform>();
        img = GetComponent<Image>();
    }

    /// <summary>Allows dragging</summary>
    public void OnDrag (PointerEventData eventData) {
        trans.anchoredPosition += eventData.delta / 2.0f;
    }

    private void OnTriggerEnter2D (Collider2D collision) {
        img.color = new Color(1.0f, 0.2f, 0.2f, 0.3f);
        validPosition = false;
    }

    private void OnTriggerExit2D (Collider2D collision) {
        img.color = new Color(1.0f, 1.0f, 1.0f, 0.3f);
        validPosition = true;
    }
}
