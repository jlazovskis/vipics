using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRemoveBalls : MonoBehaviour {
    public GameObject leftHandAnchor;
    public GameObject rightHandAnchor;
    public Transform ballPrefab;
	
	// Update is called once per frame
	void Update () {
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
    }
}
