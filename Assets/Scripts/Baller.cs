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
	private GameObject nearBall;
	private GameObject clone;
	private GameObject halo;
	private Rigidbody rb;
	private string name;
	static int num;
	
	// Update is called once per frame
	void Update () {
		int layerMask = 1 << 2;
		layerMask = ~layerMask;
        
		// Create
		
	if(OVRInput.Get(OVRInput.Button.PrimaryThumbstickUp))
	{
		  transform.localScale += Vector3(0.5,0.5,0.5);
	}
	if(OVRInput.Get(OVRInput.Button.PrimaryThumbstickDown))
	{
		  transform.localScale -= Vector3(0.5,0.5,0.5);
	}
        if (OVRInput.GetDown(OVRInput.RawButton.B)) {
            if (rightHandAnchor != null) {
                GameObject clone = (GameObject)Instantiate(ballPrefab, rightHandAnchor.transform.position, rightHandAnchor.transform.rotation);
				clone.name = "clone" + num.ToString();
                GameObject halo = (GameObject)Instantiate(haloPrefab, rightHandAnchor.transform.position, rightHandAnchor.transform.rotation);
				halo.name = "halo" + num.ToString();				
				num++;
				// Add new clone to list of balls
				BList.Add(clone);
            }
        }
        if (OVRInput.GetDown(OVRInput.RawButton.Y)) {
            if (leftHandAnchor != null) {
				GameObject clone = (GameObject)Instantiate(ballPrefab, leftHandAnchor.transform.position, leftHandAnchor.transform.rotation);
				clone.name = "clone" + num.ToString();
				num++;
				// Add new clone to list of balls
				BList.Add(clone);
			}
        }
		
		if (OVRInput.GetDown(OVRInput.RawButton.A)) {
			if (rightHandAnchor != null) {
				RaycastHit[] hits = Physics.SphereCastAll(rightHandAnchor.transform.position, threshold, rightHandAnchor.transform.forward, 0f);
				foreach (RaycastHit ball in hits){
					nearBall = ball.collider.gameObject;
					Debug.Log("GONNA DESTROY: " + ball.collider.name);
					// Remove clone from list of balls
					BList.Remove(nearBall);
					// Drop clone to bottom, make invisible
					rb = nearBall.GetComponent<Rigidbody>();
					rb.isKinematic = false;
					rb.useGravity = true;
					Destroy(nearBall.GetComponent<MeshRenderer>());
				}
			}
		}
		if (OVRInput.GetDown(OVRInput.RawButton.X)) {
			if (leftHandAnchor != null) {
				RaycastHit[] hits = Physics.SphereCastAll(leftHandAnchor.transform.position, threshold, leftHandAnchor.transform.forward, 0f);
				foreach (RaycastHit ball in hits){
					nearBall = ball.collider.gameObject;
					Debug.Log("GONNA DESTROY: " + ball.collider.name);
					// Remove clone from list of balls
					BList.Remove(nearBall);
					// Drop clone to bottom, make invisible					
					rb = nearBall.GetComponent<Rigidbody>();
					rb.isKinematic = false;
					rb.useGravity = true;
					Destroy(nearBall.GetComponent<MeshRenderer>());
				}
			}
		}
	}
}

