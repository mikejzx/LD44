using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerBaseShooter : MonoBehaviour {

    public RectTransform arms, imagePivot;
    public Vector3 initialArmPos = Vector3.zero;
    public Vector3 shootArmPos = Vector3.zero;

    private Coroutine shootCr = null;
    private float timer = 0.0f;
    private bool hasTarget = false;


    protected virtual void Start () {
        initialArmPos = arms.localPosition;
    }

    protected virtual void Update () {
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
            if (Vector3.Distance(enemies[i].transform.position, transform.position) < GetRange()) {
                inRange[i] = true;
                hasTarget = true;
                targetIdx = i; // Will always be last target.
                break;
            }
        }

        // Return if there's no target
        if (!hasTarget || targetIdx == -1) {
            timer = GetShootDelay(); // So next time is instant to shoot.
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

    protected abstract float GetRange ();
    protected abstract float GetShootDelay ();

    protected void Shoot (Enemy target) {
        // Handle actually firing here...
        target.Damage();

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
