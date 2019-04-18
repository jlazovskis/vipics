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
	
	// For keeping track
	private List<GameObject> BList = new List<GameObject>();
	private List<GameObject> HList = new List<GameObject>();
	private List<GameObject> CList = new List<GameObject>();
	private List<List<int>> VertexList = new List<List<int>>();
	private List<List<int>> EdgeList = new List<List<int>>();
	private List<int> TempEdgeList1 = new List<int>();
    private List<int> DeletedBallList = new List<int>();
	private List<GameObject> TempEdgeList2 = new List<GameObject>();
	private int vertex;
	private int edge;
	private int radius;
	private int ballA;
	private int ballB;
	
	// For adding
	private GameObject clone;
	private GameObject halo;
	private GameObject cylinder;
	static int num;
	
	// For deleting
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
		if(OVRInput.Get(OVRInput.Button.PrimaryThumbstickUp)) {
			r += 0.02f;
			UpdateRadii();
		}
		
		if(OVRInput.Get(OVRInput.Button.PrimaryThumbstickDown)) {
			r -= 0.02f;
			UpdateRadii();
		}
		
		// When to adjust connecting cylinders
		foreach(GameObject ball in BList){
			if(ball.transform.hasChanged){
				Debug.Log("Whoa!");
				int vertex = BList.IndexOf(ball);
				foreach(int cylindex in EdgeList[vertex]){
					cylinder = CList[cylindex];
					ballA = VertexList[cylindex][0];
					ballB = VertexList[cylindex][1];
					cylinder.transform.position = (BList[ballA].transform.position + BList[ballB].transform.position)/2;
					cylinder.transform.rotation = Quaternion.FromToRotation(Vector3.up, BList[ballA].transform.position - BList[ballB].transform.position);
					cylinder.transform.localScale = new Vector3(0.015f, Vector3.Distance(BList[ballA].transform.position/2, BList[ballB].transform.position/2), 0.015f);
				}
				ball.transform.hasChanged = false;
			}
		}
	}
	
	// Adds a ball and a halo at the input anchor position
	void AddBall (GameObject anchor) {
		// Instantiate ball at anchor position
    	GameObject clone = (GameObject)Instantiate(ballPrefab, anchor.transform.position, anchor.transform.rotation);
		clone.name = "clone" + num.ToString();
		TempEdgeList1.Clear();
		// Instantiate cylinder between anchor and other clones
		foreach (GameObject existing_ball in BList){
			int vertex = BList.IndexOf(existing_ball);
            if (!DeletedBallList.Contains(vertex)) {
				GameObject cylinder = (GameObject)Instantiate(cylinderPrefab, (anchor.transform.position + existing_ball.transform.position)/2, Quaternion.FromToRotation(Vector3.up, anchor.transform.position - existing_ball.transform.position));
				cylinder.transform.localScale = new Vector3(0.015f, Vector3.Distance(existing_ball.transform.position/2, anchor.transform.position/2), 0.015f);
				cylinder.name = "cylinder-" + num.ToString() + "-" + vertex.ToString();
				// Add cylinder to lists
				CList.Add(cylinder);
				TempEdgeList1.Add(CList.IndexOf(cylinder));
				EdgeList[vertex].Add(CList.IndexOf(cylinder));
				//Debug.Log("Ball " + vertex + " now has " + EdgeList[vertex].Count + " neighbors");
				// Add vertices of cylinder to list
				VertexList.Add(new List<int> { num, vertex });
			}
		}
		// Add ball to list
		BList.Add(clone);
		// Add edges of ball to list
		//EdgeList.Add(TempEdgeList1);
		EdgeList.Add(new List<int> {});
		foreach (int cylindex in TempEdgeList1) {
			EdgeList[num].Add(cylindex);
		}
		// Instantiate halo at anchor position, as child of ball
        GameObject halo = (GameObject)Instantiate(haloPrefab, anchor.transform.position, anchor.transform.rotation);
		halo.name = "halo" + num.ToString();
		halo.transform.SetParent(clone.transform);
		HList.Add(halo);
		// Increase counter	
		num++;
	}

	// Removes all balls and halos within a threshold distance of the anchor position
	void RemoveBall (GameObject anchor) {
		// Find objects within threshold of anchor
		RaycastHit[] hits = Physics.SphereCastAll(anchor.transform.position, threshold, anchor.transform.forward, 0f);
		foreach (RaycastHit ball in hits){
			nearBall = ball.collider.gameObject;
			Destroy(nearBall.GetComponent<MeshRenderer>());
			if (nearBall.tag == "BALL") {
				Debug.Log("GONNA DESTROY: " + ball.collider.name);
				nearHalo = nearBall.transform.GetChild(0).gameObject;
				// Remove from lists
				//BList.Remove(nearBall);
				//HList.Remove(nearHalo);
				// Add to list of deleted balls
                DeletedBallList.Add(BList.IndexOf(nearBall));
				// Drop ball to bottom, make invisible
				rb = nearBall.GetComponent<Rigidbody>();
				rb.isKinematic = false;
				rb.useGravity = true;
				Destroy(nearHalo.GetComponent<MeshRenderer>());
			}
		}
	}

	void UpdateRadii () {
		foreach (GameObject halo in HList) {
			halo.transform.localScale = new Vector3(r, r, r);
		}
	}
}