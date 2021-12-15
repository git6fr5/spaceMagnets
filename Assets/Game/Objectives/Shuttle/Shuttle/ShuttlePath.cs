using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ShuttlePath : MonoBehaviour {

    public static float StepDistance = 1f / 16f;
    public static float Speed = 1f;
    public static float Mass = 1f;
    public static float MaxLength = 64f;
    public static float HorizonForce = 100f;
    // for scoring
    public static float MaxDistance = 2f;
    public static float MinDistance = 1f;

    public ShuttleMass[] shuttleMasses;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    public bool refresh;

    public int steps;
    public float stepSpeed;
    public float stepInterval;

    public Force[] forces;
    public Wormhole[] wormholes;

    [Space(5), Header("Score")]

    public int revenue;
    public Dictionary<Revenue, int> revenueDict;
    public Revenue[] revenues;

    public int cost;
    public Cost[] costs;

    public bool reachedStation = false;
    public Station station;

    public int profit;

    [Space(5), Header("Controls")]
    public Vector2 direction;
    public int angleIncrements = 16;
    public int angleIndex = 0;
    public KeyCode clockwiseKey = KeyCode.J;
    public KeyCode counterClockwiseKey = KeyCode.K;

    private void Start() {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        GetPath(Mass, Speed, MaxLength, transform.localPosition, direction);
    }

    private void Update() {
        if (refresh) {
            GetPath(Mass, Speed, MaxLength, transform.localPosition, direction);
        }

        if (Input.GetKeyDown(clockwiseKey)) {
            IncrementDirection(1);
        }
        else if (Input.GetKeyDown(counterClockwiseKey)) {
            IncrementDirection(-1);
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)direction);
    }

    private void IncrementDirection(int increment) {
        angleIndex += increment;
        direction = Quaternion.Euler(0, 0, angleIndex * (360 / angleIncrements)) * Vector2.right;

    }

    public void GetPath(float mass, float speed, float length, Vector3 initialPosition, Vector3 initialDirection) {

        Speed = speed;
        Mass = mass;
        MaxLength = length;

        GetForces();
        GetWormholes();

        steps = (int)Mathf.Floor(MaxLength / StepDistance);
        stepSpeed = Speed / StepDistance;
        stepInterval = 1f / stepSpeed; // I think this is right?

        shuttleMasses = new ShuttleMass[steps];

        bool clearPath = true;
        bool collision = false;

        shuttleMasses[0] = new ShuttleMass(initialPosition, Speed * initialDirection.normalized, Vector3.zero);
        ApplyForces(shuttleMasses[0]);

        for (int i = 1; i < steps; i++) {
            // Simpletic Euler so we increment the velocity first in order to conserve energy.
            Vector3 nextVelocity = shuttleMasses[i - 1].velocity + shuttleMasses[i - 1].acceleration * stepInterval;
            Vector3 nextPosition = shuttleMasses[i - 1].position + nextVelocity * stepInterval;

            nextPosition = CheckWormhole(nextPosition);

            shuttleMasses[i] = new ShuttleMass(nextPosition, nextVelocity, Vector3.zero);

            collision = ApplyForces(shuttleMasses[i]);
            if (collision) {
                clearPath = false;
            }
        }

        revenue = 0;
        cost = 0;
        GetRevenue();
        GetCost();
        reachedStation = GetStation();
        if (!clearPath || !reachedStation) {
            profit = 0;
        }
        else {
            profit = revenue - cost;
        }
    }

    private void OnGUI() {
        List<Vector3> positions = new List<Vector3>();
        positions.Add(shuttleMasses[0].position);

        List<Color> colors = new List<Color>();
        colors.Add(Color.white);

        List<int> indices = new List<int>();

        for (int i = 1; i < shuttleMasses.Length; i++) {
            positions.Add(shuttleMasses[i].position);
            indices.Add(i - 1);
            indices.Add(i);
            colors.Add(Color.white);
        }

        meshFilter.mesh.SetVertices(positions);
        meshFilter.mesh.SetIndices(indices.ToArray(), MeshTopology.Lines, 0);
        meshFilter.mesh.colors = colors.ToArray();
    }

    /* --- Forces --- */
    private void GetForces() {
        forces = (Force[])GameObject.FindObjectsOfType(typeof(Force));
    }

    private void GetWormholes() {
        wormholes = (Wormhole[])GameObject.FindObjectsOfType(typeof(Wormhole));
    }

    private void GetRevenue() {

        revenues = (Revenue[])GameObject.FindObjectsOfType(typeof(Revenue));
        revenueDict = new Dictionary<Revenue, int>();

        for (int i = 0; i < revenues.Length; i++) {
            revenueDict.Add(revenues[i], 0);
            for (int j = 0; j < shuttleMasses.Length; j++) {
                float sqrDistance = (revenues[i].transform.position - (transform.position + shuttleMasses[j].position)).sqrMagnitude;                
                if (sqrDistance < MaxDistance * MaxDistance) {
                    // Can probably optimize this with some smart checking.
                    // score * range(min_dist, max_dist).normalized
                    // therefore at min_dist => 1, at max_dist => 0;
                    float distance = Mathf.Sqrt(sqrDistance);
                    float factor = 1f - ((distance -  MinDistance) / (MaxDistance - MinDistance));
                    factor = Mathf.Clamp(factor, 0f, 1f);
                    int value = (int)Mathf.Floor(revenues[i].value * factor);
                    if (revenueDict[revenues[i]] < value) {
                        revenueDict[revenues[i]] = value;
                    }
                }
            }
        }

        foreach (KeyValuePair<Revenue, int> scorePair in revenueDict) {
            revenue += scorePair.Value;
        }

    }

    private void GetCost() {

        costs = (Cost[])GameObject.FindObjectsOfType(typeof(Cost));
        for (int i = 0; i < costs.Length; i++) {
            cost += costs[i].value;
        }

    }

    private bool GetStation() {

        // Under the assumption that only one exists in the level.
        station = (Station)GameObject.FindObjectOfType(typeof(Station));

        for (int i = 0; i < shuttleMasses.Length; i++) {
            float sqrDistance = (station.transform.position - (transform.position + shuttleMasses[i].position)).sqrMagnitude;
            if (sqrDistance < station.horizon * station.horizon) {
                return true;
            }
        }

        return false;
    }

    private Vector3 CheckWormhole(Vector3 position) {

        for (int i = 0; i < wormholes.Length; i++) {
            if (wormholes[i].targetPoint != null) {
                float sqrDistance = (wormholes[i].transform.position - (transform.position + position)).sqrMagnitude;
                if (sqrDistance < Wormhole.Radius * Wormhole.Radius) {

                    Vector3 displacement = (wormholes[i].transform.position - (transform.position + position));
                    return (wormholes[i].targetPoint.transform.position - transform.position) + displacement;
                }
            }
        }

        return position;
    }

    public bool ApplyForces(ShuttleMass shuttleMass) {

        bool collision = false;
        for (int i = 0; i < forces.Length; i++) {

            Vector3 displacement = (int)forces[i].direction * (forces[i].transform.position - (transform.position + shuttleMass.position));
            float sqrDistance = displacement.sqrMagnitude;
            // The acceleration is used to calculate the NEXT node's position
            // It does not affect this node's position at all and therefore
            // The order in which we apply the forces does not matter.

            if (sqrDistance < forces[i].horizon * forces[i].horizon) {
                shuttleMass.acceleration += HorizonForce * displacement.normalized;
                collision = true; 
            }
            else if (sqrDistance < forces[i].radius * forces[i].radius) {
                shuttleMass.acceleration += forces[i].mass * displacement.normalized / sqrDistance;
            }

        }

        return collision;
    }

}
