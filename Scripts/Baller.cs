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
	
	// Parameters
	public float threshold = 0.03f;
	public float r = 0.1f;
	
	// For showing / hiding
	public Material halo_shown;
	public Material cylinder_shown;
	public Material hidden;
	static int show_hide = 0;

	// For keeping track
	private List<GameObject> BList = new List<GameObject>();
	private List<GameObject> HList = new List<GameObject>();
	private List<GameObject> CList = new List<GameObject>();
	private List<int> BList_alive = new List<int>();
	private List<int> BList_dead = new List<int>();
	private List<int> CList_alive = new List<int>();
	private List<int> CList_dead = new List<int>();
	private Vector3 edger;
	private int vertex;
	private int radius;
	private GameObject ball0;
	private GameObject ball1;
	
	// For adding / deleting
	private GameObject clone;
	private GameObject halo;
	private GameObject cylinder;
	static int num0 = 0;
	static int num1 = 0;
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
            }
        }
        if (OVRInput.GetDown(OVRInput.RawButton.Y)) {
            if (leftHandAnchor != null) {
            	AddBall(leftHandAnchor);
				UpdateRadii();
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
		}
		
		if (OVRInput.Get(OVRInput.RawButton.RThumbstickDown)) {
			r -= 0.02f;
			UpdateRadii();
			foreach (int cylindex in CList_alive) {
				if (.05*r < CList[cylindex].GetComponent<Simplex1>().length ){
					CList[cylindex].GetComponent<Simplex1>().is_drawn = false;
				}
			}
		}
		
		// When to show or hide halos and edges
		if (OVRInput.GetDown(OVRInput.RawButton.RThumbstick)) {
			show_hide++;
			foreach (int ballindex in BList_alive) {
				BList[ballindex].GetComponent<Simplex0>().is_halo_shown = (show_hide % 2 == 0);
			}
			foreach (int cylindex in CList_alive) {
				CList[cylindex].GetComponent<Simplex1>().is_shown = (show_hide % 4 < 2);
			}
		}
		
		foreach (int cylindex in CList_alive) {
			if (CList[cylindex].GetComponent<Simplex1>().is_shown & CList[cylindex].GetComponent<Simplex1>().is_drawn) {
				CList[cylindex].GetComponent<MeshRenderer>().material = cylinder_shown;
			}
			else {
				CList[cylindex].GetComponent<MeshRenderer>().material = hidden;
			}
		}
		
		foreach (int ballindex in BList_alive){
			// When to adjust connecting cylinders
			if (BList[ballindex].gameObject.transform.hasChanged){
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
			CList_alive.Add(num1);
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
			num1++;
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
				// Remove edge from lists
				foreach (int index1 in nearBall.GetComponent<Simplex0>().neighbors1) {
					Destroy(CList[index1].GetComponent<MeshRenderer>());
					CList_alive.Remove(index1);
					CList_dead.Add(index1);
					foreach (int index0 in nearBall.GetComponent<Simplex0>().neighbors0) {
						if (BList[index0].GetComponent<Simplex0>().neighbors1.Contains(index1)) {
							BList[index0].GetComponent<Simplex0>().neighbors1.Remove(index1);
						}
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
}