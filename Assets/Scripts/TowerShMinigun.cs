using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
   Minigun tower
*/

public class TowerShMinigun : TowerBaseShooter {

    public float range = 5.0f;
    public float firerate = 0.7f;
    public int damage = 1;

    // Minigun specific stuff
    public static int instances = 0;
    public SpritesheetAnim armsAnim; // The arms sprite changes on the minigun
    private float throttle = 0.0f;
    private float lastThrottleChange = 0.0f;
    private static readonly float throttleSpeed = 5.0f, throttleThreshold = 0.3f;

    private void Awake () {
        ++instances;
    }

    private void OnDestroy () {
        --instances;
        GUITowerModule.instance.UpdateHealth(Player.health); // Refresh cost label
    }

    protected override void Update () {
        base.Update();

        armsAnim.frameSpeed = 20 * throttle;

        // If time since last throttle change is greater than a speciif amount then decellerate.
        if (lastThrottleChange < throttleThreshold) {
            lastThrottleChange += Time.deltaTime;
        }
        else {
            // Greater/equal than 1.0, decrease it
            DecrThrottle();
        }
    }

    protected override void Shoot (Enemy target) {
        if (throttle == 1.0f) {
            base.Shoot(target);
        }

        // Set minigun throttle
        IncrThrottle();
    }

    private void IncrThrottle () {
        lastThrottleChange = 0.0f;
        throttle = Mathf.Clamp01(throttle + Time.deltaTime * throttleSpeed);
    }
    private void DecrThrottle () => throttle = Mathf.Clamp01(throttle - Time.deltaTime * throttleSpeed);

    protected override float GetRange () => range;
    protected override float GetShootDelay () => firerate;
    protected override int GetDamage () => damage;
}
