using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleMeshGenerator : MonoBehaviour {
	
	void Start() {
		
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        mesh.vertices = new Vector3[] {new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0)};
        mesh.uv = new Vector2[] {new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1)};
        mesh.triangles = new int[] { 0, 1, 2 };
	}
}