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
            isMoving = true;
        }
        if (Input.GetMouseButtonUp(0)) {
            isMoving = false;
        }

        hitbox.enabled = !isMoving;
        if (isMoving) {
            transform.position = (Vector3)(Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0f, 0f, transform.position.z);
            // ScreenBounds();
        }


        // Activating
        if (isOver && Input.GetMouseButtonDown(1)) {
            if (GetComponent<Cost>() != null) {
                Destroy(gameObject);
            }
            else {
                Destroy(transform.parent.gameObject);
            }
        }
    }

    private void OnMouseOver() {
        isOver = true;
    }

    private void OnMouseExit() {
        isOver = false;
    }

    private void ScreenBounds() {

        Vector2 screenPos = Camera.main.WorldToScreenPoint(transform.position);

        if (screenPos.x > 1f) {
            screenPos.x = 1f;
        }
        else if (screenPos.x < 0f) {
            screenPos.x = 0f;
        }

        if (screenPos.y > 1f) {
            screenPos.y = 1f;
        }
        else if (screenPos.y < 0f) {
            screenPos.y = 0f;
        }

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        transform.position = new Vector3(worldPos.x, worldPos.y, 0f);

    }
}
