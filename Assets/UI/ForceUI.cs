/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(Force))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class ForceUI : MonoBehaviour {

    /* --- Component --- */
    private Force force;
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D hitbox;

    /* --- Properties --- */
    public bool isActive = false;
    public bool isMoving = false;
    public bool isOver = false;

    /* --- Unity --- */
    private void Start() {
        // Cache these references.
        hitbox = GetComponent<CircleCollider2D>();
        force = GetComponent<Force>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set up these components.
        spriteRenderer.sortingLayerName = GameRules.Midground;
        Color color = Color.white * 0.85f + (force.direction == Force.Direction.Pull ? GameRules.Red : GameRules.Blue);
        spriteRenderer.material.SetColor("_OutlineColor", color);
        hitbox.isTrigger = true;

        Activate();

    }

    // Runs once per frame.
    private void Update() {

        // Moving
        if (isOver && Input.GetMouseButtonDown(0)) {
            GameRules.IsEditing = true;
            isMoving = true;
        }
        if (Input.GetMouseButtonUp(0)) {
            GameRules.Reset();
            GameRules.IsEditing = false;
            isMoving = false;
        }

        hitbox.enabled = !isMoving;
        if (isMoving && GameRules.IsEditing) {
            transform.position = (Vector3)(Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0f, 0f, transform.position.z);
            ScreenBounds();
        }


        // Activating
        if (isOver && Input.GetMouseButtonDown(1)) {
            GameRules.Reset();
            isActive = !isActive;
            Activate();
        }
    }

    private void ScreenBounds() {

        float x = transform.position.x;
        float y = transform.position.y;

        if (transform.position.x > GameRules.PixelsHorizontal / (2f * GameRules.PixelsPerUnit)) {
            x = GameRules.PixelsHorizontal / (2f * GameRules.PixelsPerUnit);
        }
        else if (transform.position.x < -GameRules.PixelsHorizontal / (2f * GameRules.PixelsPerUnit)) {
            x = -GameRules.PixelsHorizontal / (2f * GameRules.PixelsPerUnit);
        }

        if (transform.position.y > GameRules.PixelsVertical / (2f * GameRules.PixelsPerUnit)) {
            y = GameRules.PixelsVertical / (2f * GameRules.PixelsPerUnit);
        }
        else if (transform.position.y < -GameRules.PixelsVertical / (2f * GameRules.PixelsPerUnit)) {
            y = -GameRules.PixelsVertical / (2f * GameRules.PixelsPerUnit);
        }

        transform.position = new Vector2(x, y);

    }

    private void OnMouseOver() {
        print("Over");
        isOver = true;
    }

    private void OnMouseExit() {
        print("Out");
        isOver = false;
    }

    /* --- Methods --- */
    private void Activate() {
        force.enabled = isActive;
        foreach (Transform child in transform) {
            child.gameObject.SetActive(isActive);
        }
        float width = isActive ? 0.025f : 0f;
        spriteRenderer.material.SetFloat("_OutlineWidth", width);
    }


}
