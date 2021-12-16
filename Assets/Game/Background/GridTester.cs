using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Background))]
public class GridTester : MonoBehaviour {

    Background background;

    private Vector2 forceOrigin;

    public float pressBuffer = 0.3f;
    public float pressTicks = 0f;


    // Start is called before the first frame update
    void Start() {
        background = GetComponent<Background>();
    }

    // Update is called once per frame
    void Update() {
        ClickToInteract();
        MoveToInteract();
        //if (Input.GetMouseButtonDown(0)) {
        //    forceOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition)
        //}
        //if (Input.GetMouseButtonUp(0)) {

        //}


    }

    private Vector3 prevMousePos;
    public float factor = 5000f;

    private void MoveToInteract() {

        Vector3 mousePos = (Vector3)(Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (background.grid != null) {
            print("Explosive");
            background.grid.ApplyCounterClockwiseForce(factor * (mousePos - prevMousePos).sqrMagnitude / (Time.deltaTime * Time.deltaTime), mousePos, 0.5f);
        }

        prevMousePos = mousePos;
    }

    private void ClickToInteract() {
        Vector3 mousePos = (Vector3)(Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(0) && pressTicks <= 0f) {
            if (background.grid != null) {
                print("Explosive");
                background.grid.ApplyCounterClockwiseForce(5000f, mousePos, 0.5f);
            }
            pressTicks = pressBuffer;
        }

        if (Input.GetMouseButton(1) && pressTicks <= 0f) {
            if (background.grid != null) {
                print("Implosive");
                background.grid.ApplyClockwiseForce(5000f, mousePos, 0.5f);
            }
            pressTicks = pressBuffer;
        }

        if (pressTicks > 0f) {
            pressTicks -= Time.deltaTime;
        }
        else {
            pressTicks = 0f;
        }
    }
}
