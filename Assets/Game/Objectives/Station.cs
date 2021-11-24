/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
public class Station : MonoBehaviour {

    /* --- Components --- */
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D hitbox;

    /* --- Unity --- */
    private void Start() {
        // Cache these references.
        hitbox = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set up these components.
        hitbox.isTrigger = true;
        spriteRenderer.sortingLayerName = GameRules.Midground;
    }

    void OnTriggerEnter2D(Collider2D collider) {
        CheckWin(collider);
    }

    /* --- Methods --- */
    private static void CheckWin(Collider2D collider) {
        Shuttle shuttle = collider.GetComponent<Shuttle>();
        if (shuttle != null) {
            print("Won game");
            Destroy(shuttle.gameObject);
        }
    }

}
