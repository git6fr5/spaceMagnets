/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Radial))]
[RequireComponent(typeof(Score))]
public class Blackhole : MonoBehaviour {

    /* --- Components --- */
    private SpriteRenderer spriteRenderer;
    private Radial forceField;
    private Score score;

    /* --- Properties --- */
    public string blackholeName;
    public int scoreValue;
    public int mass;

    /* --- Unity --- */
    private void Start() {
        // Cache these components
        spriteRenderer = GetComponent<SpriteRenderer>();
        forceField = GetComponent<Radial>();
        score = GetComponent<Score>();

        // Set up the components
        SetForceField();
        score.value = scoreValue;
        spriteRenderer.sortingLayerName = GameRules.Midground;
    }

    private void Update() {
        SetForceField();
    }

    /* --- Methods --- */
    private void SetForceField() {
        forceField.isActive = true;
        forceField.isInteractable = true;
        forceField.direction = Force.Direction.Pull;
        forceField.magnitude = mass * 5;
        forceField.radius = Mathf.Sqrt(mass);
    }

}
