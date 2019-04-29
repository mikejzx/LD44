using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerBaseShooter : MonoBehaviour, ITower {

    public RectTransform arms, imagePivot;
    public Vector3 initialArmPos = Vector3.zero;
    public Vector3 shootArmPos = Vector3.zero;
    public AudioClip shootSound;
    public float soundVolume = 1.0f;
    public RectTransform rangeCircle;

    public int towerIndex = -1; // Jesus christ im in a rush sry

    private Coroutine shootCr = null;
    private float timer = 0.0f;
    private bool hasTarget = false;
    private RectTransform trans;

    private bool working = true;
    private TowerInfo info;

    protected virtual void Start () {
        initialArmPos = arms.localPosition;
        trans = GetComponent<RectTransform>();
    }

    protected virtual void Update () {
        if (!working) { return; } // If the tower is not enabled (if it's being purchased)
        SearchForTargetsAndShoot();
    }

    private void SearchForTargetsAndShoot () {
        // Increment while it's smaller than the shoot delay
        if (timer < GetShootDelay()) { timer += Time.deltaTime; }

        // Check if any targets are in range
        var enemies = Spawner.instance.enemies;
        bool[] inRange = new bool[enemies.Count];
        int targetIdx = -1;
        for (int i = 0; i < enemies.Count; i++) {
            if (enemies[i].onTrack && Vector3.Distance(enemies[i].trans.anchoredPosition, trans.anchoredPosition) < GetRange()) {
                inRange[i] = true;
                hasTarget = true;
                targetIdx = i; // Will always be last target.
                break;
            }
        }

        // Return if there's no target
        if (!hasTarget || targetIdx == -1) {
            return;
        }

        // Look at the target.
        Enemy target = enemies[targetIdx];
        Vector3 relPos = target.transform.position - imagePivot.position;
        imagePivot.rotation = Quaternion.LookRotation(relPos, Vector3.forward);

        // Shoot
        if (timer >= GetShootDelay()) {
            timer = 0.0f;
            Shoot(target);
        }
    }

    // Implementation for ITower
    public void ShowRangeCircle (bool show) {
        float w = GetRange();
        var v = new Vector2(w * 2.0f, w * 2.0f);
        rangeCircle.sizeDelta = v;
        rangeCircle.gameObject.SetActive(show);
    }

    public TowerInfo MyInfo () => TowerDatabase.towers[towerIndex];
    public void SetTowerInfo (TowerInfo t) => info = t;
    public GameObject GetGameobject () => gameObject;
    public GameObject GetRangeCircle () => rangeCircle.gameObject;
    public void SetEnabled (bool enable) => working = enable;
    public void LockMovement () => rangeCircle.GetComponent<TowerDisplay>().LockMovement();
    public int SellPrice () => Mathf.RoundToInt(MyInfo().cost * (5.0f / 8.0f));

    protected abstract float GetRange ();
    protected abstract float GetShootDelay ();
    protected abstract int GetDamage ();

    protected virtual void Shoot (Enemy target) {
        // Handle actually firing here...
        target.Damage(GetDamage());
        if (shootSound != null) {
            AudioHandler.Play2DSound(shootSound, soundVolume);
        }

        // This is for the VFX suchas recoil kickback etc.
        if (shootCr != null) { StopCoroutine(shootCr); }
        shootCr = StartCoroutine(ShootCr());
    }

    /// <summary>Shoot the weapon</summary>
    private IEnumerator ShootCr () {
        float t = 0.0f, s = 2.0f;
        // Instantly send the arms to the shoot pos.
        arms.localPosition = shootArmPos;
        while (t < 1.0f) {
            t = Mathf.Clamp01(t + Time.deltaTime * s);
            arms.localPosition = Vector3.Lerp(shootArmPos, initialArmPos, 1.0f - ((1.0f - t) * (1.0f - t)));
            yield return null;
        }
        arms.localPosition = initialArmPos;
    }
}
