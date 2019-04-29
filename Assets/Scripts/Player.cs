using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public static bool dead = false;

    private static int _kills;
    public static int kills {
        get { return _kills; }
        set {
            ++health; // 1 health per kill
            _kills = value;
        }
    }
    private static int _health = 150;
    public static int health {
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

#if UNITY_EDITOR
    private void Update () {
        // For testing. Drop health below zero to test lose screen.
        if (Input.GetKeyDown(KeyCode.Z)) {
            Damage(1000);
        }

        if (Input.GetKeyDown(KeyCode.X)) {
            Damage(20);
        }

        if (Input.GetKeyDown(KeyCode.C)) {
            health += 200;
        }
    }
#endif

    /// <summary>Check if the player has lost all their life.</summary>
    private void CheckLoss () {
        if (health <= 0) {
            health = 0;
            dead = true;
            GUIManager.LoseScreen();
        }
    }
}
