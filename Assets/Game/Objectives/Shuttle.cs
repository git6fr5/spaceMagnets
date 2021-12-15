/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Shuttle : MonoBehaviour {

    /* --- Components --- */
    public ShuttlePath path;

    /* --- Properties --- */
    public float speed = 5f;

    /* --- Unity --- */
    private void Start() {
    }

    private void Update() {
        
    }

    

    /* --- Methods --- */

    /* --- Editor --- */
    private void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        // Gizmos.DrawLine(transform.position, transform.position + (Vector3)velocity);
    }

}