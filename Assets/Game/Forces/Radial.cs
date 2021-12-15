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

    ///* --- Properties --- */
    //public float radius;

    ///* --- Override --- */
    //protected override Collider2D[] Area() {

    //    // Find all the necessary colliders.
    //    Collider2D[] colliders = Physics2D.OverlapCircleAll((Vector2)transform.position, radius);

    //    // Return the array.
    //    return colliders;
    //}

    //protected override void ForceToGrid(float force) {
    //    if (Background.Instance?.grid != null) {
    //        Background.Instance.grid.ApplyImplosiveForce(force, transform.position, radius);
    //    }
    //}


    ///* --- Editor --- */
    //private void OnDrawGizmos() {
    //    Gizmos.color = direction == Direction.Pull ? GameRules.Red : GameRules.Blue;
    //    Gizmos.DrawWireSphere(transform.position, radius);
    //}

}