using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour {
	public float speed = default(int);
	[HideInInspector]
	public Vector3 from;
	[HideInInspector]
	public Vector3 to;
	private float t;


	// Use this for initialization
	void Start () {
		from = transform.position - transform.right*4;
		to = transform.position + transform.right*4;
		t = 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
		t += Time.deltaTime*speed;
		if (t >= 1) {
			t -= 1;
			var temp = from;
			from = to;
			to = temp;
		}
		transform.position = Vector3.Lerp (from, to, t);
	}
}
