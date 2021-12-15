/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* --- Definitions --- */

/* --- Enumerations --- */

/// <summary>
/// 
/// </summary>
public class Force : MonoBehaviour {

    public enum Direction {
        Push = -1, Pull = 1
    }

    /* --- Components --- */
    public Direction direction;

    /* --- Properties --- */
    public float mass;
    public float horizon;
    public float radius;

    // State Variables
    public bool isActive = false;
    public bool pulse = false; // purely aesthetic.
    public float pulseInterval = 0.3f;

    private float internalTicks = 0f;
    public float period;

    /* --- Unity --- */
    void Start() {
        if (pulse) {
            StartCoroutine(IEPulseForce(pulseInterval));
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = direction == Direction.Pull ? GameRules.Red : GameRules.Blue;
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.DrawWireSphere(transform.position, horizon);
    }

    void Update() {
        if (!pulse) {
            ApplyForces();
        }

        internalTicks += Time.deltaTime;
        GetComponent<SpriteRenderer>().material.SetFloat("_OffsetY", 2f / 16f * Mathf.Sin(period * Mathf.PI * internalTicks));
    }

    /* --- Methods --- */
    private void ApplyForces() {
        if (Background.Instance?.grid != null) {
            float magnitude = 500f * 0.25f * Mathf.Sqrt(mass / 0.25f);
            Background.Instance.grid.ApplyImplosiveForce(magnitude, transform.position, radius);
            // float horizonForce = Mathf.Min(magnitude * magnitude, 250f * 250f);
            // Background.Instance.grid.ApplyImplosiveForce(magnitude * magnitude, transform.position, horizon);
        }
    }

    /* --- Virtual --- */
    protected virtual Collider2D[] Area() { // Obsolete but a mess to clean up.
        return new Collider2D[0];
    }

    /* --- Coroutines --- */
    private IEnumerator IEPulseForce(float delay) {
        yield return new WaitForSeconds(delay);
        pulse = !pulse;
        StartCoroutine(IEPulseForce(pulseInterval));
        yield return null;
    }


}