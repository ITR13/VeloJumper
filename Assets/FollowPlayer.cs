using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {
	public GameObject player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (player.transform.position.y - 12 > transform.position.y) {
			transform.position = new Vector3 (transform.position.x, player.transform.position.y - 12, transform.position.z);
		}
	}
}
