/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class Shuttle : MonoBehaviour {

    /* --- Components --- */
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D body;
    [HideInInspector] public CapsuleCollider2D hitbox;

    /* --- Properties --- */
    public Vector2 velocity;

    /* --- Unity --- */
    private void Start() {
        // Cache these references.
        body = GetComponent<Rigidbody2D>();
        hitbox = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set up these components.
        body.constraints = RigidbodyConstraints2D.FreezeRotation;
        body.gravityScale = 0f;
        body.angularDrag = 0f;
        hitbox.isTrigger = true;
        spriteRenderer.sortingLayerName = GameRules.Midground;
    }

    private void Update() {
        Velocity();
        Point();
    }

    private void Velocity() {
        body.velocity = velocity;

        // Set the material parameters.
        spriteRenderer.material.SetFloat("_Speed", velocity.magnitude);
        spriteRenderer.material.SetFloat("_DirectionX", velocity.normalized.x);
        spriteRenderer.material.SetFloat("_DirectionY", velocity.normalized.y);
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


    /* --- Editor --- */
    private void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)velocity);
    }

}