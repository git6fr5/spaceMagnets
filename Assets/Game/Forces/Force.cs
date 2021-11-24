/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* --- Definitions --- */

/* --- Enumerations --- */

/// <summary>
/// 
/// </summary>
public class Force : MonoBehaviour {

    public enum Direction {
        Push = -1, Pull = 1
    }

    /* --- Components --- */
    public Direction direction;

    /* --- Properties --- */
    public float magnitude;
    [SerializeField] private List<Shuttle> shuttles;

    /* --- Unity --- */
    // Runs once every frame.
    private void FixedUpdate() {
        // Get the shuttles in the area.
        shuttles = Area();
        
        // Apply a force to each of those shuttles.
        for (int i = 0; i < shuttles.Count; i++) {
            Apply(shuttles[i]);
        }
    }

    /* --- Methods --- */
    private void Apply(Shuttle shuttle) {
        // Get the square distance.
        float sqrDistance = (transform.position - shuttle.transform.position).sqrMagnitude;
        Vector2 forceDirection = (transform.position - shuttle.transform.position).normalized;
        // Apply the force (as long as we're not inside the actual object).
        if (sqrDistance > 1f) {
            Vector2 acceleration = (int)direction * magnitude * forceDirection / sqrDistance;
            Vector2 deltaAcceleration = Time.fixedDeltaTime * acceleration;
            shuttle.velocity += deltaAcceleration;
        }
        //if (sqrDistance > 1f) {
        //    Vector2 acceleration = (int)direction * magnitude * forceDirection;
        //    Vector2 deltaAcceleration = Time.fixedDeltaTime * acceleration;
        //    shuttle.velocity += deltaAcceleration;
        //}
    }

    /* --- Virtual --- */
    protected virtual List<Shuttle> Area() {
        return new List<Shuttle>();
    }

}