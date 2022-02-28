using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
public class AsteroidPath : MonoBehaviour
{

	public class BezierPath {

		public List<Vector3> pathPoints;
		private int segments;
		public int pointCount;

		public BezierPath() {
			pathPoints = new List<Vector3>();
			pointCount = 128;
		}

		public void DeletePath() {
			pathPoints.Clear();
			// pathPoints = new List<Vector3>();
		}

		Vector3 BezierPathCalculation(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
			float tt = t * t;
			float ttt = t * tt;
			float u = 1.0f - t;
			float uu = u * u;
			float uuu = u * uu;

			Vector3 B = new Vector3();
			B = uuu * p0;
			B += 3.0f * uu * t * p1;
			B += 3.0f * u * tt * p2;
			B += ttt * p3;

			return B;
		}

		public void CreateCurve(List<Vector3> controlPoints) {
			segments = controlPoints.Count / 3;

			for (int s = 0; s < controlPoints.Count - 3; s += 3) {
				Vector3 p0 = controlPoints[s];
				Vector3 p1 = controlPoints[s + 1];
				Vector3 p2 = controlPoints[s + 2];
				Vector3 p3 = controlPoints[s + 3];

				if (s == 0) {
					pathPoints.Add(BezierPathCalculation(p0, p1, p2, p3, 0.0f));
				}

				for (int p = 0; p < (pointCount / segments); p++) {
					float t = (1.0f / (pointCount / segments)) * p;
					Vector3 point = new Vector3();
					point = BezierPathCalculation(p0, p1, p2, p3, t);
					pathPoints.Add(point);
				}
			}

		}
	}

	public bool smoothenCurve;
	public float stepDistance = 0f;

	public static float Speed = 2f;
	public static float StepDistance = 1f / 16f;

	private MeshFilter meshFilter;
	private MeshRenderer meshRenderer;

	public BezierPath path = new BezierPath();
	public GameObject[] nodes;

	// Use this for initialization
	void Start() {

		meshFilter = GetComponent<MeshFilter>();
		meshRenderer = GetComponent<MeshRenderer>();

		GetPath();
	}

	// Update is called once per frame 
	void FixedUpdate() { // OnGUI() {
        GetPath();
        Render();
    }

	private Vector3 origin;
	public enum RenderMode {
		Points,
		MeshLine,
		MeshPoints
    }
	public RenderMode renderMode;

	private void Render() {

		if (transform.position == origin) {
			// return;
        }

		List<Vector3> positions = new List<Vector3>();
		List<int> indices = new List<int>();
		List<Color> colors = new List<Color>();

		positions.Add(curve[0] - transform.position);
		positions.Add(curve[0] - transform.position);

		colors.Add(Color.yellow);
		colors.Add(Color.yellow);

		int count = curve.Count;

		for (int i = 1; i < count; i++) {

			Vector3 position = curve[i] - transform.position;
			Vector3 direction = (curve[i] - curve[i-1]).normalized;
			Vector3 offset = Asteroid.MaxRadius * (Quaternion.Euler(0, 0, 90f) * direction);
			positions.Add(position + offset);
			positions.Add(position - offset);

			if (renderMode == RenderMode.MeshPoints) {
				indices.Add(2 * i);
				indices.Add(2 * i + 1);

			}
			else if (renderMode == RenderMode.MeshLine) {
				indices.Add(2 * (i - 1));
				indices.Add(2 * i);

				indices.Add(2 * (i - 1) + 1);
				indices.Add(2 * i + 1);
			}

			colors.Add(Color.yellow);
			colors.Add(Color.yellow);
		}



		MeshTopology meshTopology = MeshTopology.Points;
		if (renderMode == RenderMode.MeshPoints) {
			meshTopology = MeshTopology.Points;
		}
		else if (renderMode == RenderMode.MeshLine) {
            positions.Add(curve[count - 1] - transform.position);
            positions.Add(curve[count - 1] - transform.position);

            indices.Add(2 * (count - 1));
            indices.Add(2 * count);

            indices.Add(2 * (count - 1) + 1);
            indices.Add(2 * count + 1);

            colors.Add(Color.yellow);
            colors.Add(Color.yellow);
            meshTopology = MeshTopology.Lines;
		}

		meshFilter.mesh.SetVertices(positions.ToArray());
		meshFilter.mesh.SetIndices(indices.ToArray(), meshTopology, 0);
		meshFilter.mesh.colors = colors.ToArray();

		origin = transform.position;
	}

	void OnDrawGizmos() {
        for (int i = 1; i < (path.pointCount); i++) {
            Vector3 startv = path.pathPoints[i - 1];
            Vector3 endv = path.pathPoints[i];
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(startv, endv);
        }
		for (int i = 0; i < nodes.Length; i++) {
			if (nodes[i] != null) {
				Gizmos.DrawWireSphere(nodes[i].transform.position, 0.1f);
				
			}
		}
	}

	public List<Vector3> curve = new List<Vector3>();
	private void GetPath() {
		List<Vector3> nodePositions = new List<Vector3>();
		for (int i = 0; i < nodes.Length; i++) {
			if (nodes[i] != null) {
				Vector3 position = nodes[i].transform.position;
				nodePositions.Add(position);
			}
		}
		path.DeletePath();
		path.CreateCurve(nodePositions);
		curve = path.pathPoints;
		if (smoothenCurve) {
			curve = SmoothenCurve(curve);
		}
	}

	public List<Vector3> SmoothenCurve(List<Vector3> curve) {

		// float stepDistance = ShuttlePath.StepDistance;
		float distance = 0;
		List<Vector3> smoothCurve = new List<Vector3>();
		smoothCurve.Add(curve[0]);

		for (int i = 1; i < curve.Count; i++) {

			distance += ((Vector2)curve[i] - (Vector2)curve[i - 1]).magnitude;
			if (distance > stepDistance) {

				smoothCurve.Add(curve[i]);
				distance -= stepDistance;

			}

		}

		if (!smoothCurve.Contains(curve[curve.Count - 1])) {
			smoothCurve.Add(smoothCurve[curve.Count - 1]);
		}

		return smoothCurve;

	}


}
