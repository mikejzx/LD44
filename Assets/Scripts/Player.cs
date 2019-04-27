using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private int _health = 150;
    public int health {
        get { return _health; }
        set {
            GUIManager.SetHealth(value);
            _health = value;
        }
    }

    public void Damage (int damage) {
        health -= damage;
        CheckLoss();
    }

    /// <summary>Check if the player has lost all their life.</summary>
    private void CheckLoss () {
        
    }
}
