/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
public class AsteroidWormhole : MonoBehaviour {

    /* --- Components --- */
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D hitbox;
    public AsteroidWormhole target;
    public Asteroid asteroidBase;

    /* --- Properties --- */
    [SerializeField] private bool enableAsteroids = false;
    [SerializeField] [Range(1, 10)] private int batchSize = 1;
    [SerializeField] [Range(0.05f, 2f)] private float minFireInterval = 0.2f;
    [SerializeField] [Range(0.05f, 2f)] private float maxFireInterval = 0.4f;

    // Pathing
    // private bool curveToNextTarget

    /* --- Unity --- */
    private void Start() {
        // Cache these references.
        hitbox = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set up these components.
        hitbox.isTrigger = true;
        spriteRenderer.sortingLayerName = GameRules.Background;

        // Start spawning asteroids.
        if (enableAsteroids && target != null) {
            StartCoroutine(IEShootAsteroid());
        }

    }

    void OnTriggerStay2D(Collider2D collider) {
        CheckAsteroid(collider);
    }

    /* --- Methods --- */
    private void CheckAsteroid(Collider2D collider) {
        Asteroid asteroid = collider.GetComponent<Asteroid>();
        if (asteroid != null) {
            // print("Died");
            float scale = (asteroid.transform.position - transform.position).magnitude / (2 * hitbox.radius); // technically hitbox.radius + asteroid.hitbox.radius
            asteroid.transform.localScale = new Vector3(scale, scale, 1f);

            if (scale  < GameRules.MovementPrecision) {
                EndAsteroid(asteroid);
            }
        }
    }

    private void EndAsteroid(Asteroid asteroid) {
        Destroy(asteroid.gameObject);
    }

    /* --- Coroutines --- */
    // Spawns shooting pixels on a looped timer.
    IEnumerator IEShootAsteroid() {
        yield return new WaitForSeconds(Random.Range(minFireInterval, maxFireInterval));
        for (int i = 0; i < batchSize; i++) {
            Asteroid newAsteroid = Instantiate(asteroidBase.gameObject, transform.position, Quaternion.identity, transform).GetComponent<Asteroid>();
            newAsteroid.gameObject.SetActive(true);
            newAsteroid.transform.localScale = new Vector3(GameRules.MovementPrecision, GameRules.MovementPrecision, 1f);

            // Offset the asteroid to outside the wormholes collision radius so that it doesn't immediately kill itself when it spawns.
            newAsteroid.transform.position += (2 * GameRules.MovementPrecision) * (target.transform.position - transform.position).normalized;
            newAsteroid.target = target;
        }
        yield return StartCoroutine(IEShootAsteroid());
    }

}
