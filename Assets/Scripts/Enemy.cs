using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [Range(0.0f, 1.0f)]
    public float completion = 0.0f; // How much of the track has been passed. 0.0f-1.0f
    [Range(0.0f, 1.0f)]
    public float completionCurrentPt = 0.0f; // How much of the *current point* has been passed.
    public float speed = 0.1f;
    public float travelledDist = 0.0f;
    public float prevSectorsDist = 0.0f;

    public int currentPt = 0;
    private RectTransform trans;

    private static Trackpath path { get { return GameManager.currentPath; } }

    private void Start () {
        trans = GetComponent<RectTransform>();
    }

    private void Update () {
        completion += Time.deltaTime * speed; // Will need to take the path's size into account....
        travelledDist = path.totalDistance * completion;

        // Calculate completion of current sector.
        completionCurrentPt = Mathf.Clamp01((travelledDist - prevSectorsDist) / path[currentPt].dist);

        // Calculate whether current wapoint index should increment
        Vector2 pos = trans.position;
        if (Mathf.Round(Vector3.Distance(pos, path[currentPt + 1].pos)) == 0.0f && currentPt < path.size - 2) {
            prevSectorsDist += path[currentPt].dist;
            ++currentPt;
            completionCurrentPt = 0.0f;
        }


        // Interpolate between each position based on current
        // waypoint and completion.
        trans.anchoredPosition = Vector2.Lerp(path[currentPt].apos, path[currentPt + 1].apos, completionCurrentPt);
    }
}
