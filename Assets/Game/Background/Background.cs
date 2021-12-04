/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Arcade style background.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class Background : MonoBehaviour {

    /* --- Components --- */
    [HideInInspector] public SpriteRenderer spriteRenderer;
    public Star star;
    public Sand sand; // Eventually will a sandUI

    /* --- Properties --- */
    [Space(5)] [Header("Sand Grid")]
    [SerializeField] [Range(0, 100)] private int verticalPrecision = 5;
    [SerializeField] [Range(0, 100)] private int horizontalPrecision = 5;
    [Space(5)] [Header("Shooting Stars")]
    [SerializeField] [Range(1, 10)] private int batchSize = 5;
    [SerializeField] [Range(0.05f, 2f)] private float fireRate = 1f;

    /* --- Unity --- */
    // Runs once before the first frame.
    void Start() {
        // Cache these references.
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(IEShootStar());

        // Sand grid
        SandGrid();
    }

    private void Update() {
        if (GameRules.IsEditing) {
            spriteRenderer.color = Color.green;
            Time.timeScale = 0.5f; // Should I be doing this here?
        }
        else {
            spriteRenderer.color = Color.white;
            Time.timeScale = 1f;
        }
    }

    /* --- Methods --- */
    private void SandGrid() {

        Vector2 offset = new Vector2(GameRules.PixelsHorizontal, GameRules.PixelsVertical) / (2f * GameRules.PixelsPerUnit) - new Vector2(0.25f, 0.25f);
        float scaleX =  ((float)(GameRules.PixelsHorizontal / GameRules.PixelsPerUnit)) / (float)horizontalPrecision; // Distance We Need To Cover / Amount
        float scaleY = ((float)(GameRules.PixelsVertical / GameRules.PixelsPerUnit)) / (float)verticalPrecision; // Distance We Need To Cover / Amount

        for (int i = 0; i < verticalPrecision; i++) {
            for (int j = 0; j < horizontalPrecision; j++) {

                Sand newSand = Instantiate(sand.gameObject, new Vector2(j * scaleX, i * scaleY) - offset, Quaternion.identity, transform).GetComponent<Sand>();
                newSand.gameObject.SetActive(true);

            }
        }

    }

    /* --- Coroutines --- */
    // Spawns shooting pixels on a looped timer.
    IEnumerator IEShootStar() {
        yield return new WaitForSeconds(fireRate);
        for (int i = 0; i < batchSize; i++) {
            Star newShootingStar = Instantiate(star.gameObject).GetComponent<Star>();
            newShootingStar.transform.parent = transform;
            newShootingStar.gameObject.SetActive(true);
        }
        yield return StartCoroutine(IEShootStar());
    }

}
