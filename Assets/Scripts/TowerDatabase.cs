using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TowerDatabase {

    public static List<TowerInfo> towers = new List<TowerInfo>() {
        new TowerInfo("tower_pistol", "Pistol Tower", 50, "Shoots a low-power pistol in a short range, at short intervals."),
        new TowerInfo("tower_sniper", "Sniper Tower", 200, "A high-powered sniper rifle that is capable of obliterating anything within an enormous range!"),
        // TODO: More towers !
    };
}

public class TowerInfo {
    public string identifier = "null";
    public string towerName = "uninitialised";
    public string towerDescription = "null";
    public int cost = 50;
    public Sprite mySprite;
    public GameObject myPrefab;

    public TowerInfo (string Identifier, string TowerName, int Cost, string TowerDescription) {
        this.identifier = Identifier;
        this.towerName = TowerName;
        this.cost = Cost;
        this.towerDescription = TowerDescription;

        try {
            myPrefab = Resources.Load<GameObject>($"prefabs/{ identifier }");
            myPrefab.GetComponent<ITower>().SetTowerInfo(this);
            mySprite = Resources.Load<Sprite>($"tower_icos/ico_{ identifier }");
        }
        catch (System.Exception e) {
            Debug.LogError("Error loading tower resources! :: " + e.StackTrace);
            NullifyImage();
        }

        // Check if it loaded or not.
        if (mySprite == null) {
            NullifyImage();
        }
    }

    private void NullifyImage () {
        Debug.LogWarning("Loading image resource failed. Was null...");
        mySprite = GUIManager.missingTexture;
    }
}
