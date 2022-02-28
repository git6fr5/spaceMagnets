using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ShuttlePath : MonoBehaviour {

    public enum RenderMode {
        Continous,
        Point,
        Quad
    }

    public static float StepDistance = 1f / 16f;
    public static float Speed = 2f;
    public static float Mass = 1f;
    public static float MaxLength = 64f;
    public static float HorizonForce = 100f;
    // for scoring
    public static float MaxDistance = 2f;
    public static float MinDistance = 1f;

    public RenderMode renderMode;
    public Gradient lineColorGradient;
    public Gradient collisionColorGradient;
    public Gradient connectedColorGradient;

    public ShuttleMass[] shuttleMasses;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    public bool refresh;

    public int steps;
    public float stepSpeed;
    public float stepInterval;

    private float stepTicks = 0f;
    private int renderOffset = 0;
    private int renderIncrement = 8;

    public Force[] forces;
    public Wormhole[] wormholes;

    [Space(5), Header("Score")]

    public int revenue;
    public Dictionary<Revenue, int> revenueDict;
    public Revenue[] revenues;

    public int cost;
    public Cost[] costs;

    public int firstCollisionIndex = -1;
    public bool reachedStation = false;
    public Station station;

    public int profit;

    [Space(5), Header("Controls")]
    public Shuttle shuttle;
    public Vector2 direction;
    public int angleIncrements = 16;
    public int angleIndex = 0;
    public float pressTurnedBuffer;
    public float holdTurnedBuffer;
    [SerializeField] private float turnedTicks = 0f;
    public KeyCode launchKey = KeyCode.Space;
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

        if (Input.GetKeyDown(launchKey)) {
            Launch();
        }


        if (Input.GetKeyDown(clockwiseKey)) {
            IncrementDirection(1);
            turnedTicks = pressTurnedBuffer;
        }
        else if (Input.GetKeyDown(counterClockwiseKey)) {
            IncrementDirection(-1);
            turnedTicks = pressTurnedBuffer;
        }

        if (Input.GetKey(clockwiseKey) && turnedTicks <= 0f) {
            IncrementDirection(1);
            turnedTicks = holdTurnedBuffer;
        }
        else if (Input.GetKey(counterClockwiseKey) && turnedTicks <= 0f) {
            IncrementDirection(-1);
            turnedTicks = holdTurnedBuffer;
        }

        if (turnedTicks > 0f) {
            turnedTicks -= Time.deltaTime;
        }
        else {
            turnedTicks = 0f;
        }

        if (reachedStation) {
            stepTicks += Time.deltaTime;
        }
        if (stepTicks > stepInterval) {
            renderOffset = (renderOffset + 1) % renderIncrement;
            stepTicks -= stepInterval; // Don't reset to 0, this way we don't lose any of the trailing time fragments.
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)direction);
    }

    private void Launch() {
        Shuttle newShuttle = Instantiate(shuttle.gameObject, transform.position, Quaternion.identity, transform).GetComponent<Shuttle>();
        newShuttle.SetPath(shuttleMasses, firstCollisionIndex);
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
        GetStation();

        steps = (int)Mathf.Floor(MaxLength / StepDistance);
        stepSpeed = Speed / StepDistance;
        stepInterval = 1f / stepSpeed; // I think this is right?

        shuttleMasses = new ShuttleMass[steps];

        reachedStation = false;
        firstCollisionIndex = -1;
        bool clearPath = true;
        bool collision = false;

        shuttleMasses[0] = new ShuttleMass(initialPosition, Speed * initialDirection.normalized, Vector3.zero);
        ApplyForces(shuttleMasses[0]);

        for (int i = 1; i < steps; i++) {
            // Simpletic Euler so we increment the velocity first in order to conserve energy.
            Vector3 nextVelocity = shuttleMasses[i - 1].velocity + shuttleMasses[i - 1].acceleration * stepInterval;
            Vector3 nextPosition = shuttleMasses[i - 1].position + nextVelocity * stepInterval;

            nextPosition = CheckWormhole(nextPosition);
            nextVelocity = CheckStation(nextPosition, nextVelocity);

            shuttleMasses[i] = new ShuttleMass(nextPosition, nextVelocity, Vector3.zero);
            if (!reachedStation) {
                collision = ApplyForces(shuttleMasses[i]);
            }
            if (collision && firstCollisionIndex == -1) {
                firstCollisionIndex = i;
                clearPath = false;
            }

            // if (firstCollisionIndex )
        }

        if (firstCollisionIndex != -1) {
            reachedStation = false;
        }

        revenue = 0;
        cost = 0;
        GetRevenue();
        GetCost();
        // reachedStation =  // we can do this more efficently.
        if (!clearPath || !reachedStation) {
            profit = 0;
        }
        else {
            profit = revenue - cost;
        }
    }

    private void FixedUpdate() {

        List<Vector3> positions = new List<Vector3>();
        List<Color> colors = new List<Color>();
        List<int> indices = new List<int>();

        int steps = firstCollisionIndex == -1 ? shuttleMasses.Length : firstCollisionIndex;
        Gradient gradient = firstCollisionIndex != -1 ? collisionColorGradient : (reachedStation ? connectedColorGradient : lineColorGradient);
        int offset = reachedStation ? renderOffset : 0;
        float size = (reachedStation || firstCollisionIndex != -1) ? 2f / 16f : 1f / 16f; // pixelWidth / GameRules.PixelsPerUnit;
        int increment = (reachedStation || firstCollisionIndex != -1) ? renderIncrement : 3;

        switch (renderMode) {
            case (RenderMode.Continous):
                positions.Add(shuttleMasses[0].position);
                colors.Add(Color.white);


                for (int i = 1; i < shuttleMasses.Length; i++) {
                    positions.Add(shuttleMasses[i].position);
                    indices.Add(i - 1);
                    indices.Add(i);
                    colors.Add(Color.white);
                }

                meshFilter.mesh.SetVertices(positions);
                meshFilter.mesh.SetIndices(indices.ToArray(), MeshTopology.Lines, 0);
                meshFilter.mesh.colors = colors.ToArray();
                break;
            case (RenderMode.Point):
                // can't be assed
                break;
            case (RenderMode.Quad):
                float normalizedSpeed = 0f;
                Color col = Color.white;
                int index = 0;
                for (int i = offset; i < steps; i+= increment) {

                    //if (reachedStation && Background.Instance?.grid != null) {
                    //    float magnitude = 100000f;
                    //    Background.Instance.grid.ApplyDirectedForce(shuttleMasses[i].position - shuttleMasses[i - 1].position, transform.position + shuttleMasses[i].position, 1f);
                    //}

                    positions.Add(shuttleMasses[i].position);
                    positions.Add(shuttleMasses[i].position + new Vector3(size, 0f, 0f));
                    positions.Add(shuttleMasses[i].position + new Vector3(0f, size, 0f));
                    positions.Add(shuttleMasses[i].position + new Vector3(size, size, 0f));

                    indices.Add(4 * index + 0);
                    indices.Add(4 * index + 1);
                    indices.Add(4 * index + 3);

                    indices.Add(4 * index + 0);
                    indices.Add(4 * index + 2);
                    indices.Add(4 * index + 3);

                    // normalizedSpeed = 0f; // Mathf.Min(1f, shuttleMasses[i].velocity.sqrMagnitude / 5f);
                    if (reachedStation) {
                        normalizedSpeed = ((float)renderOffset / (float)increment);
                    }
                    else if (firstCollisionIndex != -1) {
                        normalizedSpeed = ((float)i / (float)firstCollisionIndex);
                    }

                    col = gradient.Evaluate(normalizedSpeed);
                    colors.Add(col);
                    colors.Add(col);
                    colors.Add(col);
                    colors.Add(col);

                    index += 1;
                }

                meshFilter.mesh.SetVertices(positions);
                meshFilter.mesh.SetIndices(indices.ToArray(), MeshTopology.Triangles, 0);
                meshFilter.mesh.colors = colors.ToArray();
                break;


        }
         
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

    private void GetStation() {

        // Under the assumption that only one exists in the level.
        station = (Station)GameObject.FindObjectOfType(typeof(Station));

        //for (int i = 0; i < shuttleMasses.Length; i++) {
        //    float sqrDistance = (station.transform.position - (transform.position + shuttleMasses[i].position)).sqrMagnitude;
        //    if (sqrDistance < station.horizon * station.horizon) {
        //        return true;
        //    }
        //}

        //return false;
    }

    private Vector3 CheckStation(Vector3 localPosition, Vector3 nextVelocity) {

        float sqrDistance = (station.transform.position - (transform.position + localPosition)).sqrMagnitude;
        if (sqrDistance < station.horizon * station.horizon) {
            reachedStation = true;
            return nextVelocity.magnitude * (station.transform.position - (transform.position + localPosition)).normalized;
        }
        return nextVelocity;

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
                if (firstCollisionIndex == -1) {
                    if (Background.Instance?.grid != null) {
                        float magnitude = 100000f;
                        Background.Instance.grid.ApplyExplosiveForce(magnitude, (transform.position + shuttleMass.position), 0.75f);
                    }
                }
            }
            else if (sqrDistance < forces[i].radius * forces[i].radius) {
                shuttleMass.acceleration += forces[i].mass * displacement.normalized / sqrDistance;
            }

        }

        return collision;
    }

}
