using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 2-dimensional (triangle, or face) class
public class Simplex2 : MonoBehaviour {

	// True if user has chosen to show edges, false otherwise	
	public bool is_shown;
	public bool is_drawn;
	// Unnecessary for Vietoris-Rips approach, is_drawn depends on is_drawn of neighbors1.
	// Necessary for Cech approach.
	public float draw_threshold;
	public List<int> neighbors0 = new List<int>();
	public List<int> neighbors1 = new List<int>();
	
	public Vector3[] newVertices = new Vector3[3];

    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        mesh.Clear();

        mesh.vertices = newVertices;
        mesh.uv = new Vector2[] {new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1)};;
        mesh.triangles = new int[] { 0, 1, 2 };
    }
}