/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class AsteroidWormhole : MonoBehaviour {

    public static float Radius = 6f / 16f;
    public static float MinFireInterval = 0.2f;
    public static float MaxFireInterval = 0.205f;

    /* --- Components --- */
    private SpriteRenderer spriteRenderer;
    public AsteroidWormhole target;
    public Asteroid asteroidBase;

    private AsteroidPath.BezierPath path;

    /* --- Properties --- */
    [SerializeField] private bool enableAsteroids = false;
    [SerializeField] [Range(1, 10)] private int batchSize = 1;

    // Pathing
    // private bool curveToNextTarget

    /* --- Unity --- */
    private void Start() {
        // Start spawning asteroids.
        if (enableAsteroids && target != null) {
            path = transform.parent.GetComponent<AsteroidPath>().path;
            StartCoroutine(IEShootAsteroid());
        }

    }

    /* --- Coroutines --- */
    // Spawns shooting pixels on a looped timer.
    IEnumerator IEShootAsteroid() {
        yield return new WaitForSeconds(Random.Range(MinFireInterval, MaxFireInterval));
        for (int i = 0; i < batchSize; i++) {
            Asteroid newAsteroid = Instantiate(asteroidBase.gameObject, transform.position, Quaternion.identity, transform).GetComponent<Asteroid>();
            newAsteroid.gameObject.SetActive(true);

            // Offset the asteroid to outside the wormholes collision radius so that it doesn't immediately kill itself when it spawns.
            newAsteroid.SetPath(path.pathPoints);
        }
        yield return StartCoroutine(IEShootAsteroid());
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Radius);
        if (target != null && enableAsteroids) {
            // Gizmos.DrawLine(transform.position, target.transform.position);
        }
    }

}
