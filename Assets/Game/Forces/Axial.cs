using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axial : Force
{
    /* --- Properties --- */
    public float length;

    /* --- Override --- */
    protected override Collider2D[] Area() {

        // Find all the necessary colliders.
        Collider2D[] vertical = Physics2D.OverlapBoxAll(transform.position, new Vector2(1f, length * 2f), 0f);
        Collider2D[] horizontal = Physics2D.OverlapBoxAll(transform.position, new Vector2(length * 2f, 1f), 0f);

        Collider2D[] colliders = new Collider2D[vertical.Length + horizontal.Length];
        vertical.CopyTo(colliders, 0);
        horizontal.CopyTo(colliders, vertical.Length);

        // Return the array.
        return colliders;
    }


    /* --- Editor --- */
    private void OnDrawGizmos() {
        Gizmos.color = direction == Direction.Pull ? GameRules.Red : GameRules.Blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(1f, length * 2f, 1f));
        Gizmos.DrawWireCube(transform.position, new Vector3(length * 2f, 1f, 1f));
    }
}
