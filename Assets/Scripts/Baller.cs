using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baller : MonoBehaviour {
    public GameObject leftHandAnchor;
    public GameObject rightHandAnchor;
    public GameObject ballPrefab;
    public GameObject haloPrefab;
	public float threshold = 0.03f;
	private List<GameObject> BList = new List<GameObject>();
	private List<GameObject> HList = new List<GameObject>();
	private GameObject nearBall;
	private GameObject clone;
	private GameObject halo;
	private Rigidbody rb;
	private string name;
	static int num;
	static int curnum;
	
	// Update is called once per frame
	void Update () {
		int layerMask = 1 << 2;
		layerMask = ~layerMask;
        
		// Create
	/*	
	if(OVRInput.Get(OVRInput.Button.PrimaryThumbstickUp))
	{
		  transform.localScale += Vector3(0.5,0.5,0.5);
	}
	if(OVRInput.Get(OVRInput.Button.PrimaryThumbstickDown))
	{
		  transform.localScale -= Vector3(0.5,0.5,0.5);
	}
	*/
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
	}

	// Adds a ball and a halo at the input anchor position
	void AddBall (GameObject anchor) {
		// Instantiate ball at anchor position
    	GameObject clone = (GameObject)Instantiate(ballPrefab, anchor.transform.position, anchor.transform.rotation);
		clone.name = "clone" + num.ToString();
		BList.Add(clone);
		// Instantiate halo at anchor position
        GameObject halo = (GameObject)Instantiate(haloPrefab, anchor.transform.position, anchor.transform.rotation);
		halo.name = "halo" + num.ToString();				
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
			Debug.Log("GONNA DESTROY: " + ball.collider.name);
			// Remove the ball
			if (nearBall.name.Substring(0,5) == "clone"){
				// Remove from list
				BList.Remove(nearBall);
				// Drop to bottom, make invisible
				rb = nearBall.GetComponent<Rigidbody>();
				rb.isKinematic = false;
				rb.useGravity = true;
				Destroy(nearBall.GetComponent<MeshRenderer>());
			}
			// Remove the halo
			else { if (nearBall.name.Substring(0,4) == "halo") {
				// Remove from list
				HList.Remove(nearBall);
				// Drop to bottom, make invisible
				rb = nearBall.GetComponent<Rigidbody>();
				rb.isKinematic = false;
				rb.useGravity = true;
				Destroy(nearBall.GetComponent<MeshRenderer>());
			}}
		}
	}

	void UpdateRadii () {
	}

}