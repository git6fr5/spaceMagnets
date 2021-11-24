/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ArrowUI : MonoBehaviour {

    /* --- Static Properties --- */
    // Transparency
    public static float Transparency = 0.5f;
    // Size
    public static float MaxScale = 0.75f;
    public static float MinScale = 0.65f;
    public static float Rate = 0.3f;

    /* --- Components --- */
    private SpriteRenderer spriteRenderer;

    /* --- Properties --- */
    public bool startAtMax;
    public int scalar = 1;


    /* --- Unity --- */
    // Runs once before the first frame.
    private void Start() {
        // Cache these references.
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingLayerName = GameRules.Midground;

        // Evaluate these references.
        spriteRenderer.color = new Color(1, 1, 1, 0.5f);

        // Set the size.
        if (startAtMax) {
            transform.localScale = new Vector3(MaxScale, MaxScale, 0f);
        }
        else {
            transform.localScale = new Vector3(MinScale, MinScale, 0f);
        }
    }

    // Runs once per frame.
    private void Update() {
        // If we've gone below the minimum threshold.
        if (scalar == -1 && transform.localScale.x < MinScale) {
            scalar = 1;
        }
        // If we've gone above the maximum threshold.
        else if (scalar == 1 && transform.localScale.x > MaxScale) {
            scalar = -1;
        }
        transform.localScale += Time.deltaTime * scalar * new Vector3(Rate, Rate, 0f);
    }

}
