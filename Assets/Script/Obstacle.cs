using UnityEngine;
using System.Collections;


public enum Mode {Horizontal, Vertical, Diagonal, DiagonalInvert };

public class Obstacle : MonoBehaviour {
	public Mode mode = Mode.Horizontal;
	public float offset;
	public float speed;
	int direction = 1;
	Vector3 center;
	public float damagePerSecond = 1;
	Vector3 startPosition;

	// Use this for initialization
	void Start () {
		center = transform.position;
		startPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (!transform.parent.parent.GetComponentInParent<GA> ().isDone) {
			Vector3 target;
			if (mode == Mode.Horizontal) {
				target = new Vector3 (center.x + (offset * direction), center.y, center.z);
			} else if (mode == Mode.Diagonal) {
				target = new Vector3 (center.x + (offset * direction), center.y, center.z + (offset * direction));
			} else if (mode == Mode.DiagonalInvert) {
				target = new Vector3 (center.x + (offset * direction), center.y, center.z - (offset * direction));
			} else {
				target = new Vector3 (center.x, center.y, center.z + (offset * direction));
			}
			if (Vector3.Distance (transform.position, target) <= 0.1) {
				direction *= -1;
			}
			transform.position = Vector3.MoveTowards (transform.position, target, speed * Time.deltaTime * transform.parent.parent.parent.GetComponent<GA> ().global_speed);
		}
	}

	void OnTriggerStay(Collider other) {
		if (other.name == "Robery") {
			other.GetComponent<Robery> ().GetDamage (Time.deltaTime * damagePerSecond);
		}
	}

	public void Reset() {
		transform.position = startPosition;
		direction = 1;
	}
}
