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
		int layerMask = 1 << 2;
		layerMask = ~layerMask;

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
					// Debug.Log("Text: BALLS" + ball);
					nearBall = ball.transform.gameObject;
					Debug.Log("GONNA DESTROY: " + nearBall.tag + " " + nearBall);
					Destroy(nearBall);
					// if (nearBall.tag == "BALL") {
						// Destroy(nearBall);
						// if (ball.collider != null ) {
							// Debug.Log("Text: AFTER" + nearBall.tag);
						// Destroy(nearBall.GetComponent<MeshRenderer>());
							// Destroy(ball.rigidbody);
							// Destroy(ball.transform.gameObject);
						// }
					// }
				}
			}
		}
	}
}

