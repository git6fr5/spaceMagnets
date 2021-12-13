/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Score))]
public class Wormhole : MonoBehaviour {

    /* --- Components --- */
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D hitbox;
    private Score score;
    private Rope rope;

    /* --- Properties --- */
    public Wormhole targetPoint;
    public string wormholeName;
    public int scoreValue;

    /* --- Unity --- */
    private void Start() {
        // Cache these components
        spriteRenderer = GetComponent<SpriteRenderer>();
        hitbox = GetComponent<CircleCollider2D>();
        score = GetComponent<Score>();

        // Set up the components
        score.value = scoreValue;
        spriteRenderer.sortingLayerName = GameRules.Midground;
        if (targetPoint != null) {
            rope = gameObject.AddComponent<Rope>();
            rope.startpoint = transform;
            rope.endpoint = targetPoint.transform;
            rope.ropeLength = 5f;
            rope.ropeWidth = 0.1f;
        }

    }

    private void Update() {

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

}
