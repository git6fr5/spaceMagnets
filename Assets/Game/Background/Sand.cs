using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sand : MonoBehaviour {

    // ----
    public Vector2 origin;
    public Vector2 velocity;

    // Start is called before the first frame update
    void Start() {
        origin = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate() {

        float factor = 2f;
        velocity = velocity * 0.9f;

        Vector2 displacement = origin - (Vector2)transform.position;
        if (displacement.magnitude < 0.1f) {
            displacement = displacement.normalized * 0.1f;
        }
        Vector2 reactionVelocity = displacement.normalized * Mathf.Pow(displacement.magnitude, factor);
        Vector2 deltaPosition = (velocity + reactionVelocity) * Time.timeScale * Time.fixedDeltaTime;
        transform.position += (Vector3)(deltaPosition);
    }
}
