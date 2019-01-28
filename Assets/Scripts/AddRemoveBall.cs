using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRemoveBall : MonoBehaviour {
    public GameObject leftHandAnchor;
    public GameObject rightHandAnchor;
	public Transform leftHandAnchor;
    public Transform rightHandAnchor;
    public Transform ballPrefab;
	
	public float threshold = 0.03f;
    float distance;
	
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
			hits = Physics.SphereCastAll(rightHandAnchor.transform.position, threshold, rightHandAnchor.transform.forward, 0f);
			if (hits.Length > 0){
				for (int i = 0; i < hits.Length; i++){
					nearBall = hits[i].transform.GameObject;
					Destroy(nearBall);
				}
			}
		}
	}
}

