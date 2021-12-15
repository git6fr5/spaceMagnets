/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Wormhole : MonoBehaviour {

    public static float Radius = 0.25f;
    public static float VelocityFactor = 0.95f;

    public enum WormholeRotation {
        Clockwise,
        Counterclockwise
    }

    /* --- Components --- */
    private SpriteRenderer spriteRenderer;
    public WormholeRope rope;

    /* --- Properties --- */
    public WormholeRotation rotation;
    public Wormhole originPoint;
    public Wormhole targetPoint;
    public string wormholeName;

    public Vector2 velocity;
     // either make this static - or make it so that if the interactable component is moving, then this has 0 or less value.

    /* --- Unity --- */
    private void Start() {

        rope = Instantiate(rope.gameObject, transform.position, Quaternion.identity, transform).GetComponent<WormholeRope>();
        rope.gameObject.SetActive(false);

        if (targetPoint != null) {
            rope.gameObject.SetActive(true);
            rope.startpoint = transform;
            rope.endpoint = targetPoint.transform;
            rope.ropeLength = 3f;
            rope.ropeWidth = 0.1f;
        }

    }

    private void Update() {

        velocity *= 0.95f;
        if (velocity.sqrMagnitude < 0.001f * 0.001f) {
            velocity = Vector3.zero;
        }
        Vector2 deltaPosition = velocity * Time.deltaTime; // Is this the correct delta time to use?
        transform.position += (Vector3)deltaPosition;

        if (originPoint != null) {
            OriginBounds();
        }
        if (targetPoint != null) {
            TargetBounds();
        }

        // ApplyForces();
    }

    private void ApplyForces() {
        if (Background.Instance?.grid != null && rotation == WormholeRotation.Clockwise) {
            Background.Instance.grid.ApplyClockwiseForce(1000f, transform.position, 1f);
        }
        else {
            Background.Instance.grid.ApplyCounterClockwiseForce(1000f, transform.position, 1f);
        }
    }

    private void OriginBounds() {

        float distance = (transform.position - originPoint.transform.position).magnitude;

        if (distance  > originPoint.rope.ropeLength) {
            velocity -= (Vector2)(transform.position - originPoint.transform.position).normalized * (distance / originPoint.rope.ropeLength) * VelocityFactor;
        }


    }

    private void TargetBounds() {

        float distance = (targetPoint.transform.position - transform.position).magnitude;

        if (distance > rope.ropeLength) {
            velocity += (Vector2)(targetPoint.transform.position - transform.position).normalized * (distance / rope.ropeLength) * VelocityFactor;
        }


    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }

}
