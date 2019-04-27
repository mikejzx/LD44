using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trackpath : MonoBehaviour {

    public float totalDistance = 0.0f;
    /// <summary>The number of points in the path</summary>
    public float size { get { return points.Count; } }
    private List<TrackPoint> points = new List<TrackPoint>();

    private void Start () {
        FindPoints();
    }

    // Allows sqr-bracket operator to be used.
    public TrackPoint this[int idx] {
        get { return points[idx]; }
        set { points[idx] = value; }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos () {
        FindPoints();
        if (points.Count < 2) {
            Debug.Log("Returning...");
            return;
        }

        Gizmos.color = Color.red;
        Vector2 prev = points[0].pos;
        for (int i = 1; i < points.Count; i++) {
            Vector2 next = points[i].pos;
            Gizmos.DrawLine(prev, next);
            prev = next;
        }
    }
#endif

    /// <summary>Find the track points. (Children of the object)</summary>
    private void FindPoints() {
        if (points == null) {
            points = new List<TrackPoint>();
        }
        points.Clear();
        int i = 0;
        Vector2 prevPos = Vector2.zero;
        totalDistance = 0.0f;
        foreach (Transform child in transform) {
            child.gameObject.name = "Pt" + i;
            RectTransform trans = child.GetComponent<RectTransform>();
            float dist = Vector2.Distance(prevPos, trans.position);
            totalDistance += dist;
            prevPos = trans.position;
            TrackPoint pt = new TrackPoint(trans, dist);
            points.Add(pt);
            ++i;
        }
    }
}
