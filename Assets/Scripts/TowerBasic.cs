using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Basic tower (pistol)
    Fires single shots at enemies.
*/

public class TowerBasic : TowerBaseShooter {

    private static readonly float RANGE = 5.0f;
    private static readonly float FIRERATE = 0.7f;

    protected override void Update () {
        base.Update();
    }

    protected override float GetRange () => RANGE;
    protected override float GetShootDelay () => FIRERATE;
}
