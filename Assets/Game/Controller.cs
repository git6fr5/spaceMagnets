using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Controller : MonoBehaviour {

    public int forceIndex;
    public Force[] forces;
    public Spawner spawner;

    /* --- Controls --- */
    public int speed;
    public KeyCode resetKey = KeyCode.P;
    public KeyCode spawnKey = KeyCode.Space;
    private KeyCode prevKey = KeyCode.J;
    public KeyCode slowMoKey = KeyCode.K;
    private KeyCode nextKey = KeyCode.L;

    /* --- Unity --- */
    private void Start() {
        GetSpawner();
        GetAllForces();
    }

    private void Update() {

        // Movement
        if (forceIndex >= 0 && forces[forceIndex] != null) {
            if (!forces[forceIndex].isActive) {
                forces[forceIndex].SetActivation(true);
            }

            Vector2 deltaVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * speed * Time.deltaTime;
            forces[forceIndex].velocity += deltaVelocity;
        }

        // Cycling
        if (Input.GetKeyDown(nextKey)) {
            forces[forceIndex].SetActivation(false);
            forceIndex = (forceIndex + 1) % forces.Length;
        }
        else if (Input.GetKeyDown(prevKey)) {
            forces[forceIndex].SetActivation(false);
            if (forceIndex == 0) { forceIndex = forces.Length; }
            forceIndex = (forceIndex - 1) % forces.Length;
        }

        // Time
        if (Input.GetKey(slowMoKey)) {
            Time.timeScale = 0.5f;
        }
        else {
            Time.timeScale = 1f;
        }

        // Spawning
        if (Input.GetKeyDown(spawnKey) && spawner.spawnTicks == 0f) {
            spawner.Spawn();
        }

        // Resetting
        if (Input.GetKeyDown(resetKey) || forceIndex == -1) {
            Reset();
        }
    }

    /* --- Static Methods --- */
    private void GetSpawner() {
        spawner = (Spawner)GameObject.FindObjectOfType(typeof(Spawner));
    }

    private void GetAllForces() {
        forces = (Force[])GameObject.FindObjectsOfType(typeof(Force));
        Array.Sort<Force>(forces, new Comparison<Force>((forceA, forceB) => Order(forceA, forceB)));
        if (forces.Length > 0) {
            forceIndex = 0;
        }
        else {
            forceIndex = -1;
        }
    }

    private void Reset() {

        ClearShuttles();
        GetAllForces();
        GetSpawner();

    }

    private void ClearShuttles() {
        Shuttle[] shuttles = (Shuttle[])GameObject.FindObjectsOfType(typeof(Shuttle));
        for (int i = 0; i < shuttles.Length; i++) {
            shuttles[i].Explode();
        }
    }

    /* --- System --- */
    // Compare the depth of the meshes.
    public static int Order(Force forceA, Force forceB) {
        if ((int)forceA.transform.position.y > (int)forceB.transform.position.y) {
            return -1;
        }
        else if ((int)forceA.transform.position.x < (int)forceB.transform.position.x) {
            return -1;
        }
        return 1;
    }

}
