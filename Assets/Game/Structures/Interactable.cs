using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Interactable : MonoBehaviour
{

    private CircleCollider2D hitbox;

    public bool isInteractable = false;
    public bool isMoving = false;
    public bool isOver = false;

    // Start is called before the first frame update
    void Start() {
        hitbox = GetComponent< CircleCollider2D>();
    }

    // Update is called once per frame
    void Update() {
        if (isInteractable) {
            Interact();
        }
    }

    private void Interact() {
        // Moving
        if (isOver && Input.GetMouseButtonDown(0)) {
            // GameRules.IsEditing = true;
            isMoving = true;
        }
        if (Input.GetMouseButtonUp(0)) {
            // GameRules.Reset();
            // GameRules.IsEditing = false;
            isMoving = false;
        }

        hitbox.enabled = !isMoving;
        if (isMoving) {
            transform.position = (Vector3)(Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0f, 0f, transform.position.z);
            ScreenBounds();
        }


        // Activating
        if (isOver && Input.GetMouseButtonDown(1)) {
            // GameRules.Reset();
            // isActive = !isActive;
        }
    }

    private void OnMouseOver() {
        isOver = true;
    }

    private void OnMouseExit() {
        isOver = false;
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
}
