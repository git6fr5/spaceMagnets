/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class Asteroid : MonoBehaviour {

    public static float MinRadius = 3f / 16f;
    public static float MaxRadius = 6f / 16f;

    public static float MinTorque = 2f;
    public static float MaxTorque = 40f;

    /* --- Components --- */
    private SpriteRenderer spriteRenderer;
    public AsteroidWormhole target;
    public Sprite[] sprites;

    /* --- Properties --- */
    public float radius;

    private int flip;
    private float offset;
    private float torque;

    public List<Vector3> positions;

    public float stepInterval;
    public float stepTicks;

    public int stepIndex;
    public int steps;

    /* --- Unity --- */
    public void SetPath(List<Vector3> pathPoints) {

        radius = Random.Range(0f, 1f) < 0.3f ? MinRadius : Random.Range(MinRadius, MaxRadius);
        torque = Random.Range(MinTorque, MaxTorque);

        positions = pathPoints;
        steps = positions.Count;

        flip = Random.Range(0f, 1f) < 0.5f ? -1 : 1;
        offset = Random.Range(AsteroidWormhole.Radius / 4f, AsteroidWormhole.Radius / 2f);

        float stepSpeed = AsteroidPath.Speed / AsteroidPath.StepDistance;
        stepInterval = 1f / stepSpeed; // I think this is right?
        stepTicks = 0f;

        stepIndex = 0;

        gameObject.SetActive(true);

    }

    void Start() {

        spriteRenderer = GetComponent<SpriteRenderer>();
        int index = (int)Mathf.Floor((sprites.Length * (radius - MinRadius - 0.001f) / (MaxRadius - MinRadius)));
        if (index < 0) {
            index = 0;
        }
        index = index % 1;
        spriteRenderer.sprite = sprites[index];

    }

    private void Update() {

        // We possibly want to interpolate between the steps??
        stepTicks += Time.deltaTime;
        if (stepTicks > stepInterval) {
            Step();
            stepTicks -= stepInterval; // Don't reset to 0, this way we don't lose any of the trailing time fragments.
        }

        ApplyForces();

    }

    private void ApplyForces() {
        if (Background.Instance?.grid != null) {
            Background.Instance.grid.ApplyExplosiveForce(radius * 16f * 10000f, transform.position, radius);
        }
    }

    private void Step() {

        stepIndex += 1;
        if (stepIndex < steps) {

            transform.position = positions[stepIndex];

            Vector2 direction = (positions[stepIndex] - positions[stepIndex - 1]).normalized;
            Vector2 offsetVector = offset * (Quaternion.Euler(0, 0, flip * 90f) * direction);
            transform.position += (Vector3)offsetVector;

            transform.eulerAngles += Vector3.forward * flip * torque * stepInterval;


        }
        else {
            Destroy(gameObject);
        }


    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

}
