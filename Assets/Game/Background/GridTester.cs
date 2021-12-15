using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Background))]
public class GridTester : MonoBehaviour {

    Background background;

    private Vector2 forceOrigin;

    // Start is called before the first frame update
    void Start() {
        background = GetComponent<Background>();
    }

    // Update is called once per frame
    void Update() {

        Vector3 mousePos = (Vector3)(Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (!Input.GetKey(KeyCode.LeftShift)) {
            print("Left Shift Being Held");
            if (Input.GetKeyDown(KeyCode.W)) {
                if (background.grid != null) {
                    print("Explosive");
                    background.grid.ApplyExplosiveForce(10000f, mousePos, 1f);
                }
            }

            if (Input.GetKeyDown(KeyCode.Q)) {
                if (background.grid != null) {
                    print("Implosive");
                    background.grid.ApplyImplosiveForce(10000f, mousePos, 1f);
                }
            }

        }
        else if (Input.GetKey(KeyCode.LeftShift)) {

            if (Input.GetKey(KeyCode.W)) {
                if (background.grid != null) {
                    print("Explosive");
                    background.grid.ApplyExplosiveForce(1000f, mousePos, 5f);
                }
            }

            if (Input.GetKey(KeyCode.Q)) {
                if (background.grid != null) {
                    print("Implosive");
                    background.grid.ApplyImplosiveForce(1000f, mousePos, 5f);
                }
            }
        }

        //if (Input.GetMouseButtonDown(0)) {
        //    forceOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition)
        //}
        //if (Input.GetMouseButtonUp(0)) {

        //}


    }
}
