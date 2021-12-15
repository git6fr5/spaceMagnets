using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShuttleMass {

    // Properties
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 acceleration;

    public ShuttleMass(Vector3 position, Vector3 velocity, Vector3 acceleration) {
        this.position = position;
        this.velocity = velocity;
        this.acceleration = acceleration;
    }

}
