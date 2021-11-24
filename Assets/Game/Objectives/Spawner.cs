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
    private SpriteRenderer spriteRenderer;
    public Shuttle shuttle;
    public Transform purpleArrow;

    /* --- Properties --- */
    private Vector2 direction;
    public float speed;
    public float spawnInterval;
    private float spawnTicks = 0f;
    private KeyCode spawnKey = KeyCode.Space;

    /* --- Unity --- */
    // Start is called before the first frame update
    private void Start() {
        // Cache these references.
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set up these components.
        spriteRenderer.sortingLayerName = GameRules.Midground;

        // Make sure the purple arrow is formatted correctly.
        SetPurpleArrow(purpleArrow.position);
    }

    // Update is called once per frame
    private void Update() {

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

    /* --- Methods --- */
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

    public void SetPurpleArrow(Vector2 position) {

        purpleArrow.position = position;

        // Set the direction.
        direction = (purpleArrow.position - transform.position).normalized;
        // Normalize the actual arrow position.
        purpleArrow.position = transform.position + (Vector3)direction * 0.75f;


        // Get the angle.
        float angularPrecision = 1f;
        float angle = angularPrecision * Mathf.Round(Mathf.Atan(direction.y / direction.x) * 180f / Mathf.PI / angularPrecision);

        // Get the flip.
        int flip = 0;
        if (direction.x <= 0) { angle = -angle; flip = 1; }

        // Set the direction.
        purpleArrow.eulerAngles = Vector3.forward * angle + flip * Vector3.up * 180f;

    }

}
