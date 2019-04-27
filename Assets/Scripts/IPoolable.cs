using UnityEngine;
using System;
using System.Collections.Generic;

/* NOT ACTUALLY USED YET */

public interface IPoolable {
    bool IsVisible();
    void SetVisibility (bool visible);
}
