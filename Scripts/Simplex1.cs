using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1-dimensional (cylinder, or edge) class
public class Simplex1 : MonoBehaviour {

	// True if user has chosen to show edges, false otherwise	
	public bool is_shown;
	// True if length is less than twice radius, false otherwise
	public bool is_drawn;
	public float length;
	public List<int> neighbors0 = new List<int>();
	public List<int> neighbors2 = new List<int>();

}