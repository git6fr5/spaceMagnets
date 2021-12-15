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

    /* --- Unity --- */
    private void OnDrawGizmos() {
        Gizmos.color = direction == Direction.Pull ? GameRules.Red : GameRules.Blue;
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.DrawWireSphere(transform.position, horizon);
    }

    /* --- Methods --- */
    private void ApplyForces() {
        if (Background.Instance?.grid != null) {
            Background.Instance.grid.ApplyImplosiveForce(mass * mass, transform.position, Mathf.Sqrt(mass));
        }
    }

    /* --- Virtual --- */
    protected virtual Collider2D[] Area() { // Obsolete but a mess to clean up.
        return new Collider2D[0];
    }


}