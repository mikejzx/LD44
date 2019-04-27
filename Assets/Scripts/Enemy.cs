using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    NOTE: This isn't actually being pooled...
        It was planned to be but is currently
        not since i need to work on other stuff lol.
*/

public class Enemy : MonoBehaviour, IPoolable {

    public float speed = 20.0f;
    public int damageDealt = 10; // How much damage it does to player.
    public int strength = 1; // How difficult it is to destroy
    public RectTransform imagePivot;

    public int currentPt = 0;
    private float prevSectorsDist = 0.0f;
    [HideInInspector] public RectTransform trans;
    [Range(0.0f, 1.0f)] private float completion = 0.0f; // How much of the track has been passed. 0.0f-1.0f
    [Range(0.0f, 1.0f)] private float completionCurrentPt = 0.0f; // How much of the *current point* has been passed.

    private bool visible = false;

    private static Trackpath path { get { return GameManager.currentPath; } }

    private void Start () {
        trans = GetComponent<RectTransform>();
        foreach (Image child in transform.GetComponentsInChildren<Image>()) {
            RectTransform r = child.GetComponent<RectTransform>();
            Vector3 a = r.localEulerAngles;
            r.localEulerAngles = new Vector3(270.0f, 180.0f, 0.0f);
        }
        LookAtPoint();
    }

    private void Update () {
        completion += Time.deltaTime * (speed / 100.0f); // Will need to take the path's size into account....
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
            GameManager.player.Damage(strength);
            Die();
        }
    }

    /// <summary>Enemy is destroyed</summary>
    private void Die () {
        Spawner.EnemyRemove(this);
        Destroy(gameObject);
    }

    /// <summary>Makes the enemy look at the next point</summary>
    private void LookAtPoint() {
        // Rotate to look at next point
        if (currentPt > path.size - 2) { return; }
        Vector3 relPos = path[currentPt + 1].pos - new Vector2(imagePivot.position.x, imagePivot.position.y);
        imagePivot.rotation = Quaternion.LookRotation(relPos, Vector3.forward);
    }

    // UNUSED --- --
    public bool IsVisible () => visible;
    public void SetVisibility (bool visible) => this.visible = visible;
}
