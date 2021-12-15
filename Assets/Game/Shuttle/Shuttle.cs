using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Shuttle : MonoBehaviour {

    SpriteRenderer spriteRenderer;
    public Sprite[] sprites;

    public float stepInterval;
    public float stepTicks;

    public int stepIndex;
    public int steps;

    public List<Vector3> positions = new List<Vector3>();

    // Start is called before the first frame update
    public void SetPath(ShuttleMass[] points, int firstCollisionIndex) {

        if (firstCollisionIndex == -1 || firstCollisionIndex > points.Length) {
            steps = points.Length;
        }
        else {
            steps = firstCollisionIndex;
        }

        positions = new List<Vector3>();
        for (int i = 0; i < steps; i++) {
            positions.Add(points[i].position);
        }

        float stepSpeed = ShuttlePath.Speed / ShuttlePath.StepDistance;
        stepInterval = 1f / stepSpeed; // I think this is right?
        stepTicks = 0f;

        stepIndex = 0;

        gameObject.SetActive(true);

    }

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() {

        // We possibly want to interpolate between the steps??
        stepTicks += Time.deltaTime;
        if (stepTicks > stepInterval) {
            Step();
            Orient();
            stepTicks -= stepInterval; // Don't reset to 0, this way we don't lose any of the trailing time fragments.
        }

    }

    void Step() {
        stepIndex += 1;
        if (stepIndex < steps) {
            transform.localPosition = positions[stepIndex];
        }
        else {
            Destroy(gameObject);
        }
    }

    void Orient() {

        Vector2 direction = positions[stepIndex] - positions[stepIndex - 1]; // Called after step, so this should be okay.
        float angle = Vector3.SignedAngle(Vector3.right, direction, -Vector3.forward);

        int increments = sprites.Length;
        if (angle < 0f) {
            angle = 360f + angle;
        }

        int index = (int)Mathf.Round((angle / 360f) * increments) % 16;
        print(index);

        spriteRenderer.sprite = sprites[index];

    }

}
