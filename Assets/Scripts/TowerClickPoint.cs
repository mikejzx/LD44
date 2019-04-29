using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerClickPoint : MonoBehaviour, IPointerClickHandler {

    private ITower tower;

    private void Start () {
        tower = GetComponent<ITower>();
    }

    /// <summary>Allow selling</summary>
    void IPointerClickHandler.OnPointerClick (PointerEventData eventData) {
        GUITowerModule.instance.OwnedTowerClick(tower);
    }
}
