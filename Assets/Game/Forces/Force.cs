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
[RequireComponent(typeof(SpriteRenderer))]
public class Force : MonoBehaviour {

    public enum Direction {
        Push = -1, Pull = 1
    }

    /* --- Components --- */
    [HideInInspector] public SpriteRenderer spriteRenderer;
    private CircleCollider2D hitbox;

    /* --- Properties --- */
    public Vector2 velocity;
    public Direction direction;
    public float magnitude;
    [SerializeField] private List<Shuttle> shuttles;
    [SerializeField] private List<Force> forces;

    // State Variables
    public bool isActive = false;
    // public bool isMoving = false;

    /* --- Unity --- */
    // Runs once before the first frame.
    private void Start() {

        spriteRenderer = GetComponent<SpriteRenderer>();

        hitbox = GetComponent<CircleCollider2D>();
        hitbox.isTrigger = true;

        SetActivation(false);

    }

    // Runs once per frame.
    private void Update() {
        GameRules.SnapWithinBounds(transform);
    }

    // Runs once every frame.
    private void FixedUpdate() {
        // Get the shuttles in the area.
        UpdateCollisions();

        // Apply the forces.
        if (isActive) {
            ApplyForces();
        }

        Move();

    }

    /* --- Methods --- */
    public void SetActivation(bool newActivation) {
        isActive = newActivation;

        foreach (Transform child in transform) {
            child.gameObject.SetActive(isActive);
        }
    }

    private void Move() {
        velocity = velocity * 0.9f;
        Vector2 deltaPosition = velocity * Time.timeScale * Time.fixedDeltaTime;
        transform.position += (Vector3)deltaPosition;
    }

    private void UpdateCollisions() {

        Collider2D[] colliders = Area();

        shuttles = new List<Shuttle>();
        forces = new List<Force>();

        for (int i = 0; i < colliders.Length; i++) {

            Shuttle shuttle = colliders[i].GetComponent<Shuttle>();
            if (shuttle != null) { shuttles.Add(shuttle); }

            Force force = colliders[i].GetComponent<Force>();
            if (force != null) { forces.Add(force); }

        }

    }

    private void ApplyForces() {

        // Apply a force to each of those shuttles.
        for (int i = 0; i < shuttles.Count; i++) {
            Apply(shuttles[i]);
        }

        // Apply a force to each of those pieces of sand.
        for (int i = 0; i < forces.Count; i++) {
            Apply(forces[i]);
        }

    }

    private void Apply(Shuttle shuttle) {
        // Get the square distance.
        float sqrDistance = (transform.position - shuttle.transform.position).sqrMagnitude;
        Vector2 forceDirection = (transform.position - shuttle.transform.position).normalized;
        // Apply the force (as long as we're not inside the actual object).
        if (sqrDistance > 1f) {
            Vector2 acceleration = (int)direction * magnitude * forceDirection / sqrDistance;
            Vector2 deltaAcceleration = Time.timeScale * Time.fixedDeltaTime * acceleration;
            shuttle.velocity += deltaAcceleration;
        }
    }

    private void Apply(Force force) {
        // if (!force.isActive) { return; }

        // Get the square distance.
        float sqrDistance = (transform.position - force.transform.position).sqrMagnitude;
        Vector2 forceDirection = (transform.position - force.transform.position).normalized;

        if (this.direction == force.direction) {
            forceDirection *= -1;
        }

        // Apply the force (as long as we're not inside the actual object).
        if (sqrDistance > 1f) {
            Vector2 acceleration = magnitude * forceDirection / sqrDistance;
            Vector2 deltaAcceleration = Time.timeScale * Time.fixedDeltaTime * acceleration;
            force.velocity += deltaAcceleration;
        }
    }

    /* --- Virtual --- */
    protected virtual Collider2D[] Area() {
        return new Collider2D[0];
    }

}