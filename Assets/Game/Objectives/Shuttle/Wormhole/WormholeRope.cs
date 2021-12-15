using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class WormholeRope : MonoBehaviour {

    /* --- Components --- */
    protected MeshFilter meshFilter;
    protected MeshRenderer meshRenderer;

    /* --- Static Variables --- */
    [SerializeField] protected static float SegmentLength = 0.2f;
    [SerializeField] protected static float SegmentWeight = 1.5f;
    [SerializeField] protected static int ConstraintDepth = 50;

    /* --- Variables --- */
    public bool initialized = false;

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
        positions.Add(ropeSegments[0]);

        List<Color> colors = new List<Color>();
        colors.Add(Color.white);

        List<int> indices = new List<int>();
        
        for (int i = 1; i < ropeSegments.Length; i++) {
            positions.Add(ropeSegments[i]);
            indices.Add(i - 1);
            indices.Add(i);
            colors.Add(Color.white);
        }

        meshFilter.mesh.SetVertices(positions);
        meshFilter.mesh.SetIndices(indices.ToArray(), MeshTopology.Lines, 0);
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
