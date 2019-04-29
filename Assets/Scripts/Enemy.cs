using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    NOTE: This isn't actually being pooled...
        It was planned to be but is currently
        not since i need to work on other stuff lol.
*/

public enum EnemyType {
    BasicTypeA = 1, // Basic type A, B, and C are all the same, but look different.
    BasicTypeB = 2,
    BasicTypeC = 3,
    BasicShooter = 4,
    Tough = 5,
    ToughShooter = 6,
    Tougher = 7,
    Insane = 8,
};

public class Enemy : MonoBehaviour, IPoolable {
    public EnemyType type;

    public bool onTrack {
        get {
            if (trans != null) {
                return trans.anchoredPosition.y < 0.0f && trans.anchoredPosition.x > 0.0f;
            }
            return false;
        }
    }

    public RectTransform imagePivot;
    public Image baseSprite; // Base enemy sprite - changes based on type.

    private float _speed = 20.0f;
    private float speed {
        get { return _speed; }
        set {
            if (legs != null) {
                legs.SetSpeed(value);
            }
            _speed = value;
        }
    }
    private SpritesheetAnim legs;
    private int damageDealt = 10; // How much damage it does to player.
    private int strength = 1; // How difficult it is to destroy
    private int health = 0;

    public int currentPt = 0;
    private float prevSectorsDist = 0.0f;
    [HideInInspector] public RectTransform trans;
    [Range(0.0f, 1.0f)] private float completion = 0.0f; // How much of the track has been passed. 0.0f-1.0f
    [Range(0.0f, 1.0f)] private float completionCurrentPt = 0.0f; // How much of the *current point* has been passed.

    private bool visible = false;

    private static Trackpath path { get { return GameManager.currentPath; } }

    private void Start () {
        legs = GetComponent<SpritesheetAnim>();
        InitialiseEnemyType();

        trans = GetComponent<RectTransform>();
        foreach (Image child in transform.GetComponentsInChildren<Image>()) {
            RectTransform r = child.GetComponent<RectTransform>();
            Vector3 a = r.localEulerAngles;
            r.localEulerAngles = new Vector3(270.0f, 180.0f, 0.0f);
        }
        LookAtPoint();

        health = strength;
    }

    private void Update () {
        completion += Time.deltaTime * (speed / 1000.0f); // Will need to take the path's size into account....
        float travelledDist = path.totalDistance * completion;

        // Calculate completion of current sector.
        completionCurrentPt = Mathf.Clamp01((travelledDist - prevSectorsDist) / path[currentPt].dist);

        // Calculate whether current wapoint index should increment
        Vector2 pos = trans.position;
        if ((Mathf.Round(Vector3.Distance(pos, path[currentPt + 1].pos) * 1000.0f) / 1000.0f == 0.0f || completionCurrentPt >= 1.0f) && currentPt < path.size - 2) {
            prevSectorsDist += path[currentPt].dist;
            ++currentPt;
            completionCurrentPt = 0.0f;
            LookAtPoint();
        }


        // Interpolate between each position based on current
        // waypoint and completion.
        trans.anchoredPosition = Vector2.Lerp(path[currentPt].apos, path[currentPt + 1].apos, completionCurrentPt);

        // If the enemy gets past the end point, damage the player.
        if (completion >= 1.0f) {
            GameManager.player.Damage(damageDealt);
            Die();
        }
    }

    /// <summary>Damage the enemy by 1. Call Die() to destroy it instantly instead.</summary>
    public void Damage (int deal) {
        health -= deal;
        if (health <= 0) {
            // Add to player's kills
            ++Player.kills;

            Die();
        }
    }

    /// <summary>Enemy is destroyed</summary>
    private void Die () {
        Spawner.EnemyRemove(this);
        Destroy(gameObject);
    }

    /// <summary>Makes the enemy look at the next point</summary>
    private void LookAtPoint () {
        // Rotate to look at next point
        if (currentPt > path.size - 2) { return; }
        Vector3 relPos = path[currentPt + 1].pos - new Vector2(imagePivot.position.x, imagePivot.position.y);
        imagePivot.rotation = Quaternion.LookRotation(relPos, Vector3.forward);
    }

    private void InitialiseEnemyType () {
        //type = (EnemyType) Random.Range (1, ENEMYTYPE_COUNT + 1);
        switch (type) {
            case (EnemyType.BasicTypeA): {
                baseSprite.sprite = GameManager.sprite_enemyBasicA;
                strength = GameManager.ENEMYSTRENGTH_BASIC;
                speed = GameManager.ENEMYSPEED_BASIC;
                damageDealt = GameManager.ENEMYDAMAGE_BASIC;
            } break;

            case (EnemyType.BasicTypeB): {
                baseSprite.sprite = GameManager.sprite_enemyBasicB;
                strength = GameManager.ENEMYSTRENGTH_BASIC;
                speed = GameManager.ENEMYSPEED_BASIC;
                damageDealt = GameManager.ENEMYDAMAGE_BASIC;
            } break;

            case (EnemyType.BasicTypeC): {
                baseSprite.sprite = GameManager.sprite_enemyBasicC;
                strength = GameManager.ENEMYSTRENGTH_BASIC;
                speed = GameManager.ENEMYSPEED_BASIC;
                damageDealt = GameManager.ENEMYDAMAGE_BASIC;
            } break;

            // TODO: Shooters can shoot :P

            case (EnemyType.Tough): {
                baseSprite.sprite = GameManager.sprite_enemyTough;
                strength = GameManager.ENEMYSTRENGTH_TOUGH;
                speed = GameManager.ENEMYSPEED_TOUGH;
                damageDealt = GameManager.ENEMYDAMAGE_TOUGH;
            } break;

            case (EnemyType.Tougher): {
                baseSprite.sprite = GameManager.sprite_enemyTougher;
                strength = GameManager.ENEMYSTRENGTH_TOUGHER;
                speed = GameManager.ENEMYSPEED_TOUGHER;
                damageDealt = GameManager.ENEMYDAMAGE_TOUGHER;
            } break;

            case (EnemyType.Insane): {
                baseSprite.sprite = GameManager.sprite_enemyInsane;
                strength = GameManager.ENEMYSTRENGTH_INSANE;
                speed = GameManager.ENEMYSPEED_INSANE;
                damageDealt = GameManager.ENEMYDAMAGE_INSANE;
            } break;

            default: {
                baseSprite.sprite = GameManager.sprite_enemyBasicA;
                strength = GameManager.ENEMYSTRENGTH_BASIC;
                speed = GameManager.ENEMYSPEED_BASIC;
                damageDealt = GameManager.ENEMYDAMAGE_BASIC;
            } break;
        }
    }

    // UNUSED --- --
    public bool IsVisible () => visible;
    public void SetVisibility (bool visible) => this.visible = visible;
}
