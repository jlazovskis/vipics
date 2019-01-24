using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBall : MonoBehaviour {
    public Transform leftHandAnchor;
    public Transform rightHandAnchor;
	
    public float threshold = 0.03f;
    float distance;
	
	// Update is called once per frame
	void Update () {
		if (OVRInput.GetDown(OVRInput.RawButton.A)) {
            distance = Vector3.Distance(rightHandAnchor.transform.position, this.transform.position);
            if (distance <= threshold) {
                Destruction();
            }
        }
	}
	
	void Destruction() {
        Destroy(this.gameObject);
    }
}
