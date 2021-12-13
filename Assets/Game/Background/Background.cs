/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Arcade style background.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class Background : MonoBehaviour {

    /* --- Components --- */
    [HideInInspector] public SpriteRenderer spriteRenderer;
    public Star star;
    public Sand sand; // Eventually will a sandUI
    public LineRenderer lineRenderer;

    /* --- Properties --- */
    [Space(5), Header("Sand Grid")]
    [SerializeField] private bool enableSandGrid = false;
    [SerializeField] [Range(10, 100)] private int verticalPrecision = 5;
    [SerializeField] [Range(10, 100)] private int horizontalPrecision = 5;
    [SerializeField] private Sand[][] sandGrid;
    [Space(5), Header("Shooting Stars")]
    [SerializeField] private bool enableStars = false;
    [SerializeField] [Range(1, 10)] private int batchSize = 5;
    [SerializeField] [Range(0.05f, 2f)] private float fireInterval = 1f;
    [Space(5), Header("Force Field Lines")]
    [SerializeField] private bool enableLineGrid = false;
    [SerializeField] private LineRenderer[] verticalLineGrid;
    [SerializeField] private LineRenderer[] horizontalLineGrid;
    [SerializeField] private float lineWidth = 0.05f;


    /* --- Unity --- */
    // Runs once before the first frame.
    private void Start() {
        // Cache these references.
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Stars.
        if (enableStars) {
            StartCoroutine(IEShootStar());
        }

        // Sand grid
        if (enableSandGrid) {
            SandGrid();
            if (enableLineGrid) {
                LineGrid();

            }
        }

        
    }

    private void Update() {

        if (enableLineGrid) {
            UpdateLineGrid();
        }
    }

    /* --- Methods --- */
    private void SandGrid() {

        Vector2 offset = new Vector2(GameRules.PixelsHorizontal, GameRules.PixelsVertical) / (2f * GameRules.PixelsPerUnit) - new Vector2(0.25f, 0.25f);
        float scaleX =  ((float)(GameRules.PixelsHorizontal / GameRules.PixelsPerUnit)) / (float)horizontalPrecision; // Distance We Need To Cover / Amount
        float scaleY = ((float)(GameRules.PixelsVertical / GameRules.PixelsPerUnit)) / (float)verticalPrecision; // Distance We Need To Cover / Amount

        sandGrid = new Sand[verticalPrecision][];
        for (int i = 0; i < verticalPrecision; i++) {
            sandGrid[i] = new Sand[horizontalPrecision];
            for (int j = 0; j < horizontalPrecision; j++) {

                Sand newSand = Instantiate(sand.gameObject, new Vector2(j * scaleX, i * scaleY) - offset, Quaternion.identity, transform).GetComponent<Sand>();
                newSand.gameObject.SetActive(true);
                sandGrid[i][j] = newSand;

            }
        }

    }

    private void LineGrid() {

        verticalLineGrid = new LineRenderer[verticalPrecision];
        for (int i = 0; i < verticalPrecision; i++) {

            verticalLineGrid[i] = Instantiate(lineRenderer.gameObject, transform.position, Quaternion.identity, transform).GetComponent<LineRenderer>();
            verticalLineGrid[i].gameObject.SetActive(true);

            for (int j = 0; j < horizontalPrecision; j++) {
                sandGrid[i][j].GetComponent<SpriteRenderer>().enabled = false;
            }
        }

        horizontalLineGrid = new LineRenderer[horizontalPrecision];
        for (int i = 0; i < horizontalPrecision; i++) {

            horizontalLineGrid[i] = Instantiate(lineRenderer.gameObject, transform.position, Quaternion.identity, transform).GetComponent<LineRenderer>();
            horizontalLineGrid[i].gameObject.SetActive(true);
        }
    }

    private void UpdateLineGrid() {

        for (int i = 0; i < verticalPrecision; i++) {

            Vector3[] positions = new Vector3[horizontalPrecision];
            for (int j = 0; j < horizontalPrecision; j++) {
                positions[j] = sandGrid[i][j].transform.position;
                positions[j] += new Vector3(0f, 0f, -1f);
            }

            verticalLineGrid[i].startWidth = lineWidth;
            verticalLineGrid[i].endWidth = lineWidth;
            verticalLineGrid[i].positionCount = horizontalPrecision;
            verticalLineGrid[i].SetPositions(positions);
        }

        for (int i = 0; i < horizontalPrecision; i++) {

            Vector3[] positions = new Vector3[verticalPrecision];
            for (int j = 0; j < verticalPrecision; j++) {
                positions[j] = sandGrid[j][i].transform.position;
                positions[j] += new Vector3(0f, 0f, -1f);
            }

            horizontalLineGrid[i].startWidth = lineWidth;
            horizontalLineGrid[i].endWidth = lineWidth;
            horizontalLineGrid[i].positionCount = verticalPrecision;
            horizontalLineGrid[i].SetPositions(positions);
        }

    }

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

}
