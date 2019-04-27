using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public static Trackpath currentPath { get { return instance._currentPath; } }
    public static Player player;

    [SerializeField] private Trackpath _currentPath;
    [SerializeField] private Camera _mainCam;
    [SerializeField] private GUIManager gui;
    [SerializeField] private Spawner spawner;

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
