/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Trailing pixel that moves across the screen.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class StarUI : MonoBehaviour {

    /* --- Components --- */
    SpriteRenderer spriteRenderer;

    /* --- Properties --- */
    private Vector2 direction;
    private float speed;
    private float acceleration;

    /* --- Unity --- */
    void OnEnable() {

        // Set the sprite.
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingLayerName = Random.Range(0f, 1f) < 0.85f ? GameRules.Background : GameRules.Foreground;

        // Set the position.
        int flip = Random.Range(0f, 1f) < 0.5f ? 1 : -1;
        bool verticalAxis = Random.Range(0f, 1f) < 0.5f ? true : false;

        float verticalPosition = (verticalAxis ? flip * GameRules.PixelsVertical : Random.Range(-GameRules.PixelsVertical, GameRules.PixelsVertical)) / (2f * GameRules.PixelsPerUnit);
        float horizontalPosition = (verticalAxis ? Random.Range(-GameRules.PixelsHorizontal, GameRules.PixelsHorizontal) : flip * GameRules.PixelsHorizontal) / (2f * GameRules.PixelsPerUnit);
        transform.position = new Vector3(horizontalPosition, verticalPosition, transform.position.z);

        // Set the movement parameters.
        speed = Random.Range(3f, 5f);
        acceleration = Random.Range(0f, 1f);
        direction = verticalAxis ? flip * Vector2.down : flip * Vector2.left;

        // Set the material parameters.
        spriteRenderer.material.SetFloat("_Speed", speed);
        spriteRenderer.material.SetFloat("_DirectionX", direction.x);
        spriteRenderer.material.SetFloat("_DirectionY", direction.y);

    }

    void FixedUpdate() {
        Accelerate();
        Move();
    }

    /* --- Methods --- */
    private void Move() {
        Vector3 deltaPosition = direction * speed * Time.fixedDeltaTime;
        transform.position = transform.position + deltaPosition;

        // If we've moved past the screen.
        if (transform.position.x > GameRules.PixelsHorizontal / (2f * GameRules.PixelsPerUnit)) {
            Destroy(gameObject);
        }
    }

    private void Accelerate() {
        speed = speed + acceleration * Time.fixedDeltaTime;
        spriteRenderer.material.SetFloat("_Speed", speed);
    }
}
