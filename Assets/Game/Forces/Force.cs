/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* --- Definitions --- */

/* --- Enumerations --- */

/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(CircleCollider2D))]
public class Force : MonoBehaviour {

    public enum Direction {
        Push = -1, Pull = 1
    }

    /* --- Components --- */
    public Direction direction;
    private CircleCollider2D hitbox;

    /* --- Properties --- */
    public float magnitude;
    [SerializeField] private List<Shuttle> shuttles;
    //[SerializeField] private List<Sand> sandBox;
    //[SerializeField] private List<Star> stars;
    // State Variables
    public bool isActive = false;

    /* --- Unity --- */
    // Runs once before the first frame.
    private void Start() {
        hitbox = GetComponent<CircleCollider2D>();
        hitbox.isTrigger = true;
    }

    // Runs once every frame.
    private void FixedUpdate() {
        // Apply the forces.
        if (isActive) {
            // Get the shuttles in the area.
            UpdateCollisions();
            ApplyForces();
        }

    }

    /* --- Methods --- */
    private void UpdateCollisions() {

        Collider2D[] colliders = Area();

        shuttles = new List<Shuttle>();
        //sandBox = new List<Sand>();
        //stars = new List<Star>();

        for (int i = 0; i < colliders.Length; i++) {
            Shuttle shuttle = colliders[i].GetComponent<Shuttle>();
            if (shuttle != null) { shuttles.Add(shuttle); }

            //Sand sand = colliders[i].GetComponent<Sand>();
            //if (sand != null) { sandBox.Add(sand); }

            //Star star = colliders[i].GetComponent<Star>();
            //if (star != null) { stars.Add(star); }
        }

    }

    private void ApplyForces() {

        // Apply a force to each of those shuttles.
        for (int i = 0; i < shuttles.Count; i++) {
            Apply(shuttles[i]);
        }
        ForceToGrid(magnitude * magnitude);
        //// Apply a force to each of those pieces of sand.
        //for (int i = 0; i < sandBox.Count; i++) {
        //    Apply(sandBox[i]);
        //}

        //// Apply a force to each of those pieces of sand.
        //for (int i = 0; i < stars.Count; i++) {
        //    Apply(stars[i]);
        //}

    }

    private void Apply(Shuttle shuttle) {
        // Get the square distance.
        float sqrDistance = (transform.position - shuttle.transform.position).sqrMagnitude;
        Vector2 forceDirection = (transform.position - shuttle.transform.position).normalized;
        // Apply the force (as long as we're not inside the actual object).
        if (sqrDistance > hitbox.radius * hitbox.radius) {
            Vector2 acceleration = (int)direction * magnitude * forceDirection / sqrDistance;
            Vector2 deltaAcceleration = Time.fixedDeltaTime * acceleration;
            shuttle.velocity += deltaAcceleration;
            print("Applying force to a Shuttle.");
        }
    }

    private void Apply(Sand sand) {
        // Get the square distance.
        float sqrDistance = (transform.position - sand.transform.position).sqrMagnitude;
        Vector2 forceDirection = (transform.position - sand.transform.position).normalized;
        // Apply the force (as long as we're not inside the actual object).
        if (sqrDistance > hitbox.radius * hitbox.radius) {
            Vector2 acceleration = (int)direction * magnitude * forceDirection / sqrDistance;
            Vector2 deltaAcceleration = Time.fixedDeltaTime * acceleration;
            sand.velocity += deltaAcceleration;
        }
        // sand.GetComponent<SpriteRenderer>().color = direction == Direction.Pull ? GameRules.Red : GameRules.Blue;

    }

    private void Apply(Star star) {
        // Get the square distance.
        float sqrDistance = (transform.position - star.transform.position).sqrMagnitude;
        Vector2 forceDirection = (transform.position - star.transform.position).normalized;
        // Apply the force (as long as we're not inside the actual object).
        if (sqrDistance > 1f) {
            Vector2 acceleration = (int)direction * magnitude * forceDirection / sqrDistance;
            Vector2 deltaAcceleration = Time.fixedDeltaTime * acceleration;
            star.direction += deltaAcceleration;
        }
    }

    /* --- Virtual --- */
    protected virtual Collider2D[] Area() {
        return new Collider2D[0];
    }

    protected virtual void ForceToGrid(float force) {
        //
    }

}