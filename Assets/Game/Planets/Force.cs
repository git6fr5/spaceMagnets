/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* --- Definitions --- */

/* --- Enumerations --- */

/// <summary>
/// 
/// </summary>
public class Force : MonoBehaviour {

    public enum Direction {
        Push = -1, Pull = 1
    }

    /* --- Components --- */
    public Direction direction;

    /* --- Properties --- */
    public float mass;
    public float horizon;
    public float radius;

    // State Variables
    public bool isActive = false;
    public bool pulse = false; // purely aesthetic.
    public float pulseInterval = 0.3f;

    private float internalTicks = 0f;
    public float period;

    /* --- Unity --- */
    void Start() {
        if (pulse) {
            StartCoroutine(IEPulseForce(pulseInterval));
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = direction == Direction.Pull ? GameRules.Red : GameRules.Blue;
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.DrawWireSphere(transform.position, horizon);
    }

    void Update() {
        if (!pulse) {
            ApplyForces();
        }

        internalTicks += Time.deltaTime;
    }

    public static int Precision = 72;
    private Vector3 origin;
    private MeshFilter outerForceMesh;
    public Material[] lineMats;

    void FixedUpdate() {
        //

        GetComponent<SpriteRenderer>().material.SetFloat("_OffsetY", 2f / 16f * Mathf.Sin(period * Mathf.PI * internalTicks));

        if (outerForceMesh != null && origin == transform.position) {
            return;
        }

        if (outerForceMesh == null) {
            GameObject meshObject = new GameObject();
            meshObject.transform.parent = transform;
            meshObject.transform.localPosition = new Vector3(0f, 0f, -1f);
            meshObject.AddComponent<MeshFilter>();

            outerForceMesh = meshObject.GetComponent<MeshFilter>();
            outerForceMesh.mesh = new Mesh();

            meshObject.AddComponent<MeshRenderer>();
            meshObject.GetComponent<MeshRenderer>().materials = lineMats;

        }

        List<Vector3> positions = new List<Vector3>();
        // int[] indices = new int[2 * Precision];
        List<int> indices = new List<int>();
        List<Color> colors = new List<Color>();

        positions.Add(radius * Vector2.right);
        positions.Add(horizon * Vector2.right);

        for (int i = 1; i < Precision; i++) {

            // Makes a very cool wavy pattern.
            // Vector3 outerPoint = (radius + (1f / 16f) * Mathf.Sin() * (Quaternion.Euler(0, 0, 360f * (float)i / Precision) * Vector2.right);

            float a = (1f / 16f);
            //float t = (i / Mathf.PI) * Time.time;
            //float t = (i / Mathf.PI) + Time.time;
            // float t = ((float)i / 100) * ((i / Mathf.PI) + Time.time);
            float t = (i / Mathf.PI);

            Vector3 outerPoint = (radius + a * Mathf.Sin(t)) * (Quaternion.Euler(0, 0, 360f * (float)i / Precision) * Vector2.right);
            Vector3 innerPoint = (horizon + a * Mathf.Sin(t)) * (Quaternion.Euler(0, 0, 360f * (float)i / Precision) * Vector2.right);

            positions.Add(outerPoint);
            positions.Add(innerPoint);

            indices.Add(2 * (i-1));
            indices.Add(2 * i);

            indices.Add(2 * (i - 1) + 1);
            indices.Add(2 * i + 1);

            colors.Add(GameRules.Red);
            colors.Add(GameRules.Red);

        }

        indices.Add(2 * (Precision - 1));
        indices.Add(0);

        indices.Add(2 * (Precision - 1) + 1);
        indices.Add(1);

        colors.Add(GameRules.Red);
        colors.Add(GameRules.Red);

        outerForceMesh.mesh.SetVertices(positions);
        outerForceMesh.mesh.SetIndices(indices.ToArray(), MeshTopology.Lines, 0);
        outerForceMesh.mesh.colors = colors.ToArray();

        origin = transform.position;

    }

    /* --- Methods --- */
    private void ApplyForces() {
        if (Background.Instance?.grid != null) {
            float magnitude = 500f * 0.25f * Mathf.Sqrt(mass / 0.25f);
            Background.Instance.grid.ApplyImplosiveForce(magnitude, transform.position, radius);
            // float horizonForce = Mathf.Min(magnitude * magnitude, 250f * 250f);
            // Background.Instance.grid.ApplyImplosiveForce(magnitude * magnitude, transform.position, horizon);
        }
    }

    /* --- Virtual --- */
    protected virtual Collider2D[] Area() { // Obsolete but a mess to clean up.
        return new Collider2D[0];
    }

    /* --- Coroutines --- */
    private IEnumerator IEPulseForce(float delay) {
        yield return new WaitForSeconds(delay);
        pulse = !pulse;
        StartCoroutine(IEPulseForce(pulseInterval));
        yield return null;
    }


}