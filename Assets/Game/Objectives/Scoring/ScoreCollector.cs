/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class ScoreCollector : MonoBehaviour {

    /* --- Components --- */

    /* --- Properties --- */
    public Dictionary<Score, float> scores = new Dictionary<Score, float>();
    public float radius;
    [SerializeField] public int value = 0;

    /* --- Unity --- */
    private void Start() {

    }

    private void Update() {
        UpdateScore();
    }

    private void FixedUpdate() {
        GetScore();
    }

    /* --- Methods --- */
    protected void GetScore() {

        // Find all the necessary colliders.
        Collider2D[] colliders = Physics2D.OverlapCircleAll((Vector2)transform.position, radius);
        for (int i = 0; i < colliders.Length; i++) {
            CheckScore(colliders[i]);
        }

    }

    private void UpdateScore() {
        float fValue = 0;
        foreach (KeyValuePair<Score, float> scoreValue in scores) {
            fValue += scoreValue.Value;
        }
        value = (int)fValue;
    }

    private void CheckScore(Collider2D collider) {
        Score score = collider.GetComponent<Score>();
        if (score != null) {
            Debug.DrawRay(transform.position, score.transform.position - transform.position, Color.yellow);
            if (scores.ContainsKey(score)) {
                // Update the score.
                float newScore = (float)score.value / ((score.transform.position - transform.position).magnitude - score.hitbox.radius);
                newScore = Mathf.Min(score.value, newScore);
                if (newScore > scores[score]) {
                    scores[score] = newScore;
                }
            }
            else {
                // Add a new score.
                scores.Add(score, 0f);
            }

        }
    }

    /* --- Editor --- */
    private void OnDrawGizmos() {
        Gizmos.color = GameRules.White;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

}
