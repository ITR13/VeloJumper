using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
	public GameObject WinScreen;
	public GameObject LoseScreen;

	private Rigidbody2D rb = null;
	private MoveScript parent = null;
	private MoveScript grandparent = null;
	private float startx = 0;

	private int lives = 0;
	private Vector3 lastlockedlocpos = default(Vector3);

	private Collider2D _collider;

	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		startx = transform.position.x;
		lives = 3;
		_collider = GetComponent<Collider2D> ();
	}
	
	void Update () {
		if (!_collider.enabled) {
			_collider.enabled = true;
		}
		if(Input.GetKeyDown(KeyCode.Escape)){
			Application.Quit ();
			return;
		}

		if (rb.isKinematic) {
			if (Input.anyKeyDown) {
				lastlockedlocpos = transform.localPosition;
				rb.isKinematic = false;
				transform.parent = null;
				rb.velocity = (Vector2)(parent.to - parent.from) * parent.speed + Vector2.up * 9.5f;
				_collider.enabled = false;
			}
		} else {
			if (transform.position.x-startx < -6) {
				transform.position += Vector3.right*12;
			} else if (transform.position.x-startx > 6) {
				transform.position -= Vector3.right*12;
			}
		}
	}

	void OnTriggerStay2D(Collider2D other){
		if (WinScreen.activeInHierarchy || LoseScreen.activeInHierarchy) {
			return;
		}


		if (other.gameObject.name == "lose") {
			lives--;
			if (lives > 0) {
				StartCoroutine (BlinkForMe ());
				OnTriggerStay2D (parent.GetComponent<Collider2D> ());
				transform.localPosition = lastlockedlocpos;

				return;
			}

			foreach (GameObject g in FindObjectsOfType<GameObject>()) {
				if (g.transform.parent == null) {
					Destroy (g);
				}
			}
			this.enabled = false;
			LoseScreen.SetActive (true);
			return;
		}

		if (rb.velocity.y < -0.1f) {
			if (other.gameObject.name == "win") {
				foreach (GameObject g in FindObjectsOfType<GameObject>()) {
					if (g.transform.parent == null) {
						Destroy (g);
					}
				}
				this.enabled = false;
				WinScreen.SetActive (true);
				return;
			}

			rb.velocity = Vector2.zero;
			rb.isKinematic = true;

			if (grandparent != null && grandparent != other.GetComponent<MoveScript>()) {
				//Destroy (grandparent.gameObject);
			}
			grandparent = parent;
			parent = other.GetComponent<MoveScript> ();
			if (parent == grandparent) {
				grandparent = null;
			}

			if (transform != null && parent != null) {
				transform.parent = parent.transform;
			}
		}
	}


	public IEnumerator BlinkForMe(){
		var r = GetComponent<Renderer> ();
		for (int i = 0; i < 8; i++) {
			yield return new WaitForSeconds (0.25f);
			r.enabled = !r.enabled;
		}
		yield return new WaitForSeconds (0.25f);
		r.enabled = true;
	}
}
