/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class Block : MonoBehaviour {

    /* --- Components --- */
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D hitbox;

    /* --- Unity --- */
    private void Start() {
        // Cache these references.
        hitbox = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set up these components.
        hitbox.isTrigger = true;
        spriteRenderer.sortingLayerName = GameRules.Midground;
    }

    void OnTriggerEnter2D(Collider2D collider) {
        CheckBounce(collider);
    }

    /* --- Methods --- */
    public Vector2 normalVector;

    private void CheckBounce(Collider2D collider) {
        Shuttle shuttle = collider.GetComponent<Shuttle>();
        if (shuttle != null) {
            print("Getting Normal");
            normalVector = GetNormal((Vector2)shuttle.transform.position);
            Bounce(shuttle, normalVector);
        }
    }

    private Vector2 GetNormal(Vector2 position) {

        Vector2 diff = position - (Vector2)transform.position;

        // If they're around the corner
        if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y) - 0.05f && Mathf.Abs(diff.x) < Mathf.Abs(diff.y) + 0.05f) {
            return (new Vector2(Mathf.Sign(diff.x), Mathf.Sign(diff.y))).normalized;
        }
        // If they're on the horizontal axis
        else if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y)) {
            return new Vector2(Mathf.Sign(diff.x), 0f);
        }
        // If they're on the vertical axis
        else {
            return new Vector2(0f, Mathf.Sign(diff.y));
        }

    }

    private void Bounce(Shuttle shuttle, Vector2 normalVector) {

        Vector2 normalForce = shuttle.velocity * normalVector;
        shuttle.velocity += 2f * normalForce;

    }


    /* --- Editor --- */
    private void OnDrawGizmos() {
        Gizmos.color = GameRules.White;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)normalVector);
    }

}
