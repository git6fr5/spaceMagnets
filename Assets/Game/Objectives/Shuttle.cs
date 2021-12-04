/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class Shuttle : MonoBehaviour {

    /* --- Components --- */
    private SpriteRenderer spriteRenderer;
    [HideInInspector] public CapsuleCollider2D hitbox;

    /* --- Properties --- */
    public Vector2 velocity;

    /* --- Unity --- */
    private void Start() {
        // Cache these references.
        hitbox = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set up these components.
        hitbox.isTrigger = true;
        spriteRenderer.sortingLayerName = GameRules.Midground;
    }

    private void Update() {

        if (!GameRules.IsWithinBounds(transform)) {
            Explode();
        }

    }

    private void FixedUpdate() {
        Move();
        Point();
    }

    private void Move() {
        Vector2 deltaPosition = velocity * Time.fixedDeltaTime;
        transform.position += (Vector3)(deltaPosition);
    }

    /* --- Methods --- */
    private void Point() {

        // Get the angle.
        float angularPrecision = 1f;
        float angle = angularPrecision * Mathf.Round(Mathf.Atan(velocity.y / velocity.x) * 180f / Mathf.PI / angularPrecision);

        // Get the flip.
        int flip = 0;
        if (velocity.x <= 0) { angle = -angle; flip = 1; }

        // Set the direction.
        transform.eulerAngles = Vector3.forward * angle + flip* Vector3.up * 180f;
    }

    public void Explode() {

        // Play an animation.

        // Self-destruct
        Destroy(gameObject);

    }

    /* --- Editor --- */
    private void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)velocity);
    }

}