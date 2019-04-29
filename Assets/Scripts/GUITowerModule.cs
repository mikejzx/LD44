using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles all tower-related UI stuff
/// </summary>
[System.Serializable]
public class GUITowerModule {

    public static GUITowerModule instance;

    public Transform towerUIContainer;
    public GameObject towerElementPrefab;
    public List<TowerUIElement> towerUIElements;
    public CanvasGroup buyOptions;
    [Space]
    public Button btnConfirmPurchase;
    public Button btnCancelPurchase;
    public Text label_selectedInfoHeader, label_selectedInfoContent;
    [Space]
    public Button btnSellCancel;
    public Button btnSellConfirm;
    public CanvasGroup sellOptions;
    public Text label_sellBtn;

    private ITower selectedTowerForPurchase;
    private TowerDisplay selectedTd;
    private TowerClickPoint selectedCp;
    private bool healthFlag = false;
    private ITower selectedTowerForSell;

    private bool bBuying = false;
    private bool bSelling = false;

    private static readonly string TEXT_SELLBTN = "Sell for $";

    public void Initialise () {
        instance = this;

        // Create all UI grid elements
        var t = TowerDatabase.towers;
        towerUIElements = new List<TowerUIElement>();
        for (int i = 0; i < t.Count; i++) {
            TowerInfo info = t[i];
            GameObject ins = Object.Instantiate(towerElementPrefab, towerUIContainer);
            ins.SetActive(true);
            TowerUIElement e = ins.GetComponent<TowerUIElement>();
            e.Initialise();
            e.SetText($"{info.cost:n0} L");
            e.SetIcon(info.mySprite);
            e.myInfo = info;
            e.btn.onClick.AddListener(delegate { TowerClicked(info.myPrefab); });
            towerUIElements.Add(e);
        }

        UpdateHealth(GameManager.STARTING_HEALTH);

        buyOptions.alpha = 0.0f;
        buyOptions.gameObject.SetActive(false);
        btnConfirmPurchase.onClick.AddListener(TowerPurchaseConfirm);
        btnCancelPurchase.onClick.AddListener(TowerPurchaseCancel);

        label_selectedInfoHeader.text = string.Empty;
        label_selectedInfoContent.text = string.Empty;

        sellOptions.alpha = 0.0f;
        sellOptions.gameObject.SetActive(false);
        btnSellConfirm.onClick.AddListener(TowerSellConfirm);
        btnSellCancel.onClick.AddListener(TowerSellCancel);
    }

    /// <summary>Instantiates the clicked tower into the game somewhere.</summary>
    private void TowerClicked (GameObject prefab) {
        // Return if already purchasing to avoid nasty bugs :P
        if (selectedTowerForPurchase != null || bSelling) { return; }

        AudioHandler.ClickSound();

        GameObject ins = Object.Instantiate(prefab, TowerManager.towersContainer);
        ins.GetComponent<RectTransform>().anchoredPosition = new Vector2(GameManager.CANVAS_WIDTH / 2.0f, GameManager.CANVAS_HEIGHT / -2.0f);
        ins.SetActive(true);
        selectedTowerForPurchase = ins.GetComponent<ITower>();
        selectedCp = ins.GetComponent<TowerClickPoint>();
        selectedCp.enabled = false; // Don't allow clicking for sell while purchasing.
        selectedTd = selectedTowerForPurchase.GetRangeCircle().AddComponent<TowerDisplay>(); // To allow dragging.
        selectedTd.Initialise();
        selectedTowerForPurchase.ShowRangeCircle(true);
        selectedTowerForPurchase.SetEnabled(false);

        // Show the buy options
        buyOptions.alpha = 1.0f;
        buyOptions.gameObject.SetActive(true);

        label_selectedInfoHeader.text = selectedTowerForPurchase.MyInfo().towerName;
        label_selectedInfoContent.text = selectedTowerForPurchase.MyInfo().towerDescription;

        bBuying = true;
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
        selectedTowerForPurchase.LockMovement();

        selectedCp.enabled = true;

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

        label_selectedInfoHeader.text = string.Empty;
        label_selectedInfoContent.text = string.Empty;

        buyOptions.alpha = 0.0f;
        buyOptions.gameObject.SetActive(false);

        bBuying = false;
    }

    /// <summary>Shows the sell options for the selected tower.</summary>
    public void OwnedTowerClick(ITower click) {
        if (bBuying) { return; }

        selectedTowerForSell = click;
        selectedTowerForSell.ShowRangeCircle(true);
        label_sellBtn.text = string.Format("{0}{1:n0}", TEXT_SELLBTN, selectedTowerForSell.SellPrice());

        sellOptions.alpha = 1.0f;
        sellOptions.gameObject.SetActive(true);

        bSelling = true;
    }

    private void TowerSellConfirm () {
        AudioHandler.ClickSound();

        // Sell the selected tower.
        Player.health += selectedTowerForSell.SellPrice();
        GameObject.Destroy(selectedTowerForSell.GetGameobject());

        HideSellOptions();
    }

    private void TowerSellCancel () {
        AudioHandler.ClickSound();
        HideSellOptions();
    }

    private void HideSellOptions () {
        selectedTowerForSell.ShowRangeCircle(false);
        selectedTowerForSell = null;

        sellOptions.alpha = 0.0f;
        sellOptions.gameObject.SetActive(false);
        bSelling = false;
    }

    /// <summary>Change the availability of items that cannot be bought with current life.</summary>
    public void UpdateHealth (int newHealth) {
        for (int i = 0; i < towerUIElements.Count; i++) {
            // Only allow 1 minigun in the game.
            TowerInfo inf = towerUIElements[i].myInfo;
            if (inf.identifier == TowerDatabase.TOWER_MINIGUN_ID) {
                if (TowerShMinigun.instances > 0) {
                    towerUIElements[i].SetAvailable(false);
                    towerUIElements[i].SetText("Unavailable");
                    continue;
                }
                else {
                    towerUIElements[i].SetText($"{inf.cost:n0} L");
                }
            }

            // Normal towers.
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
