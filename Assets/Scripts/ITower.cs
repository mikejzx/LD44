﻿using UnityEngine;
using System;
using System.Collections.Generic;

public interface ITower {
    void ShowRangeCircle (bool show);
    GameObject GetRangeCircle ();
    void SetEnabled (bool enable);
    TowerInfo MyInfo ();
    void SetTowerInfo (TowerInfo t);
    GameObject GetGameobject ();
    void LockMovement (); // Locks the movement so it cannot be dragged
    int SellPrice ();
}
