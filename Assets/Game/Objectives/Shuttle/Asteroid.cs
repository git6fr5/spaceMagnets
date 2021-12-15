/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class Asteroid : MonoBehaviour {

    public static float MinRadius = 3f / 16f;
    public static float MaxRadius = 6f / 16f;
    public static float Speed = 2f;

    /* --- Components --- */
    private SpriteRenderer spriteRenderer;
    public AsteroidWormhole target;

    /* --- Properties --- */
    public float radius;
    public Vector2 velocity = Vector2.zero;
    private Vector3 offset;

    /* --- Unity --- */
    private void Start() {
        radius = Random.Range(MinRadius, MaxRadius);
        // offset = velocity 
    }

    private void Update() {

        if (offset == Vector3.zero && velocity != Vector2.zero) {
            int flip = Random.Range(0f, 1f) < 0.5f ? -1 : 1;
            offset = Random.Range(0.05f, AsteroidWormhole.Radius / 2f) * (Quaternion.Euler(0, 0, flip * 90f) * (Vector3)velocity.normalized);
            transform.position += offset;
        }

        Vector3 deltaPosition = (Vector3)velocity * Time.deltaTime;
        transform.position += deltaPosition;

        if ((transform.position - target.transform.position).sqrMagnitude < AsteroidWormhole.Radius * AsteroidWormhole.Radius) {

            // reached end
            Destroy(gameObject);

        }

    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

}
