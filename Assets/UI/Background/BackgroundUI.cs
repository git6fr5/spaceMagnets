/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Arcade style background.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundUI : MonoBehaviour {

    /* --- Components --- */
    [HideInInspector] public SpriteRenderer spriteRenderer;
    public StarUI starUI;

    /* --- Properties --- */
    [SerializeField] [Range(1, 10)] private int batchSize = 5;
    [SerializeField] [Range(0.05f, 2f)] private float fireRate = 1f;

    /* --- Unity --- */
    // Runs once before the first frame.
    void Start() {
        // Cache these references.
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(IEShootStar());
    }

    /* --- Coroutines --- */
    // Spawns shooting pixels on a looped timer.
    IEnumerator IEShootStar() {
        yield return new WaitForSeconds(fireRate);
        for (int i = 0; i < batchSize; i++) {
            StarUI newShootingStar = Instantiate(starUI.gameObject).GetComponent<StarUI>();
            newShootingStar.gameObject.SetActive(true);
        }
        yield return StartCoroutine(IEShootStar());
    }

}
