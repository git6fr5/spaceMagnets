/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(Force))]
[RequireComponent(typeof(SpriteRenderer))]
public class ForceUI : MonoBehaviour {

    /* --- Component --- */
    private Force force;
    private SpriteRenderer spriteRenderer;

    /* --- Properties --- */
    public bool prevActive;

    /* --- Unity --- */
    private void Start() {
        // Cache these references.
        force = GetComponent<Force>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set up these components.
        spriteRenderer.sortingLayerName = GameRules.Midground;
        Color color = Color.white * 0.85f + (force.direction == Force.Direction.Pull ? GameRules.Red : GameRules.Blue);
        spriteRenderer.material.SetColor("_OutlineColor", color);

        Activate();

    }

    private void Update() {

        if (prevActive != force.isActive) {
            Activate();
        }
        prevActive = force.isActive;
    }

    private void Activate() {
        foreach (Transform child in transform) {
            child.gameObject.SetActive(force.isActive);
        }
        float width = force.isActive ? 1f / GameRules.PixelsPerUnit : 0f;
        spriteRenderer.material.SetFloat("_OutlineWidth", width);
    }


}
