using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Spring {

    public static float DisplacementFactor;
    public static float Taughtness = 0.99f;

    // Nodes
    public PointMass nodeA;
    public PointMass nodeB;

    // Properties
    public float restLength;
    public float stiffness;
    public float damping;

    public Spring(PointMass nodeA, PointMass nodeB, float stiffness, float damping) {
        this.nodeA = nodeA;
        this.nodeB = nodeB;
        this.stiffness = stiffness;
        this.damping = damping;

        // Slightly less than the actual distance in order to keep it taught.
        this.restLength = Vector3.Distance(nodeA.position, nodeB.position) * Taughtness;
    }

    public void Update(float deltaTime) {

        Vector3 deltaPosition = (nodeA.position - nodeB.position);
        float currLength = deltaPosition.magnitude;

        // The springs can only pull, not push
        if (currLength <= restLength) {
            return;
        }

        Vector3 displacement = (deltaPosition / currLength) * (currLength - restLength); // Hooke's Law
        // Vector3 displacement = (deltaPosition / currLength) * (currLength * currLength - restLength * restLength); // Newton's Law
        Vector3 deltaVelocity = nodeA.velocity - nodeB.velocity;
        Vector3 force = DisplacementFactor * stiffness * displacement; // - deltaVelocity * damping;

        nodeA.ApplyForce(-force);
        nodeB.ApplyForce(force);
    }
}
