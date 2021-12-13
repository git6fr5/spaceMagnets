/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : MonoBehaviour {

    /* --- Components --- */
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D hitbox;
    private Rigidbody2D body;
    public AsteroidWormhole target;

    /* --- Properties --- */
    [SerializeField] private float speed = 0;

    /* --- Unity --- */
    private void Start() {
        // Cache these references.
        hitbox = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();

        // Set up these components.
        body.constraints = RigidbodyConstraints2D.FreezeRotation;
        body.gravityScale = 0f;
        body.angularDrag = 0f;
        hitbox.isTrigger = true;
        spriteRenderer.sortingLayerName = GameRules.Midground;
    }

    private void Update() {
        Velocity();
    }

    void OnTriggerEnter2D(Collider2D collider) {
        CheckDeath(collider);
    }

    /* --- Methods --- */
    private void Velocity() {
        body.velocity = (target.transform.position - transform.position).normalized * speed;
    }

    private static void CheckDeath(Collider2D collider) {
        Shuttle shuttle = collider.GetComponent<Shuttle>();
        if (shuttle != null) {
            print("Died");
            Destroy(shuttle.gameObject);
        }
    }

}
