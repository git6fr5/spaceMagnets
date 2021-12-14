/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Score))]
public class Wormhole : MonoBehaviour {

    public enum WormholeRotation {
        Clockwise,
        Counterclockwise
    }

    /* --- Components --- */
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D hitbox;
    private Rigidbody2D body;
    private Score score;
    public Rope rope;

    /* --- Properties --- */
    public WormholeRotation rotation;
    public Wormhole originPoint;
    public Wormhole targetPoint;
    public string wormholeName;
    public int scoreValue = 0;
    public Vector2 velocity;
    [Range(0.05f, 1f)] public float velocityFactor; // either make this static - or make it so that if the interactable component is moving, then this has 0 or less value.

    /* --- Unity --- */
    private void Start() {
        // Cache these components
        spriteRenderer = GetComponent<SpriteRenderer>();
        hitbox = GetComponent<CircleCollider2D>();
        score = GetComponent<Score>();
        body = GetComponent<Rigidbody2D>();

        // Set up the components
        score.value = scoreValue;
        spriteRenderer.sortingLayerName = GameRules.Midground;

        body.constraints = RigidbodyConstraints2D.FreezeRotation;
        body.gravityScale = 0f;
        body.angularDrag = 0f;
        hitbox.isTrigger = true;

        if (targetPoint != null) {
            rope.startpoint = transform;
            rope.endpoint = targetPoint.transform;
            rope.ropeLength = 7f;
            rope.ropeWidth = 0.1f;
        }
        else if (rope != null) {
            rope.gameObject.SetActive(false);
        }

    }

    private void Update() {

        velocity *= 0.95f;
        if (velocity.sqrMagnitude < 0.001f * 0.001f) {
            velocity = Vector3.zero;
        }
        body.velocity = velocity;

        if (originPoint != null) {
            OriginBounds();
        }
        if (targetPoint != null) {
            TargetBounds();
        }

        if (Background.Instance?.grid != null && rotation == WormholeRotation.Clockwise) {
            Background.Instance.grid.ApplyClockwiseForce(1000f, transform.position, 1f);
        }
        else {
            Background.Instance.grid.ApplyCounterClockwiseForce(1000f, transform.position, 1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        CheckTeleport(collider);
    }

    /* --- Methods --- */
    private void CheckTeleport(Collider2D collider) {
        Shuttle shuttle = collider.GetComponent<Shuttle>();
        if (shuttle != null && targetPoint != null) {
            print("Died");
            // Add the score
            shuttle.transform.position = targetPoint.transform.position + (Vector3)shuttle.velocity.normalized * hitbox.radius;
        }
    }

    private void OriginBounds() {

        float distance = (transform.position - originPoint.transform.position).magnitude;

        if (distance  > originPoint.rope.ropeLength) {
            velocity -= (Vector2)(transform.position - originPoint.transform.position).normalized * (distance / originPoint.rope.ropeLength) * velocityFactor;
        }


    }

    private void TargetBounds() {

        float distance = (targetPoint.transform.position - transform.position).magnitude;

        if (distance > rope.ropeLength) {
            velocity += (Vector2)(targetPoint.transform.position - transform.position).normalized * (distance / rope.ropeLength) * velocityFactor;
        }


    }

}
