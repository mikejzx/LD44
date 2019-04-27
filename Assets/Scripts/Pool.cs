using UnityEngine;
using System;
using System.Collections.Generic;

/*
    A generic class type used for object pools. 
*/
/* NOT ACTUALLY USED YET */

public class Pool<T> where T : IPoolable {
    public int size = 0;

    protected List<T> pool; // The whole pool.
    protected List<T> visible; // Objects currently displayed
    protected List<T> invisible; // Objects still free
    protected int currentlyVisible = 0;

    public Pool () { }

    public Pool (List<T> initialiser) { Initialise(initialiser); }

    public void Initialise(List<T> i) {
        pool = new List<T>(i);
        invisible = new List<T>(i);
        visible = new List<T>();
    }

    public T this[int i] {
        get { return pool[i]; }
        set { pool[i] = value; }
    }

    /// <summary>Get an object</summary>>
    public T GetObject () {
        if (invisible.Count == 0) {
            // No pooled objects left. Return nothing?
            return visible[0];
        }
        invisible[0].SetVisibility(true);
        return invisible[0];
    }
}
