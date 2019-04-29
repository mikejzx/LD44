using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public static Trackpath currentPath { get { return instance._currentPath; } }
    public static Camera mainCam { get { return instance._mainCam; } }
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
    public static readonly Color COLOUR_TOWER_AVAILABLE = new Color(0.0f, 1.0f, 0.2f, 1.0f);
    public static readonly Color COLOUR_TOWER_UNAVAILABLE = new Color(1.0f, 0.1f, 0.1f, 0.5f);
    public static readonly int ENEMYSTRENGTH_BASIC = 5,
        ENEMYSTRENGTH_TOUGH = 10,
        ENEMYSTRENGTH_TOUGHER = 15,
        ENEMYSTRENGTH_INSANE = 18;
    public static readonly int ENEMYSPEED_BASIC = 20,
        ENEMYSPEED_TOUGH = 15,
        ENEMYSPEED_TOUGHER = 10,
        ENEMYSPEED_INSANE = 30;
    public static readonly int ENEMYDAMAGE_BASIC = 10,
        ENEMYDAMAGE_TOUGH = 15,
        ENEMYDAMAGE_TOUGHER = 20,
        ENEMYDAMAGE_INSANE = 30;

    public static readonly float CANVAS_WIDTH = 800.0f, CANVAS_HEIGHT = 450.0f;
    public static readonly int WAVE_BONUS_LIFE_MUL = 15;
    public static readonly int STARTING_HEALTH = 200;
    public static readonly float VOLUME_WAVECOMPLETE = 0.7f, VOLUME_WAVEBEGIN = 0.9f;

    private void Awake () => instance = this;

    private void Start () {
        gui.Initialise(this);
        spawner.Initialise();
        player = GetComponent<Player>();
        Player.health = STARTING_HEALTH;
        TowerShMinigun.instances = 0; // For restarting the game.

        ForceWidescreenRatio();
    }

    private void Update () {
        spawner.Update();
        gui.Update();
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
