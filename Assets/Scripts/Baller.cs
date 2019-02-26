using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baller : MonoBehaviour {
    public GameObject leftHandAnchor;
    public GameObject rightHandAnchor;
    public GameObject ballPrefab;
    public GameObject haloPrefab;
	public float threshold = 0.03f;
	// For keeping track
	private List<GameObject> BList = new List<GameObject>();
	private List<GameObject> HList = new List<GameObject>();
	private int radius;
	// For adding
	private GameObject clone;
	private GameObject halo;
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
            }
        }
        if (OVRInput.GetDown(OVRInput.RawButton.Y)) {
            if (leftHandAnchor != null) {
            	AddBall(leftHandAnchor);
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
			UpdateRadii();
		}
	}

	// Adds a ball and a halo at the input anchor position
	void AddBall (GameObject anchor) {
		// Instantiate ball at anchor position
    	GameObject clone = (GameObject)Instantiate(ballPrefab, anchor.transform.position, anchor.transform.rotation);
		clone.name = "clone" + num.ToString();
		BList.Add(clone);
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
			Debug.Log("GONNA DESTROY: " + ball.collider.name);
			nearBall = ball.collider.gameObject;
			nearHalo = nearBall.transform.GetChild(0).gameObject;
			// Remove from lists
			BList.Remove(nearBall);
			HList.Remove(nearHalo);
			// Drop ball to bottom, make invisible
			rb = nearBall.GetComponent<Rigidbody>();
			rb.isKinematic = false;
			rb.useGravity = true;
			Destroy(nearBall.GetComponent<MeshRenderer>());
			Destroy(nearHalo.GetComponent<MeshRenderer>());
		}
	}

	void UpdateRadii () {
		foreach (GameObject halo in HList) {
			halo.transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);
		}
	}
}