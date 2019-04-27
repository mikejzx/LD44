using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public static Trackpath currentPath { get { return instance._currentPath; } }

    [SerializeField] private Trackpath _currentPath;

    private void Awake () => instance = this;
}
