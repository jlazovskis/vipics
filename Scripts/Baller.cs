using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baller : MonoBehaviour {
	
	// Objects in the scene
	public GameObject leftHandAnchor;
    public GameObject rightHandAnchor;
    public GameObject ballPrefab;
    public GameObject haloPrefab;
    public GameObject cylinderPrefab;
    public GameObject trianglePrefab;
	
	// Parameters
	public float threshold = 0.03f;
	public float r = 0.1f;
	
	// For showing / hiding
	public Material halo_shown;
	public Material cylinder_shown;
	public Material triangle_shown;
	public Material hidden;
	static int show_hide = 0;

	// For keeping track
	private List<GameObject> BList = new List<GameObject>();
	private List<GameObject> HList = new List<GameObject>();
	private List<GameObject> CList = new List<GameObject>();
	private List<GameObject> TList = new List<GameObject>();
	private List<int> BList_alive = new List<int>();
	private List<int> BList_dead = new List<int>();
	private List<int> CList_alive = new List<int>();
	private List<int> CList_dead = new List<int>();
	private List<int> TList_alive = new List<int>();
	private List<int> TList_dead = new List<int>();
	private Vector3 edger;
	private int vertex;
	private int radius;
	private GameObject ball0;
	private GameObject ball1;
	private GameObject ball2;
	private GameObject cyl0;
	private GameObject cyl1;
	
	// For adding / deleting
	private GameObject clone;
	private GameObject halo;
	private GameObject cylinder;
	private GameObject triangle;
	private List<int> temp_list0 = new List<int>();
	private List<int> temp_list1 = new List<int>();
	static int num0 = 0;
	static int num1 = 0;
	static int num2 = 0;
	private GameObject nearBall;
	private GameObject nearHalo;
	private Rigidbody rb;	
	
	// Update is called once per frame
	void Update () {
		// Mask to not detect hands when deleting objects in scene
		int layerMask = 1 << 2;
		layerMask = ~layerMask;
        
        // When to add balls and halos
		if (OVRInput.GetDown(OVRInput.RawButton.B)) {
            if (rightHandAnchor != null) {
            	AddBall(rightHandAnchor);
				UpdateRadii();
				UpdateCylinders();
            }
        }
        if (OVRInput.GetDown(OVRInput.RawButton.Y)) {
            if (leftHandAnchor != null) {
            	AddBall(leftHandAnchor);
				UpdateRadii();
				UpdateCylinders();
			}
        }
		
		// When to remove balls and halos
		if (OVRInput.GetDown(OVRInput.RawButton.A)) {
			if (rightHandAnchor != null) {
				RemoveBall(rightHandAnchor);
			}
		}
		if (OVRInput.GetDown(OVRInput.RawButton.X)) {
			if (leftHandAnchor != null) {
				RemoveBall(leftHandAnchor);
			}
		}
		
		// When to change radius of halos
		if (OVRInput.Get(OVRInput.RawButton.RThumbstickUp)) {
			r += 0.02f;
			UpdateRadii();
			foreach (int cylindex in CList_alive) {
				if (.05*r >= CList[cylindex].GetComponent<Simplex1>().length ){
					CList[cylindex].GetComponent<Simplex1>().is_drawn = true;
				}
			}
			UpdateCylinders();
		}
		
		if (OVRInput.Get(OVRInput.RawButton.RThumbstickDown)) {
			r -= 0.02f;
			UpdateRadii();
			foreach (int cylindex in CList_alive) {
				if (.05*r < CList[cylindex].GetComponent<Simplex1>().length ){
					CList[cylindex].GetComponent<Simplex1>().is_drawn = false;
				}
			}
			UpdateCylinders();
		}
		
		//if (OVRInput.GetDown(OVRInput.RawButton.LThumbstick)) {
		//	GameObject triangle = (GameObject)Instantiate(trianglePrefab, leftHandAnchor.transform.position, leftHandAnchor.transform.rotation);
		//}
		
		// When to show or hide halos, edges, triangles
		if (OVRInput.GetDown(OVRInput.RawButton.RThumbstick)) {
			show_hide++;
			foreach (int ballindex in BList_alive) {
				BList[ballindex].GetComponent<Simplex0>().is_halo_shown = (show_hide % 2 == 0);
			}
			foreach (int cylindex in CList_alive) {
				CList[cylindex].GetComponent<Simplex1>().is_shown = (show_hide % 4 < 2);
			}
			UpdateCylinders();
			foreach (int triindex in TList_alive) {
				TList[triindex].GetComponent<Simplex2>().is_shown = (show_hide % 4 < 2);
			}
		}
		
		foreach (int triindex in TList_alive) {
			if (TList[triindex].GetComponent<Simplex2>().is_shown & TList[triindex].GetComponent<Simplex2>().is_drawn) {
				TList[triindex].GetComponent<MeshRenderer>().material = triangle_shown;
			}
			else {
				TList[triindex].GetComponent<MeshRenderer>().material = hidden;
			} 
		}
		

		
		foreach (int ballindex in BList_alive){
			if (BList[ballindex].gameObject.transform.hasChanged){
				// Adjust neighboring cylinders
				foreach (int cylindex in BList[ballindex].gameObject.GetComponent<Simplex0>().neighbors1){
					GameObject cylinder = CList[cylindex];
					GameObject ball0 = BList[cylinder.GetComponent<Simplex1>().neighbors0[0]];
					GameObject ball1 = BList[cylinder.GetComponent<Simplex1>().neighbors0[1]];
					cylinder.transform.position = (ball0.transform.position + ball1.transform.position)/2;
					cylinder.transform.rotation = Quaternion.FromToRotation(Vector3.up, ball0.transform.position - ball1.transform.position);
					cylinder.transform.localScale = new Vector3(0.015f, Vector3.Distance(ball0.transform.position/2, ball1.transform.position/2), 0.015f);
					cylinder.GetComponent<Simplex1>().length = (ball0.transform.position - ball1.transform.position).magnitude;
					cylinder.GetComponent<Simplex1>().is_drawn = (.05*r >= cylinder.GetComponent<Simplex1>().length);
				}
				// Adjust neighboring triangles
				foreach (int triindex in BList[ballindex].gameObject.GetComponent<Simplex0>().neighbors2){
					GameObject triangle = TList[triindex];
					temp_list0 = triangle.GetComponent<Simplex2>().neighbors0;
					temp_list1 = triangle.GetComponent<Simplex2>().neighbors1;
					Destroy(triangle.GetComponent<Simplex2>());
					Simplex2 tri = triangle.AddComponent<Simplex2>(); 
					tri.neighbors0 = temp_list0;
					tri.neighbors1 = temp_list1;
					GameObject ball0 = BList[tri.neighbors0[0]];
					GameObject ball1 = BList[tri.neighbors0[1]];
					GameObject ball2 = BList[tri.neighbors0[2]];
					tri.newVertices = new Vector3[3] { .05f*ball0.transform.position, .05f*ball1.transform.position, .05f*ball2.transform.position };
					tri.is_drawn = (CList[tri.neighbors1[0]].GetComponent<Simplex1>().is_drawn & CList[tri.neighbors1[1]].GetComponent<Simplex1>().is_drawn & CList[tri.neighbors1[2]].GetComponent<Simplex1>().is_drawn );
					tri.is_shown = (show_hide % 4 < 2);	
				}
				BList[ballindex].gameObject.transform.hasChanged = false;
			}
			// When to show or hide halos
			if (BList[ballindex].GetComponent<Simplex0>().is_halo_shown) {
				HList[ballindex].GetComponent<MeshRenderer>().material = halo_shown;
			}
			else {
				HList[ballindex].GetComponent<MeshRenderer>().material = hidden;
			}
		}
	}

	// Adds a ball and a halo at the input anchor position
	void AddBall (GameObject anchor) {
		// Instantiate ball at anchor position and add to list
		GameObject clone = (GameObject)Instantiate(ballPrefab, anchor.transform.position, anchor.transform.rotation);
		clone.name = num0.ToString();
		BList.Add(clone);
		// Instantiate halo at anchor position
		GameObject halo = (GameObject)Instantiate(haloPrefab, anchor.transform.position, anchor.transform.rotation);
		halo.name = num0.ToString();
		halo.transform.SetParent(clone.transform);
		HList.Add(halo);
		Simplex0 s = clone.AddComponent<Simplex0>();
		s.is_halo_shown = (show_hide % 2 == 0);
		// Instantiate cylinder for every vertex in scene
		foreach (int b in BList_alive) {
			GameObject existingball = BList[b];
			GameObject cylinder = (GameObject)Instantiate(cylinderPrefab, (anchor.transform.position + existingball.transform.position)/2, Quaternion.FromToRotation(Vector3.up, anchor.transform.position - existingball.transform.position));
			cylinder.transform.localScale = new Vector3(0.015f, Vector3.Distance(existingball.transform.position/2, anchor.transform.position/2), 0.015f);
			cylinder.name = num1.ToString();
			CList.Add(cylinder);
			// Set cylinder attributes
			Simplex1 c = cylinder.AddComponent<Simplex1>();
			Vector3 edger = clone.transform.position - existingball.transform.position;
			c.length = edger.magnitude;
			c.is_shown = (show_hide % 4 < 2);
			c.is_drawn = (.05*r >= c.length);
			// Add edge index to neighbors
			s.neighbors1.Add(num1);
			existingball.GetComponent<Simplex0>().neighbors1.Add(num1);
			// Add vertex indices to neighbors
			c.neighbors0.Add(num0);
			existingball.GetComponent<Simplex0>().neighbors0.Add(num0);
			c.neighbors0.Add(System.Convert.ToInt32(existingball.name));
			s.neighbors0.Add(System.Convert.ToInt32(existingball.name));
			CList_alive.Add(num1);
			num1++;
		}
		// Instantiate triangle for every pair of vertices in scene
		foreach (int c in CList_alive) {
			if (!CList[c].GetComponent<Simplex1>().neighbors0.Contains(num0)) {
				GameObject triangle = (GameObject)Instantiate(trianglePrefab, new Vector3(0,0,0), Quaternion.identity);
				triangle.name = num2.ToString();
				TList.Add(triangle);
				triangle.transform.localScale = new Vector3 (20f,20f,20f);
				Simplex2 tri = triangle.AddComponent<Simplex2>(); 
				GameObject ball0 = BList[CList[c].GetComponent<Simplex1>().neighbors0[0]];
				GameObject ball1 = BList[CList[c].GetComponent<Simplex1>().neighbors0[1]];
				tri.newVertices = new Vector3[3] { .05f*clone.transform.position, .05f*ball0.transform.position, .05f*ball1.transform.position };
				// Add triangle index to neighbors
				s.neighbors2.Add(num2);
				ball0.GetComponent<Simplex0>().neighbors2.Add(num2);
				ball1.GetComponent<Simplex0>().neighbors2.Add(num2);
				CList[c].GetComponent<Simplex1>().neighbors2.Add(num2);
				GameObject cyl0 = CList[ball0.GetComponent<Simplex0>().neighbors1.Intersect(s.neighbors1).ToList()[0]];
				GameObject cyl1 = CList[ball1.GetComponent<Simplex0>().neighbors1.Intersect(s.neighbors1).ToList()[0]];
				cyl0.GetComponent<Simplex1>().neighbors2.Add(num2);
				cyl1.GetComponent<Simplex1>().neighbors2.Add(num2);
				// Add edge indices to neighbors
				tri.neighbors1.Add(c);
				tri.neighbors1.Add(System.Convert.ToInt32(cyl0.name));
				tri.neighbors1.Add(System.Convert.ToInt32(cyl1.name));
				// Add vertex indices to neighbors
				tri.neighbors0 = new List<int> { num0, System.Convert.ToInt32(ball0.name), System.Convert.ToInt32(ball1.name) };
				// Set triangle attributes
				tri.is_shown = (show_hide % 4 < 2);
				tri.is_drawn = ( CList[c].GetComponent<Simplex1>().is_drawn & cyl0.GetComponent<Simplex1>().is_drawn & cyl1.GetComponent<Simplex1>().is_drawn );
				TList_alive.Add(num2);
				num2++;
			}
		}
		BList_alive.Add(num0);
		num0++;
	}
	
	void RemoveBall (GameObject anchor) {
		// Find objects within threshold of anchor
		RaycastHit[] hits = Physics.SphereCastAll(anchor.transform.position, threshold, anchor.transform.forward, 0f);
		foreach (RaycastHit ball in hits){
			nearBall = ball.collider.gameObject;
			if (nearBall.tag == "BALL") {
				int vertex = System.Convert.ToInt32(nearBall.name);
				// Remove vertex from lists
				BList_alive.Remove(vertex);
				BList_dead.Add(vertex);
				foreach (int index0 in nearBall.GetComponent<Simplex0>().neighbors0) {
					BList[index0].GetComponent<Simplex0>().neighbors0.Remove(vertex);
				}
				// Remove edges from lists and destroy meshes
				foreach (int index1 in nearBall.GetComponent<Simplex0>().neighbors1) {
					Destroy(CList[index1].GetComponent<MeshRenderer>());
					CList_alive.Remove(index1);
					CList_dead.Add(index1);
					BList[CList[index1].GetComponent<Simplex1>().neighbors0.Except(new List<int> {vertex}).ToList()[0]].GetComponent<Simplex0>().neighbors1.Remove(index1);
				}
				// Remove triangles from lists and destroy meshes
				foreach (int index2 in nearBall.GetComponent<Simplex0>().neighbors2) {
					Destroy(TList[index2].GetComponent<MeshRenderer>());
					TList_alive.Remove(index2);
					TList_dead.Add(index2);
					foreach (int index1 in TList[index2].GetComponent<Simplex2>().neighbors1) {
						CList[index1].GetComponent<Simplex1>().neighbors2.Remove(index2);
					}
					foreach (int index0 in TList[index2].GetComponent<Simplex2>().neighbors0.Except(new List<int> {vertex}).ToList()) {
						BList[index0].GetComponent<Simplex0>().neighbors2.Remove(index2);
					}
				}
				// Drop ball to bottom, make invisible
				rb = nearBall.GetComponent<Rigidbody>();
				rb.isKinematic = false;
				rb.useGravity = true;
				nearHalo = nearBall.transform.GetChild(0).gameObject;
				Destroy(nearHalo.GetComponent<MeshRenderer>());
				Destroy(nearBall.GetComponent<MeshRenderer>());
			}
		}
	}

	void UpdateRadii () {
		foreach (GameObject halo in HList) {
			halo.transform.localScale = new Vector3(r, r, r);
		}
	}
	
	void UpdateCylinders () {
		foreach (int cylindex in CList_alive) {
			if (CList[cylindex].GetComponent<Simplex1>().is_shown & CList[cylindex].GetComponent<Simplex1>().is_drawn) {
				CList[cylindex].GetComponent<MeshRenderer>().material = cylinder_shown;
			}
			else {
				CList[cylindex].GetComponent<MeshRenderer>().material = hidden;
			}
		}
	}
}