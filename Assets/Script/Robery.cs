using UnityEngine;
using System.Collections;

public class Robery : MonoBehaviour {
	public Chromosome chromosome;
	NavMeshAgent agent;
	Transform walkpoints;
	Transform information;
	int indexWalkpoint = 0;
	int n;
	public float health = 100;
	public float walk = 2F, run = 3.5F;
	Transform healthText;
	bool isWaiting = true;
	public bool isDone = false;
	Vector3 startPosition;


	// Use this for initialization
	void Start () {

		startPosition = transform.position;
		//chromosome
		chromosome = new Chromosome();

		//walkpoints
		agent = GetComponent<NavMeshAgent> ();
		walkpoints = transform.parent.FindChild ("Walkpoints");

		n = walkpoints.childCount;
		Debug.Log (n);
		CreateChromosome ();
		StartCoroutine(TransitionState (chromosome.alels [indexWalkpoint]));

		//health info
		healthText = transform.FindChild("Health");
		healthText.GetComponent<TextMesh> ().text = "Health : " + health;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (!isDone) {
			agent.speed = (chromosome.alels[indexWalkpoint].state == 0) ? walk * transform.parent.parent.GetComponent<GA>().global_speed : run * transform.parent.parent.GetComponent<GA>().global_speed;

			healthText.GetComponent<TextMesh> ().text = "Health : " + health;

			if (!isWaiting && agent.remainingDistance == 0 ) {
				if (indexWalkpoint + 1 < n) {
					StartCoroutine (TransitionState (chromosome.alels [indexWalkpoint]));
				} else {
					isDone = true;
					Debug.Log ("done");
				}
			}
		}

	}

	public void GetDamage(float damage) {
		health -= damage;
	}

	void AgentSpeedByState() {
		int state = chromosome.alels [indexWalkpoint].state;
	}

	IEnumerator TransitionState(Node alel) {
		isWaiting = true;
		yield return new WaitForSeconds (alel.time);
		isWaiting = false;
		agent.SetDestination (walkpoints.GetChild (indexWalkpoint).position);
		indexWalkpoint++;
	}

	public void Reset() {
		indexWalkpoint = 0;
		health = 100;
		transform.GetComponent<NavMeshAgent> ().enabled = false;
		transform.position = startPosition;
		transform.GetComponent<NavMeshAgent> ().enabled = true;
		isDone = false;
		StartCoroutine (TransitionState (chromosome.alels[indexWalkpoint]));
	}


	public float GetFitness() {
		if (health > 0) {
			return health;
		}
		return 0;
	}

	public void Recreate() {
		chromosome.Create (walkpoints.childCount,transform.parent.parent.GetComponent<GA>());
	}
	public void CreateChromosome() {
		chromosome.Create (walkpoints.childCount,transform.parent.parent.GetComponent<GA>());
	}
}
