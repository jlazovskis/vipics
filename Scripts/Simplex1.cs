using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1-dimensional (cylinder, or edge) class
public class Simplex1 : MonoBehaviour {
	
    public GameObject cylinderPrefab;
	
	private int index;
	private GameObject simplex;
	private float length;
	private List<Simplex0> neighbors0 = new List<Simplex0>();

	public Simplex1(GameObject ballA, GameObject ballB, int num) {
		index = num;
		simplex = (GameObject)Instantiate(cylinderPrefab, (ballA.transform.position + ballB.transform.position)/2, Quaternion.FromToRotation(Vector3.up, ballA.transform.position - ballB.transform.position));
		simplex.transform.localScale = new Vector3(0.015f, Vector3.Distance(ballA.transform.position/2, ballB.transform.position/2), 0.015f);
		simplex.name = "cylinder" + num.ToString();
		//neighbors0 = BallList;
	}		
}