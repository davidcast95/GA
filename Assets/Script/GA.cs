using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GA : MonoBehaviour {


	public float global_speed = 1;
	public int iteration = 0;
	public float global_maxHealth;
//	public float batch_maxHealth;
	List<Chromosome> childs;
	Chromosome[] grandparents,parents;
	Robery[] robers;
	float[] grandparenthealts,healths;
	Chromosome bestGen;

	public float minWait, maxWait;
	public float mutationRate;

//	public int i_batch = 6,n_batch;
//	public bool isBatch = false;
	int n;
	float time;
	public bool isDone;


	void Start () {
		n = transform.childCount;
		childs = new List<Chromosome> ();
		grandparents = new Chromosome[n];
		parents = new Chromosome[n/2];
		robers = new Robery[n];
		healths = new float[n];
		grandparenthealts = new float[n];

		//get all robbers
		for (int i = 0; i < n; i++) {
			robers [i] = transform.GetChild (i).FindChild ("Robery").GetComponent<Robery> ();
		}
	}

	void Update () {

		//check all robbers if it has reached goal
		isDone = true;
		for (int i = 0; i < n; i++) {
			Robery rober = transform.GetChild(i).FindChild ("Robery").GetComponent<Robery>();
			if (!rober.isDone) {
				isDone = false;
				time = 0;
				break;
			}
		}

	
		if (isDone) {

			//get global_maxHealth
			MaxHealth ();
			//first iteration
			if (iteration == 0) {
				Debug.Log ("Generation " + iteration);
				bool pass = true;
				//selection
				for (int i = 0; i < n; i++) {
					Robery rober = transform.GetChild (i).FindChild ("Robery").GetComponent<Robery> ();
					Debug.Log ("Parent " + i);
					rober.chromosome.Print ();
					grandparents[i] = rober.chromosome;
					grandparenthealts [i] = rober.GetFitness ();
					Debug.Log ("Fitness : " + transform.GetChild (i).FindChild ("Robery").GetComponent<Robery> ().GetFitness ());
				}
				if (pass) {
					iteration++;
				}
			}
			if (iteration > 0) {
				Debug.Log ("===========================================================");
				Debug.Log ("Generation " + iteration);


				//store healths
				for (int i = 0; i < n; i++) {
					healths [i] = robers [i].GetFitness ();
				}

				//find third best in parents and childs
				float tempMax = robers[0].GetFitness();
				for (int i = 0; i < 3; i++) {
					bool isBiggerThanTempMax = false;
					for (int j = 0; j < n; j++) {
						if (grandparenthealts [j] > tempMax) {
							isBiggerThanTempMax = true;
							tempMax = grandparenthealts[j];
							grandparenthealts [j] = 0;
							parents [i] = grandparents [j];
						}
					}
					for (int j = 0; j < n; j++) {
						if (healths [j] > tempMax) {
							isBiggerThanTempMax = true;
							tempMax = healths[j];
							healths [j] = 0;
							parents [i] = childs [j];
						}
					}

					if (!isBiggerThanTempMax) {
						parents [i] = robers [0].chromosome;
						parents [i].Print ();
					}
				}

				Debug.Log ("Parent mating");
				for (int i = 0; i < n/2; i++) {
					parents [i].Print ();
					Debug.Log (healths [i]);
				}

				//reset child
				childs.RemoveRange (0, childs.Count);
				//create child
				//old method
//					for (int i = 0; i < n; i++) {
//						for (int j = 0; j < n; j++) {
//							if (i != j) {
//								Chromosome child1 = new Chromosome (), child2 = new Chromosome ();
//								child1.Copy (robers[i].chromosome);
//								child2.Copy (robers [j].chromosome);
//								child1.Crossover (ref child2);
//								child1.Mutate ();
//								child2.Mutate ();
//								childs.Add (child1);
//								childs.Add (child2);
//							}
//						}
//					}

				if (Random.Range (0, 10000) < mutationRate * 10000) {
					parents [2] = bestGen;
				}

				//new method
				for (int i = 0; i < 3; i++) {
					for (int j = 0; j < 3; j++) {
						if (i != j) {
							Chromosome child1 = new Chromosome (), child2 = new Chromosome ();
							child1.Copy (parents[i]);
							child2.Copy (parents [j]);
							child1.Crossover (ref child2);
							child1.Mutate ();
							child2.Mutate ();
							childs.Add (child1);
							childs.Add (child2);
						}
					}
				}


				//attach to robbers
				for (int i = 0; i < n; i++) {
					transform.GetChild (i).FindChild ("Robery").GetComponent<Robery> ().chromosome = childs [i];
					//reset
					transform.GetChild (i).FindChild ("Robery").GetComponent<Robery> ().Reset ();
				}
				FieldReset ();


				//save the grandparent
//					for (int i = 0; i < n; i++) {
//						grandparents [i] = robers [i].chromosome;
//						grandparenthealts [i] = robers [i].GetFitness ();
//					}

//					//batch enter
//					isBatch = true;
//					i_batch = 0;
			}
//			if (isBatch) {
////				
//				if (i_batch == 1)
//
////				n_batch = n;
//				Debug.Log ("-------------------------------------------------------------------");
//				Debug.Log ("Batch " + i_batch);
////
////				//attach chromosome to robbers per batch
//				for (int i = 0; i < n; i++) {
//					transform.GetChild (i).FindChild ("Robery").GetComponent<Robery> ().chromosome = childs [i];
//					Debug.Log ("Child " + (i));
//					transform.GetChild (i).FindChild ("Robery").GetComponent<Robery> ().chromosome.Print ();
//				}
//
////				//store the best child in batch to parents
////				parents [i_batch] = childs[FindBestInBatch (i_batch) + (i_batch * 6)];
////				healths [i_batch] = batch_maxHealth;
////				Debug.Log ("Result from batch : " + i_batch);
////				Debug.Log (parents [i_batch]);
////
////
////				i_batch++;
////				if (i_batch + 1 == n_batch) {
////					//selection by fitness function
////					Debug.Log ("Selection by fitness function, max healt = " + global_maxHealth);
////					isBatch = false;
////
////					int count = 0;
////					//evaluate the parent
////					for (int i = 0; i < n && count < 6; i++) {
////						if (grandparenthealts [i] >= global_maxHealth) {
////							transform.GetChild (count).FindChild ("Robery").GetComponent<Robery> ().chromosome = grandparents [i];
////							count++;
////						}
////					}
////
////					//evalute the children in batch
////					for (int i = 0; i < n && count < 6; i++) {
////						//check health, if 0 die
////						if (healths [i] >= global_maxHealth) {
////							Debug.Log ("Parent " + i + " pass the evaluation");
////							transform.GetChild (count).FindChild ("Robery").GetComponent<Robery> ().chromosome = parents [i];
////							count++;
////						} 
////					}
////
////					for (int i = count; i < count; i++) {
////						Debug.Log ("Parent " + i + " recreate");
////						transform.GetChild (i).FindChild ("Robery").GetComponent<Robery> ().Recreate ();
////					}
////					iteration++;
////				}
//			}

		}
	}

	void MaxHealth() {
		float last_max = global_maxHealth;
		if (global_maxHealth == null) {
			global_maxHealth = robers [0].health;
			bestGen = robers [0].chromosome;
		} else if (global_maxHealth < robers [0].health) {
			global_maxHealth = robers [0].health;
			bestGen = robers [0].chromosome;
		}
		for (int i = 1; i < transform.childCount; i++) {
			if (robers [i].health > global_maxHealth) {
				global_maxHealth = robers [i].health;
				bestGen = robers [i].chromosome;
			}
		}
		if (last_max != 0 && last_max != global_maxHealth) {
			mutationRate += (global_maxHealth - last_max) / 100;
		}
	}
		
//	int FindBestInBatch(int batch) {
//		batch_maxHealth = robers [0].health;
//		int max = 0;
//		for (int i = 1; i < transform.childCount; i++) {
//			if (robers [i].health > batch_maxHealth) {
//				batch_maxHealth = robers [i].health;
//				max = i;
//			}
//		}
//		return max;
//	}

	void FieldReset() {
		for (int i = 0; i < n; i++) {
			Transform obstacles = transform.GetChild (i).FindChild ("Obstacle");
			for (int j = 0; j < obstacles.childCount; j++) {
				obstacles.GetChild (j).GetComponent<Obstacle> ().Reset ();
			}
		}
	}


}

