/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class Spike : MonoBehaviour {

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
        CheckDeath(collider);
    }

    /* --- Methods --- */
    private static void CheckDeath(Collider2D collider) {
        Shuttle shuttle = collider.GetComponent<Shuttle>();
        if (shuttle != null) {
            print("Died");
            Destroy(shuttle.gameObject);
        }
    }

}
