using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles all tower-related UI stuff
/// </summary>
[System.Serializable]
public class GUITowerModule {

    public Transform towerUIContainer;
    public GameObject towerElementPrefab;
    public List<TowerUIElement> towerUIElements;
    public CanvasGroup buyOptions;
    [Space]
    public Button btnConfirmPurchase;
    public Button btnCancelPurchase;

    private ITower selectedTowerForPurchase;
    private TowerDisplay selectedTd;
    private bool healthFlag = false;

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
            e.btn.onClick.AddListener(delegate { TowerClicked(info.myPrefab); });
            towerUIElements.Add(e);
        }

        UpdateHealth(GameManager.STARTING_HEALTH);

        btnConfirmPurchase.onClick.AddListener(TowerPurchaseConfirm);
        btnCancelPurchase.onClick.AddListener(TowerPurchaseCancel);
    }

    /// <summary>Instantiates the clicked tower into the game somewhere.</summary>
    private void TowerClicked (GameObject prefab) {
        // Return if already purchasing to avoid nasty bugs :P
        if (selectedTowerForPurchase != null) { return; }

        AudioHandler.ClickSound();

        GameObject ins = Object.Instantiate(prefab, TowerManager.towersContainer);
        ins.GetComponent<RectTransform>().anchoredPosition = new Vector2(GameManager.CANVAS_WIDTH / 2.0f, GameManager.CANVAS_HEIGHT / -2.0f);
        ins.SetActive(true);
        selectedTowerForPurchase = ins.GetComponent<ITower>();
        selectedTd = selectedTowerForPurchase.GetRangeCircle().AddComponent<TowerDisplay>(); // To allow dragging.
        selectedTd.Initialise();
        selectedTowerForPurchase.ShowRangeCircle(true);
        selectedTowerForPurchase.SetEnabled(false);

        // Show the buy options
        buyOptions.alpha = 1.0f;
        buyOptions.gameObject.SetActive(true);
    }

    private void TowerPurchaseConfirm() {
        // Return if it's on the path.
        if (!selectedTd.validPosition) {
            return;
        }

        AudioHandler.ClickSound();

        //Debug.Log(selectedTowerForPurchase.GetGameobject().name);

        selectedTowerForPurchase.ShowRangeCircle(false);
        selectedTowerForPurchase.SetEnabled(true);

        // Remove the cost from the health (needs to be down here)
        healthFlag = true;
        Player.health -= selectedTowerForPurchase.MyInfo().cost;
        healthFlag = false;

        HideBuyOptions();
    }

    private void TowerPurchaseCancel() {
        AudioHandler.ClickSound();
        GameObject.Destroy(selectedTowerForPurchase.GetGameobject());
        HideBuyOptions();
    }

    private void HideBuyOptions () {
        selectedTowerForPurchase = null;
        selectedTd = null;

        buyOptions.alpha = 0.0f;
        buyOptions.gameObject.SetActive(false);
    }

    /// <summary>Change the availability of items that cannot be bought with current life.</summary>
    public void UpdateHealth (int newHealth) {
        for (int i = 0; i < towerUIElements.Count; i++) {
            if (newHealth <= towerUIElements[i].myInfo.cost) {
                // Cannot buy
                towerUIElements[i].SetAvailable(false);
            }
            else {
                towerUIElements[i].SetAvailable(true);
            }
        }

        // Cancel purchase of tower if it is being purchased and goes under the cost
        if (!healthFlag && selectedTowerForPurchase != null
            && newHealth <= selectedTowerForPurchase.MyInfo().cost) {
                TowerPurchaseCancel();
        }
        if (healthFlag) { healthFlag = false; }
    }
}
