using UnityEngine;
using System.Collections;

public class Walkpoints : MonoBehaviour {
	public int walkpointPerRoom;
	public Transform rooms;
	public Transform walkpoint;
	// Use this for initialization
	void Start () {
		Create ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	Vector3 RandomInRoom(Transform room) {
		float x = room.position.x;
		float y = room.position.y;
		float z = room.position.z;

		float intervalX = room.localScale.x/2;
		float intervalZ = room.localScale.z/2;

		Vector3 randomPosition = new Vector3 (Random.Range (x - intervalX, x + intervalX), y, Random.Range (z - intervalZ, z + intervalZ));
		return randomPosition ;
	}

	public void Create() {
		int i = 0;

		for (int j = 0; j < transform.childCount; j++) {
			Destroy (transform.GetChild (j).gameObject);
		}

		while (i < rooms.childCount) {
			Transform room = rooms.GetChild (i);
			Transform result;
			if (room.name == "Objective" || room.name == "Goal") {
				walkpoint.transform.position = room.position;
				walkpoint.name = "Walkpoint";
				result = Instantiate (walkpoint).transform;
				result.SetParent (transform);
				i++;
			} else {
				for (int j = 0; j < walkpointPerRoom; j++) {
					walkpoint.transform.position = RandomInRoom (room);
					walkpoint.name = "Walkpoint";
					result = Instantiate (walkpoint).transform;
					result.SetParent (transform);
				}
				i++;
			}
		}
	}
}
