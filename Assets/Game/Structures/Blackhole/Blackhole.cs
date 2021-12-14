/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Radial))]
[RequireComponent(typeof(Score))]
public class Blackhole : MonoBehaviour {

    /* --- Components --- */
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private Radial forceField;
    private Score score;

    /* --- Properties --- */
    public string blackholeName;
    public int scoreValue;
    public int mass;

    /* --- Unity --- */
    private void Start() {
        // Cache these components
        meshRenderer = GetComponent<MeshRenderer>();
        forceField = GetComponent<Radial>();
        score = GetComponent<Score>();

        // Set up the components
        SetForceField();
        score.value = scoreValue;
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
    }

    // Runs once per frame.
    private void Update() {
        SetForceField();
        if (Background.Instance?.grid != null) {
            Background.Instance.grid.ApplyClockwiseForce(10000f, transform.position, 1f);
        }
    }

    void OnTriggerStay2D(Collider2D collider) {
        CheckDeath(collider);
    }

    /* --- Methods --- */
    private void SetForceField() {
        forceField.isActive = true;
        forceField.direction = Force.Direction.Pull;
        forceField.magnitude = mass * 5;
        forceField.radius = mass;
    }

    /* --- Methods --- */
    private void CheckDeath(Collider2D collider) {
        Shuttle shuttle = collider.GetComponent<Shuttle>();
        if (shuttle != null) {
            print("Died");
            // Add the score
            Destroy(shuttle.gameObject);
        }
    }

}
