using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointMass {

    // Static
    public static float InvMass;
    public static float Damping = 0.98f;
    public static float SquareThreshold = 0.001f * 0.001f;

    // Properties
    public Vector3 position;
    public Vector3 velocity;
    private Vector3 acceleration;
    public float damping; 

    public PointMass(Vector3 position) {
        this.position = position;
        this.damping = Damping;
    }

    public void ApplyForce(Vector3 force) {
        acceleration += force * InvMass;
    }

    public void Update(float deltaTime) {
        velocity += acceleration * deltaTime;
        position += velocity * deltaTime;
        acceleration = Vector3.zero;
        if (velocity.sqrMagnitude < SquareThreshold) {
            velocity = Vector3.zero;
        }

        velocity *= damping;
        damping = Damping;
    }

}
