/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(CircleCollider2D))]
public class Score : MonoBehaviour {

    /* --- Components --- */
    [HideInInspector] public CircleCollider2D hitbox;

    /* --- Properties --- */
    [HideInInspector] public int value = 0;

    /* --- Unity --- */
    private void Start() {
        // Cache these references.
        hitbox = GetComponent<CircleCollider2D>();

        // Set up these components.
        hitbox.isTrigger = true;
    }

    void OnTriggerStay2D(Collider2D collider) {
        CheckScore(collider);
    }

    /* --- Methods --- */
    private static void CheckScore(Collider2D collider) {
        Shuttle shuttle = collider.GetComponent<Shuttle>();
        if (shuttle != null) {
            print("Died");
            // Add the score
            Destroy(shuttle.gameObject);
        }
    }

}
