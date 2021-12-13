using UnityEngine;

public struct LatLong {
    public float latitude;
    public float longitude;

    public LatLong(Vector3 point, float sphereSize) {
        this.longitude = Mathf.Atan2(point.y, Mathf.Sqrt(point.x * point.x + point.z * point.z));
        this.latitude = Mathf.Atan2(point.x, -point.z);

        this.latitude = Mathf.Rad2Deg * latitude;
        this.longitude = Mathf.Rad2Deg * longitude;
    }

    public Vector2 GetUV() {
        Vector2 v = Vector2.zero;
        v.x = Mathf.Lerp(0f, 2f, (latitude + 180f) / 360f);
        v.y = Mathf.Lerp(0f, 1f, (longitude + 90f) / 180f);
        return v;
    }
}
