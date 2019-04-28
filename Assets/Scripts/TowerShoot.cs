using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Basic tower (pistol)
    Fires single shots at enemies.
*/

public class TowerShoot : TowerBaseShooter {

    public float range = 5.0f;
    public float firerate = 0.7f;
    public int damage = 1;

    protected override void Update () {
        base.Update();
    }

    protected override float GetRange () => range;
    protected override float GetShootDelay () => firerate;
    protected override int GetDamage () => damage;
}
