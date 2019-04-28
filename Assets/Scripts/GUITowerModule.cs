using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles all tower-related UI stuff
/// </summary>
[System.Serializable]
public class GUITowerModule {

    public Transform towerUIContainer;
    public GameObject towerElementPrefab;
    public List<TowerUIElement> towerUIElements;

    public void Initialise () {
        // Create all UI grid elements
        var t = TowerDatabase.towers;
        towerUIElements = new List<TowerUIElement>();
        for (int i = 0; i < t.Count; i++) {
            TowerInfo info = t[i];
            GameObject ins = Object.Instantiate(towerElementPrefab, towerUIContainer);
            ins.SetActive(true);
            TowerUIElement e = ins.GetComponent<TowerUIElement>();
            e.Initialise();
            e.SetText($"{ info.cost } L");
            e.SetIcon(info.mySprite);
            e.myInfo = info;
            towerUIElements.Add(e);
        }

        UpdateHealth(GameManager.STARTING_HEALTH);
    }

    /// <summary>Change the availability of items that cannot be bought with current life.</summary>
    public void UpdateHealth (int newHealth) {
        for (int i = 0; i < towerUIElements.Count; i++) {
            if (newHealth < towerUIElements[i].myInfo.cost) {
                // Cannot buy
                towerUIElements[i].SetAvailable(false);
            }
        }
    }
}
