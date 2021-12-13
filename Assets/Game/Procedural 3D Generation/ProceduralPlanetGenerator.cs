﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ProceduralPlanetGenerator : MonoBehaviour {
    //Used to build a new planet on the fly
    public bool rebuild = false;

    //Local arrays for keeping track of our mesh data
    private Vector3[] m_vertices;
    private int[] m_triangles;
    private Color[] m_colors;
    private float radius = 0.5f;

    //Height/Noise mapping settings
    [Space(5), Header("Height Map Settings")]
    public bool useTexture;
    public Texture2D texture;
    public float perlinScale = 20f;
    public float seaLevel = .4f;
    public float maxHeight = 0.1f;
    public bool explode;

    //Color mapping settings
    [Space(5), Header("Color Map Settings")]
    public Gradient waterGradient;
    public Gradient landGradient;

    /* --- Properties --- */
    [SerializeField] public Vector2 inputVectorSeed;
    [SerializeField] public bool next;
    [SerializeField] public bool chunkyAnimatedSurface = false;
    [SerializeField] public bool smoothAnimatedSurface = false;
    [SerializeField] public int subdivisions;

    // Rotation
    [Range(0f, 1000f)] public float rotationSpeed;
    [Range(1f, 1000f)] public float rotationIncrements;
    public Vector3 rotationAxis = Vector3.up;
    private Vector3 internalEulerAngle;

    //Mesh filter we'll use for this planet
    MeshFilter m_meshFilter;

    private void Awake() {
        m_meshFilter = GetComponent<MeshFilter>();
    }

    private void Start() {
        BuildPlanet();
        if (chunkyAnimatedSurface) {
            StartCoroutine(IERebuild(0.2f));
        }

        internalEulerAngle = transform.eulerAngles;
    }

    private void Update() {

        if (next) {
            chunkyAnimatedSurface = false;
            smoothAnimatedSurface = false;
            inputVectorSeed += new Vector2(1f, 1f);
            BuildPlanet();
            next = false;
        }

        if (explode) {
            maxHeight -= 3 * Time.deltaTime;
            BuildPlanet();
        }

        if (smoothAnimatedSurface) {
            inputVectorSeed += new Vector2(0.01f, 0.01f);
            BuildPlanet();
        }

        //if (rebuild) {
        //    BuildPlanet();
        //    rebuild = false;
        //}

        Rotate();
    }

    private void Rotate() {

        internalEulerAngle += rotationSpeed * rotationAxis.normalized;
        internalEulerAngle = new Vector3(internalEulerAngle.x % 360f, internalEulerAngle.y % 360f, internalEulerAngle.z % 360f);
        if ((transform.eulerAngles - internalEulerAngle).magnitude > 360f / rotationIncrements) {
            transform.localRotation = Quaternion.Euler(internalEulerAngle);
        }
    }

    private IEnumerator IERebuild(float delay) {

        yield return new WaitForSeconds(delay);
        BuildPlanet();
        StartCoroutine(IERebuild(0.2f));
        yield return null;

    }

    /* --- Methods --- */
    public void BuildPlanet() {
        BuildCube();

        for (int i = 0; i < subdivisions; i++) {
            Subdivide();
        }
        Spherify();

        //RandomColors();

        HeightMap();
        RenderToMesh();
    }

    /// <summary>
    /// This is where the magic happens
    /// For each Vertex, we project a 3d point into 2d using Latitude/Longitude
    /// Then we map this to perlin noise, give height and color to the surface
    /// </summary>
    private void HeightMap() {
        if (m_colors == null || m_colors.Length != m_vertices.Length) {
            m_colors = new Color[m_vertices.Length];
        }
        Vector2 offset = GetSeed();

        for (int i = 0; i < m_vertices.Length; i++) {
            //Get Latitude and Longitude of the sphere in degrees
            LatLong latLong = new LatLong(m_vertices[i], radius);

            //Map Latlong to 0-1 values
            Vector2 uv = latLong.GetUV();

            //Sample the perlin noise
            float t = 0;
            if (useTexture && texture != null) {
                t = texture.GetPixel((int)((offset.x + uv.x) * texture.width) % texture.width, (int)((offset.y + uv.y) * texture.height) % texture.height).r;
            }
            else {
                t = Mathf.PerlinNoise((offset.x + uv.x) * perlinScale, (offset.y + uv.y) * perlinScale);
            }

            //Find the new distance from center for this variable
            float newDist = radius + Mathf.Lerp(0f, maxHeight, t);

            Color c = Color.Lerp(Color.black, Color.white, t);
            if (t <= seaLevel) {
                //If this vertex is at sea level, ensure it is level with all other water
                //Also modulate this distance against our maxHeight (as a factor of planetary radius)
                float landDepth = (seaLevel - t) / (1f - seaLevel);
                newDist = radius + (seaLevel * maxHeight * radius);
                c = waterGradient.Evaluate(landDepth);
            }
            else {
                //If this vertex is above sea level, map our height (between sea level and 1f) to a gradient
                float landLevel = (t - seaLevel) / (1f - seaLevel);
                c = landGradient.Evaluate(landLevel);
            }

            //Assign our data
            m_vertices[i] *= newDist / radius;
            m_colors[i] = c;
        }
    }

    private Vector2 GetSeed() {

        if (chunkyAnimatedSurface) {
            //Assign a random offset to our noise for some variety 
            return new Vector2(Random.Range(0, 100000), Random.Range(0, 100000));
        }

        return inputVectorSeed;
        
    }

    private void BuildCube() {
        //6 Faces, 2 Tris per face, 3 Verts per Tri
        m_triangles = new int[6 * 2 * 3];
        m_vertices = new Vector3[8];

        Vertices();
        Tris();
    }

    private void RandomColors() {
        m_colors = new Color[m_vertices.Length];
        //Now add some random colors
        for (int i = 0; i < m_colors.Length; i++) {
            Color color = Color.white;
            color.r = Random.Range(0f, 1f);
            color.g = Random.Range(0f, 1f);
            color.b = Random.Range(0f, 1f);
            m_colors[i] = color;
        }
    }

    private void RenderToMesh() {
        if (m_meshFilter != null) {
            if (m_meshFilter.mesh == null) {
                m_meshFilter.mesh = new Mesh();
                m_meshFilter.mesh.MarkDynamic();
            }

            //We're using UInt32 indexing so we can have more than 65k verts/mesh
            //This is not supported in all systems
            m_meshFilter.mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            m_meshFilter.mesh.vertices = m_vertices;
            m_meshFilter.mesh.SetTriangles(m_triangles, 0);
            m_meshFilter.mesh.colors = m_colors;
        }
    }

    private void Spherify() {
        for (int i = 0; i < m_vertices.Length; i++) {
            float curDist = Vector3.Distance(m_vertices[i], Vector3.zero);
            m_vertices[i] *= (radius / curDist);
        }
    }

    /* --- Abstract this noise --- */
    private void Subdivide() {
        //For simplicity, we're going to build our new mesh data with an arraylist
        //In high performance systems you'd probably do a lot differently, especially this
        List<Vector3> vNew = new List<Vector3>();
        List<int> tNew = new List<int>();

        //Do this for every triangle (+=3 since we're referencing indices
        for (int i = 0; i < m_triangles.Length; i += 3) {
            //Each original vertex in our original triangle
            Vector3 v1 = m_vertices[m_triangles[i]];
            Vector3 v3 = m_vertices[m_triangles[i + 1]];
            Vector3 v5 = m_vertices[m_triangles[i + 2]];

            //Create one vertex in the center of each edge of our original triangle
            Vector3 v2 = Vector3.Lerp(v1, v3, .5f);
            Vector3 v4 = Vector3.Lerp(v3, v5, .5f);
            Vector3 v6 = Vector3.Lerp(v5, v1, .5f);

            //Add each vertex (old and new) to our new vert list
            int newIndex = vNew.Count;
            vNew.Add(v1);
            vNew.Add(v2);
            vNew.Add(v3);
            vNew.Add(v4);
            vNew.Add(v5);
            vNew.Add(v6);

            //The nice thing about lists is we can make the tris we are creating a little more obvious
            //Yeah, this is for readability rather than performance again, thanks for noticing.
            tNew.AddRange(new int[] { newIndex + 0, newIndex + 1, newIndex + 5 });
            tNew.AddRange(new int[] { newIndex + 1, newIndex + 2, newIndex + 3 });
            tNew.AddRange(new int[] { newIndex + 1, newIndex + 3, newIndex + 5 });
            tNew.AddRange(new int[] { newIndex + 5, newIndex + 3, newIndex + 4 });
        }

        //Load up our new mesh data
        m_vertices = vNew.ToArray();
        m_triangles = tNew.ToArray();
    }

    private void Vertices() {
        //Bottom of cube
        m_vertices[0] = new Vector3(-.5f, -.5f, -.5f);
        m_vertices[1] = new Vector3(-.5f, -.5f, .5f);
        m_vertices[2] = new Vector3(.5f, -.5f, .5f);
        m_vertices[3] = new Vector3(.5f, -.5f, -.5f);

        //Top of cube
        m_vertices[4] = new Vector3(-.5f, .5f, -.5f);
        m_vertices[5] = new Vector3(-.5f, .5f, .5f);
        m_vertices[6] = new Vector3(.5f, .5f, .5f);
        m_vertices[7] = new Vector3(.5f, .5f, -.5f);
    }

    private void Tris() {
        //-Z Face
        m_triangles[0] = 0;
        m_triangles[1] = 4;
        m_triangles[2] = 7;

        m_triangles[3] = 0;
        m_triangles[4] = 7;
        m_triangles[5] = 3;

        //+Z Face
        m_triangles[6] = 2;
        m_triangles[7] = 6;
        m_triangles[8] = 5;

        m_triangles[9] = 2;
        m_triangles[10] = 5;
        m_triangles[11] = 1;

        //-X Face
        m_triangles[12] = 1;
        m_triangles[13] = 5;
        m_triangles[14] = 4;

        m_triangles[15] = 1;
        m_triangles[16] = 4;
        m_triangles[17] = 0;

        //+X Face
        m_triangles[18] = 3;
        m_triangles[19] = 7;
        m_triangles[20] = 6;

        m_triangles[21] = 3;
        m_triangles[22] = 6;
        m_triangles[23] = 2;

        //-Y Face
        m_triangles[24] = 1;
        m_triangles[25] = 0;
        m_triangles[26] = 3;

        m_triangles[27] = 1;
        m_triangles[28] = 3;
        m_triangles[29] = 2;

        //+Y Face
        m_triangles[30] = 4;
        m_triangles[31] = 5;
        m_triangles[32] = 6;

        m_triangles[33] = 4;
        m_triangles[34] = 6;
        m_triangles[35] = 7;
    }

}