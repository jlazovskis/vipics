using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 0-dimensional (ball, or vertex) class
public class Simplex0 : MonoBehaviour {
	
    public GameObject ballPrefab;
    public GameObject haloPrefab;	
	
	private int index;

	private GameObject simplex;
	private List<Simplex1> neighbors1 = new List<Simplex1>();

	private GameObject halo;
	private float r = 0.1f;
	
	public Simplex0(GameObject anchor, int num) {
		index = num;
		simplex = (GameObject)Instantiate(ballPrefab, anchor.transform.position, anchor.transform.rotation);
		simplex.name = "clone" + num.ToString();
		halo = (GameObject)Instantiate(haloPrefab, anchor.transform.position, anchor.transform.rotation);
		halo.name = "halo" + num.ToString();
		halo.transform.SetParent(simplex.transform);
		//neighbors1 = CylinderList;
	}
}