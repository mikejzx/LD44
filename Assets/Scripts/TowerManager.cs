using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour {

    public static TowerManager instance;
	public static Transform towersContainer {
        get { return instance._towersContainer; }
    }
    [SerializeField] private Transform _towersContainer;

    private void Awake () => instance = this;
}
