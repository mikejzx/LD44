using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public static Trackpath currentPath { get { return instance._currentPath; } }
    public static Player player;

    // Random sprites
    public static Sprite sprite_enemyBasicA { get { return instance._sprite_enemyBasicA; } }
    public static Sprite sprite_enemyBasicB { get { return instance._sprite_enemyBasicB; } }
    public static Sprite sprite_enemyBasicC { get { return instance._sprite_enemyBasicC; } }
    public static Sprite sprite_enemyTough { get { return instance._sprite_enemyTough; } }
    public static Sprite sprite_enemyTougher { get { return instance._sprite_enemyTougher; } }
    public static Sprite sprite_enemyInsane { get { return instance._sprite_enemyInsane; } }

    [SerializeField] private Trackpath _currentPath;
    [SerializeField] private Camera _mainCam;
    [SerializeField] private GUIManager gui;
    [SerializeField] private Spawner spawner;
    [SerializeField] private TowerManager towers;

    [SerializeField] private Sprite _sprite_enemyBasicA;
    [SerializeField] private Sprite _sprite_enemyBasicB;
    [SerializeField] private Sprite _sprite_enemyBasicC;
    [SerializeField] private Sprite _sprite_enemyTough;
    [SerializeField] private Sprite _sprite_enemyTougher;
    [SerializeField] private Sprite _sprite_enemyInsane;

    // Constansts
    public static readonly int ENEMYSTRENGTH_BASIC = 1,
        ENEMYSTRENGTH_TOUGH = 3,
        ENEMYSTRENGTH_TOUGHER = 5,
        ENEMYSTRENGTH_INSANE = 7;
    public static readonly int ENEMYSPEED_BASIC = 20,
        ENEMYSPEED_TOUGH = 15,
        ENEMYSPEED_TOUGHER = 10,
        ENEMYSPEED_INSANE = 30;
    public static readonly int ENEMYDAMAGE_BASIC = 10,
        ENEMYDAMAGE_TOUGH = 15,
        ENEMYDAMAGE_TOUGHER = 20,
        ENEMYDAMAGE_INSANE = 30;

    private static readonly int STARTING_HEALTH = 150;

    private void Awake () => instance = this;

    private void Start () {
        gui.Initialise();
        player = GetComponent<Player>();
        player.health = STARTING_HEALTH;

        ForceWidescreenRatio();
    }

    private void Update () {
        spawner.Update();
    }

    private void ForceWidescreenRatio () {
        float targetAspect = 16.0f / 9.0f;
        float windowAspect = Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        if (scaleHeight < 1.0f) {
            _mainCam.rect = new Rect(0.0f, (1.0f - scaleHeight) / 2.0f, 1.0f, scaleHeight);
            return;
        }
        float scaleWidth = 1.0f / scaleHeight;
        _mainCam.rect = new Rect((1.0f - scaleWidth) / 2.0f, 0.0f, scaleWidth, 1.0f);
    }
}
