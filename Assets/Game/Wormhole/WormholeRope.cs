using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class WormholeRope : MonoBehaviour {

    public enum RenderMode { 
        MeshQuads,
        MeshLines
    }
    public RenderMode renderMode;

    /* --- Components --- */
    protected MeshFilter meshFilter;
    protected MeshRenderer meshRenderer;

    /* --- Static Variables --- */
    [SerializeField] protected static float SegmentLength = 1f / 16f;
    [SerializeField] protected static float SegmentWeight = 1.5f;
    [SerializeField] protected static int ConstraintDepth = 50;

    /* --- Variables --- */
    public bool initialized = false;

    public Gradient gradient;

    [HideInInspector] protected int segmentCount; // The number of segments.
    [SerializeField] public Transform startpoint;
    [SerializeField] public Transform endpoint; 
    [SerializeField] public float ropeLength; // The length of the rope.
    [SerializeField] public float ropeWidth; // The width of the rope.
    [SerializeField] protected Vector3[] ropeSegments; // The current positions of the segments.
    [SerializeField] protected Vector3[] prevRopeSegments; // The previous positions of the segments.

    /* --- Unity --- */
    // Runs once on initialization.
    void Awake() {
        // Cache these references.
        meshFilter = GetComponent<MeshFilter>();

        // Set up these components.
        meshFilter.mesh = new Mesh();
    }

    // Runs once every frame.
    void Update() {
        if (!initialized) {
            RopeSegments();
            initialized = true;
        }
    }

    void OnGUI() {
        if (initialized) {
            Render();
        }
    }

    // Runs once every set time interval.
    void FixedUpdate() {
        if (initialized) {
            Simulation();
        }
    }

    /* --- Methods --- */
    // Initalizes the rope segments.
    void RopeSegments() {
        // Get the number of segments for a rope of this length.
        segmentCount = (int)Mathf.Ceil(ropeLength / SegmentLength);

        // Initialize the rope segments.
        ropeSegments = new Vector3[segmentCount];
        prevRopeSegments = new Vector3[segmentCount];

        for (int i = 0; i < segmentCount; i++) {
            ropeSegments[i] = Vector2.zero;
            prevRopeSegments[i] = ropeSegments[i];
        }
        ropeSegments[segmentCount - 1] += ropeLength * Vector3.right;

    }

    // Renders the rope using the line renderer and edge collider.
    void Render() {

        List<Vector3> positions = new List<Vector3>();
        List<int> indices = new List<int>();
        List<Color> colors = new List<Color>();
        MeshTopology meshTopology = MeshTopology.Points;

        switch (renderMode) {
            case (RenderMode.MeshQuads):

                float size = 1f / 16f;
                meshTopology = MeshTopology.Triangles;

                for (int i = 0; i < ropeSegments.Length; i += 1) {

                    Vector3 position = ropeSegments[i]; // - transform.position;

                    positions.Add(position);
                    positions.Add(position + new Vector3(size, 0f, 0f));
                    positions.Add(position + new Vector3(0f, size, 0f));
                    positions.Add(position + new Vector3(size, size, 0f));

                    indices.Add(4 * i + 0);
                    indices.Add(4 * i + 1);
                    indices.Add(4 * i + 3);

                    indices.Add(4 * i + 0);
                    indices.Add(4 * i + 2);
                    indices.Add(4 * i + 3);

                    // normalizedSpeed = 0f; // Mathf.Min(1f, shuttleMasses[i].velocity.sqrMagnitude / 5f);
                    Color col = gradient.Evaluate((float)i / (float)ropeSegments.Length);
                    colors.Add(col);
                    colors.Add(col);
                    colors.Add(col);
                    colors.Add(col);
                }
                break;
            case (RenderMode.MeshLines):

                meshTopology = MeshTopology.Lines;
                positions.Add(ropeSegments[0]);
                colors.Add(Color.yellow);

                for (int i = 1; i < ropeSegments.Length; i++) {
                    positions.Add(ropeSegments[i]);
                    indices.Add(i - 1);
                    indices.Add(i);
                    colors.Add(Color.yellow);
                }
                break;
        }

        meshFilter.mesh.SetVertices(positions);
        meshFilter.mesh.SetIndices(indices.ToArray(), meshTopology, 0);
        meshFilter.mesh.colors = colors.ToArray();

    }

    void Simulation() {
        // Vector3 forceGravity = new Vector3(0f, -SegmentWeight * 1f, 0f);
        for (int i = 0; i < segmentCount; i++) {
            Vector3 velocity = ropeSegments[i] - prevRopeSegments[i];
            prevRopeSegments[i] = ropeSegments[i];
            ropeSegments[i] += velocity;
            // ropeSegments[i] += forceGravity * Time.fixedDeltaTime;
        }
        for (int i = 0; i < ConstraintDepth; i++) {
            Constraints();
        }
    }

    protected virtual void Constraints() {
        ropeSegments[0] = Vector2.zero;
        ropeSegments[segmentCount - 1] = endpoint.position - startpoint.position;

        for (int i = 1; i < segmentCount; i++) {
            // Get the distance and direction between the segments.
            float newDist = (ropeSegments[i - 1] - ropeSegments[i]).magnitude;
            Vector3 direction = (ropeSegments[i - 1] - ropeSegments[i]).normalized;

            // Get the error term.
            float error = newDist - SegmentLength;
            Vector3 errorVector = direction * error;

            // Adjust the segments by the error term.
            if (i != 1) {
                ropeSegments[i - 1] -= errorVector * 0.5f;
            }
            ropeSegments[i] += errorVector * 0.5f;
        }

        //ropeSegments[0] = Vector2.zero; // startpoint.position;
        //for (int i = 1; i < segmentCount; i++) {
        //    // Get the distance and direction between the segments.
        //    float newDist = (ropeSegments[i - 1] - ropeSegments[i]).magnitude;
        //    Vector3 direction = (ropeSegments[i - 1] - ropeSegments[i]).normalized;

        //    // Get the error term.
        //    float error = newDist - SegmentLength;
        //    Vector3 errorVector = direction * error;

        //    // Adjust the segments by the error term.
        //    if (i != 1) {
        //        ropeSegments[i - 1] -= errorVector * 0.5f;
        //    }
        //    ropeSegments[i] += errorVector * 0.5f;
        //}
    }

}
