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
    [SerializeField] private List<Sand> sandBox;
    [SerializeField] private List<Star> stars;
    [SerializeField] private List<Force> forces;

    // State Variables
    public bool isActive = false;
    public bool isMoving = false;
    public bool isOver = false;

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

        // Moving
        if (isOver && Input.GetMouseButtonDown(0)) {
            GameRules.IsEditing = true;
            isMoving = true;
        }
        if (Input.GetMouseButtonUp(0)) {
            // GameRules.Reset();
            GameRules.IsEditing = false;
            isMoving = false;
        }

        hitbox.enabled = !isMoving;
        if (isMoving && GameRules.IsEditing) {
            transform.position = (Vector3)(Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0f, 0f, transform.position.z);
            GameRules.SnapWithinBounds(transform);
        }


        // Activating
        if (isOver && Input.GetMouseButtonDown(1)) {
            // GameRules.Reset();

            // Toggle the activation
            SetActivation(!isActive);
        }

    }

    private void SetActivation(bool newActivation) {
        isActive = newActivation;

        foreach (Transform child in transform) {
            child.gameObject.SetActive(isActive);
        }
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

    private void OnMouseOver() {
        isOver = true;
    }

    private void OnMouseExit() {
        isOver = false;
    }

    /* --- Methods --- */
    private void Move() {
        velocity = velocity * 0.9f;
        Vector2 deltaPosition = velocity * Time.timeScale * Time.fixedDeltaTime;
        transform.position += (Vector3)deltaPosition;
    }

    private void UpdateCollisions() {

        Collider2D[] colliders = Area();

        shuttles = new List<Shuttle>();
        sandBox = new List<Sand>();
        stars = new List<Star>();
        forces = new List<Force>();

        for (int i = 0; i < colliders.Length; i++) {
            Shuttle shuttle = colliders[i].GetComponent<Shuttle>();
            if (shuttle != null) { shuttles.Add(shuttle); }

            Sand sand = colliders[i].GetComponent<Sand>();
            if (sand != null) { sandBox.Add(sand); }

            Star star = colliders[i].GetComponent<Star>();
            if (star != null) { stars.Add(star); }

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
        for (int i = 0; i < sandBox.Count; i++) {
            Apply(sandBox[i]);
        }

        // Apply a force to each of those pieces of sand.
        for (int i = 0; i < stars.Count; i++) {
            // Apply(stars[i]);
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

    private void Apply(Sand sand) {
        // Get the square distance.
        float sqrDistance = (transform.position - sand.transform.position).sqrMagnitude;
        Vector2 forceDirection = (transform.position - sand.transform.position).normalized;
        // Apply the force (as long as we're not inside the actual object).
        if (sqrDistance > 0.5f) {
            Vector2 acceleration = (int)direction * magnitude * forceDirection / sqrDistance;
            Vector2 deltaAcceleration = Time.timeScale * Time.fixedDeltaTime * acceleration;
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
            Vector2 deltaAcceleration = Time.timeScale * Time.fixedDeltaTime * acceleration;
            star.direction += deltaAcceleration;
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