using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baller : MonoBehaviour {
    public GameObject leftHandAnchor;
    public GameObject rightHandAnchor;
    public GameObject ballPrefab;
	public float threshold = 0.03f;	
	private GameObject nearBall;
	private GameObject clone;
	private string name;
	static int num;
	
	// Update is called once per frame
	void Update () {
		int layerMask = 1 << 2;
		layerMask = ~layerMask;

        // Create
        if (OVRInput.GetDown(OVRInput.RawButton.B)) {
            if (rightHandAnchor != null) {
                GameObject clone = (GameObject)Instantiate(ballPrefab, rightHandAnchor.transform.position, rightHandAnchor.transform.rotation);
				clone.name = "clone" + num.ToString();
				num++;
				// BoxCollider bc = gameObject.AddComponent(typeof(BoxCollider)) as BoxCollider;
				// Debug.Log("FIND: " + clone.GetComponent<BoxCollider>());
            }
        }
        if (OVRInput.GetDown(OVRInput.RawButton.Y)) {
            if (leftHandAnchor != null) {
				GameObject clone = (GameObject)Instantiate(ballPrefab, leftHandAnchor.transform.position, leftHandAnchor.transform.rotation);
				clone.name = "clone" + num.ToString();
				num++;
            }
        }
		
		if (OVRInput.GetDown(OVRInput.RawButton.A)) {
			if (rightHandAnchor != null) {
				RaycastHit[] hits = Physics.SphereCastAll(rightHandAnchor.transform.position, threshold, rightHandAnchor.transform.forward, 0f);
				foreach (RaycastHit ball in hits){
					// Debug.Log("Text: BALLS" + ball);
					nearBall = ball.collider.gameObject;
					Debug.Log("GONNA DESTROY: " + ball.collider.name);
					// Debug.Log(GameObject.Find(nearBall.name));
					// name = ball.transform.name;
					// Destroy(GameObject.Find(ball.collider.name));
					// if (nearBall.tag == "BALL") {
						// Destroy(nearBall);
					// if (gameObject != null) {
						// Destroy(gameObject);
					// }
							// Debug.Log("Text: AFTER" + nearBall.tag);
					Destroy(nearBall.GetComponent<MeshRenderer>());
					// Destroy(ball.rigidbody);
							// Destroy(ball.transform.gameObject);
						// }
					// }
				}
			}
		}
		if (OVRInput.GetDown(OVRInput.RawButton.X)) {
			if (leftHandAnchor != null) {
				RaycastHit[] hits = Physics.SphereCastAll(leftHandAnchor.transform.position, threshold, leftHandAnchor.transform.forward, 0f);
				foreach (RaycastHit ball in hits){
					nearBall = ball.collider.gameObject;
					Debug.Log("GONNA DESTROY: " + ball.collider.name);
					Destroy(nearBall.GetComponent<MeshRenderer>());
				}
			}
		}
	}
}

