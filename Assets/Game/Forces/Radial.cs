/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* --- Definitions --- */

/* --- Enumerations --- */

/// <summary>
/// 
/// </summary>
public class Radial : Force {

    /* --- Properties --- */
    public float radius;

    /* --- Override --- */
    protected override List<Shuttle> Area() {

        // Instantiate a new list of shuttles.
        List<Shuttle> shuttles = new List<Shuttle>();

        // Find all the necessary shuttles.
        Collider2D[] colliders = Physics2D.OverlapCircleAll((Vector2)transform.position, radius);
        for (int i = 0; i < colliders.Length; i++) {
            Shuttle shuttle = colliders[i].GetComponent<Shuttle>();
            if (shuttle != null) { shuttles.Add(shuttle); }
        }

        // Return the list.
        return shuttles;
    }


    /* --- Editor --- */
    private void OnDrawGizmos() {
        Gizmos.color = direction == Direction.Pull ? GameRules.Red : GameRules.Blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

}