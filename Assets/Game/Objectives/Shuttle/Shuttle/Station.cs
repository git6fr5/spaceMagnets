/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* --- Definitions --- */

/* --- Enumerations --- */

/// <summary>
/// 
/// </summary>
public class Station : MonoBehaviour {

    /* --- Properties --- */
    public float horizon;

    // State Variables
    public bool isActive = false;

    /* --- Unity --- */
    private void OnDrawGizmos() {
        Gizmos.color = GameRules.Blue;
        Gizmos.DrawWireSphere(transform.position, horizon);
    }


}