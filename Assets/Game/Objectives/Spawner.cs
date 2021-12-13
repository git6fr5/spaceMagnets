/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* --- Definitions --- */

/* --- Enumerations --- */

/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class Spawner : MonoBehaviour {

    /* --- Components --- */
    public Transform arrow;
    private SpriteRenderer spriteRenderer;
    public Shuttle shuttle;

    /* --- Properties --- */
    private Vector2 direction;
    public float speed;
    public float spawnInterval;
    private float spawnTicks = 0f;

    [Space(5), Header("Manual Spawning")]
    [SerializeField] private bool manualSpawn = false;
    private KeyCode spawnKey = KeyCode.Space;

    [Space(5), Header("Increment Keys")]
    [SerializeField] private int angleIncrements = 8;
    private int angleIndex = 0;
    private KeyCode clockwiseKey = KeyCode.K;
    private KeyCode counterClockwiseKey = KeyCode.J;

    /* --- Unity --- */
    // Start is called before the first frame update
    private void Start() {
        // Cache these references.
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set up these components.
        spriteRenderer.sortingLayerName = GameRules.Midground;
        direction = Vector2.right;
    }

    // Update is called once per frame
    private void Update() {

        // MouseDirection();

        if (Input.GetKeyDown(clockwiseKey)) {
            IncrementDirection(1);
        }
        else if (Input.GetKeyDown(counterClockwiseKey)) {
            IncrementDirection(-1);
        }

        // Make sure the purple arrow is formatted correctly.
        SetArrow(arrow.position);

        if (manualSpawn) {
            ManualSpawn();
        }
        else {
            AutomaticSpawn();
        }
    }

    /* --- Methods --- */
    private void MouseDirection() {
        // for hard set directions.
        // direction = (arrow.position - transform.position).normalized;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = (mousePos - (Vector2)transform.position).normalized;

    }

    private void IncrementDirection(int increment) {
        angleIndex += increment;
        direction = Quaternion.Euler(0, 0, angleIndex * (360 / angleIncrements)) * Vector2.right;

    }

    private void AutomaticSpawn() {
        // Increment the spawn cooldown.
        if (spawnTicks > 0f) {
            spawnTicks -= Time.deltaTime;
        }
        else {
            Spawn();
        }
    }

    private void ManualSpawn() {
        // Spawn a shuttle on the spawn key.
        if (Input.GetKeyDown(spawnKey) && spawnTicks == 0f && !GameRules.IsEditing) {
            Spawn();
        }

        // Increment the spawn cooldown.
        if (spawnTicks > 0f) {
            spawnTicks -= Time.deltaTime;
        }
        else {
            spawnTicks = 0f;
        }
    }

    private void Spawn() {
        // Instantiate the shuttle.
        Shuttle nextShuttle = Instantiate(shuttle, transform.position + (Vector3)direction / 2f, Quaternion.identity, null);

        // Set up the shuttle.
        nextShuttle.velocity = direction * speed;

        // Activate the shuttle.
        nextShuttle.gameObject.SetActive(true);

        // Set the spawn cooldown.
        spawnTicks = spawnInterval;
    }

    public void SetArrow(Vector2 position) {

        arrow.position = position;

        // Normalize the actual arrow position.
        arrow.position = transform.position + (Vector3)direction * 0.75f;


        // Get the angle.
        float angularPrecision = 1f;
        float angle = angularPrecision * Mathf.Round(Mathf.Atan(direction.y / direction.x) * 180f / Mathf.PI / angularPrecision);

        // Get the flip.
        int flip = 0;
        if (direction.x <= 0) { angle = -angle; flip = 1; }

        // Set the direction.
        arrow.eulerAngles = Vector3.forward * angle + flip * Vector3.up * 180f;

    }

}
