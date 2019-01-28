using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baller : MonoBehaviour {
    public GameObject leftHandAnchor;
    public GameObject rightHandAnchor;
    public GameObject ballPrefab;
	public float threshold = 0.03f;	
	private GameObject nearBall;
	
	// Update is called once per frame
	void Update () {
        // Create
        if (OVRInput.GetDown(OVRInput.RawButton.B)) {
            if (rightHandAnchor != null) {
                Instantiate(ballPrefab, rightHandAnchor.transform.position, rightHandAnchor.transform.rotation);
            }
        }
        if (OVRInput.GetDown(OVRInput.RawButton.Y)) {
            if (leftHandAnchor != null) {
                Instantiate(ballPrefab, leftHandAnchor.transform.position, leftHandAnchor.transform.rotation);
            }
        }
		
		if (OVRInput.GetDown(OVRInput.RawButton.A)) {
			if (rightHandAnchor != null) {
				RaycastHit[] hits = Physics.SphereCastAll(rightHandAnchor.transform.position, threshold, rightHandAnchor.transform.forward, 0f);
				foreach (RaycastHit ball in hits){
					nearBall = ball.transform.gameObject;
					Destroy(nearBall);
				}
			}
		}
	}
}

