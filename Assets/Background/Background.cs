/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Arcade style background.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class Background : MonoBehaviour {

    public static Background Instance;

    /* --- Enumerations --- */
    public enum GridRenderMode {
        Point,
        Square,
        MeshPoint,
        MeshLine
    }

    /* --- Components --- */
    [HideInInspector] public SpriteRenderer spriteRenderer;
    public Texture2D pixel;
    public Star star;
    // public Sand sand; // Eventually will a sandUI

    /* --- Properties --- */
    [Space(5), Header("Sand Grid")]
    [SerializeField] public Grid grid;
    [SerializeField] private bool buildGrid = false;

    public GridRenderMode gridRenderMode = GridRenderMode.Point;
    public float gridMassPerPoint = 1;
    [Range(0.95f, 1f)] public float massVelocityDamping = 0.995f; // The lower, the more snappy?
    [Range(100f, 1000f)] public float springDisplacementFactor = 100f; // The higher, the faster it snaps back (catch-all for all types of springs).
    [Range(0.95f, 1f)] public float springTaughtness = 0.995f;

    [Range(10, 100)] public int verticalPrecision = 5;
    [Range(10, 100)] public int horizontalPrecision = 5;
    [Range(0.001f, 0.999f)] public float borderAnchorStiffness = 0.999f; // The higher, the faster it snaps back (for these particular springs)
    [Range(0.001f, 0.999f)] public float borderAnchorDamping = 0.001f;  // Redundant.
    public int anchorRatio = 3;
    [Range(0.001f, 0.999f)] public float anchorStiffness = 0.999f;  // The higher, the faster it snaps back (for these particular springs)
    [Range(0.001f, 0.999f)] public float anchorDamping = 0.001f;  // Redundant.
    [Range(0.001f, 0.999f)] public float springStiffness = 0.999f;  // The higher, the faster it snaps back (for these particular springs)
    [Range(0.001f, 0.999f)] public float springDamping = 0.001f; // Redundant.
    public float lineThickness = 1f;

    [SerializeField] private Sand[][] sandGrid;
    [Space(5), Header("Shooting Stars")]
    [SerializeField] private bool enableStars = false;
    [SerializeField] [Range(1, 10)] private int batchSize = 5;
    [SerializeField] [Range(0.05f, 2f)] private float fireInterval = 1f;


    // Don't know what I'm doing here.
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    /* --- Unity --- */
    // Runs once before the first frame.
    private void Start() {
        // Cache these references.
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Stars.
        if (enableStars) {
            StartCoroutine(IEShootStar());
        }

        //mesh = new Mesh();
        Instance = this;

    }

    void OnGUI() {
        
        if (grid != null) {

            List<Vector3> positions = new List<Vector3>();
            List<int> indices = new List<int>();
            List<Color> colors = new List<Color>();
            int index = 0;

            for (int i = 1; i < verticalPrecision; i++) {
                for (int j = 1; j < horizontalPrecision; j++) {

                    Vector2 screenPosA;
                    Vector2 screenPosB;
                    Rect rect;

                    switch (gridRenderMode) {

                        case GridRenderMode.Point:
                            screenPosA = Camera.main.WorldToScreenPoint(grid.points[i][j - 1].position);
                            rect = new Rect(screenPosA.x, screenPosA.y, lineThickness, lineThickness);
                            Graphics.DrawTexture(rect, pixel);
                            break;
                        case GridRenderMode.Square:
                            screenPosA = Camera.main.WorldToScreenPoint(grid.points[i - 1][j].position);
                            screenPosB = Camera.main.WorldToScreenPoint(grid.points[i][j].position);
                            rect = new Rect(screenPosA.x, screenPosA.y, lineThickness, screenPosB.y - screenPosA.y);
                            Graphics.DrawTexture(rect, pixel);

                            screenPosA = Camera.main.WorldToScreenPoint(grid.points[i][j - 1].position);
                            screenPosB = Camera.main.WorldToScreenPoint(grid.points[i][j].position);
                            rect = new Rect(screenPosA.x, screenPosA.y, screenPosB.x - screenPosA.x, lineThickness);
                            Graphics.DrawTexture(rect, pixel);
                            break;
                        case GridRenderMode.MeshPoint:
                            positions.Add(grid.points[i - 1][j - 1].position);
                            indices.Add(index);
                            colors.Add(Color.white);
                            index = index + 1;
                            break;
                        default:
                            i = verticalPrecision;
                            break;
                    }
                }
            }

            if (gridRenderMode == GridRenderMode.MeshLine) {
                for (int i = 0; i < grid.springs.Length; i++) {

                    positions.Add(grid.springs[i].nodeA.position);
                    positions.Add(grid.springs[i].nodeB.position);

                    indices.Add(2 * i);
                    indices.Add(2 * i + 1);
                    colors.Add(Color.white);
                    colors.Add(Color.white);
                }
            }



            if (gridRenderMode == GridRenderMode.MeshPoint) {
                meshFilter.mesh.SetVertices(positions);
                meshFilter.mesh.SetIndices(indices.ToArray(), MeshTopology.Points, 0);
                meshFilter.mesh.colors = colors.ToArray();
                // Graphics.DrawMeshNow(meshFilter.mesh, Matrix4x4.identity);
            }
            else if (gridRenderMode == GridRenderMode.MeshLine) {
                meshFilter.mesh.SetVertices(positions);
                meshFilter.mesh.SetIndices(indices.ToArray(), MeshTopology.Lines, 0);
                meshFilter.mesh.colors = colors.ToArray();
                // Graphics.DrawMeshNow(meshFilter.mesh, Matrix4x4.identity);
            }

        }
    }

    private void Update() {
        if (buildGrid) {
            meshFilter.mesh = new Mesh();
            grid = new Grid(gridMassPerPoint, massVelocityDamping,
                verticalPrecision, horizontalPrecision,
                springDisplacementFactor, springTaughtness,
                borderAnchorStiffness, borderAnchorDamping,
                anchorRatio, anchorStiffness, anchorDamping,
                springStiffness, springDamping
                );;
            buildGrid = false;
        }
    }

    private void FixedUpdate() {
        if (grid != null) {
            grid.Update(Time.fixedDeltaTime);
        }
    }

    /* --- Methods --- */

    /* --- Coroutines --- */
    // Spawns shooting pixels on a looped timer.
    IEnumerator IEShootStar() {
        yield return new WaitForSeconds(fireInterval);
        for (int i = 0; i < batchSize; i++) {
            Star newShootingStar = Instantiate(star.gameObject).GetComponent<Star>();
            newShootingStar.transform.parent = transform;
            newShootingStar.gameObject.SetActive(true);
        }
        yield return StartCoroutine(IEShootStar());
    }

    /* --- Editor --- */
    private void OnDrawGizmos() {
    }

}
