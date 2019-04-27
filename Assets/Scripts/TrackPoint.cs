using UnityEngine;

[System.Serializable]
public class TrackPoint {
    /// <summary>World pos</summary>
    public Vector2 pos { get { return trans.position; } }
    /// <summary>Anchored pos</summary>
    public Vector2 apos { get { return trans.anchoredPosition; } }
    public RectTransform trans;
    /// <summary>How far apart this point is from the next point.</summary>
    public float dist;

    public TrackPoint(RectTransform trans) {
        this.trans = trans;
        dist = 0.0f;
    }
}
