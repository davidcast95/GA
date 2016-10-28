using UnityEngine;
using System.Collections;


//walk = 0 run = 1

public class Node {
	public int state = 0;
	public float time = 0;

	public Node(int _state, float _time) {
		state = _state;
		time = _time;
	}
	public void Create(int a, float b)
	{
		state = a;
		time = b;
	}

	void SwapTime(Node a)
	{
		float z;
		z = time;
		time = a.time;
		a.time = z;
	}

	void SwapState(Node a)
	{
		int z;
		z = state;
		state = a.state;
		a.state = z;
	}

	public void Swap(Node a)
	{
		SwapTime(a);
		SwapState(a);
	}
}

public class Chromosome {
	public Node[] alels;
	int n;
	GA ga;
	public void Copy(Chromosome other) {
		n = other.n;
		alels = new Node[n];
		for (int i = 0; i < n; i++) {
			alels [i] = new Node (other.alels [i].state, other.alels [i].time);
		}
		ga = other.ga;
	}
	public void Create(int n,GA ga) {
		this.n = n;
		this.ga = ga;
		alels = new Node[n];
		for (int i = 0; i < n; i++) {
			alels [i] = new Node (Random.Range (0,2), Random.Range (ga.minWait, ga.maxWait * 1000) / 1000);

		}
	}

	public void Crossover(ref Chromosome other)
	{
		if (n % 2 == 1)
		{
			for (int a = 0; a < n - 1; a += 2)
			{
				alels[a].Swap(other.alels[a + 1]);
				other.alels[a].Swap(alels[a + 1]);
			}

			alels[n - 1].Swap(other.alels[n - 1]);
		}
		else
		{
			for (int a = 0; a < n; a += 2)
			{
				alels[a].Swap(other.alels[a + 1]);
				other.alels[a].Swap(alels[a + 1]);
			}
		}
	}

	public void Mutate() {
		for (int a = 0; a < n; a++)
		{
			int ra;
			ra = Random.Range(0,10000);
			if (ra < 100 * ga.mutationRate) {
				alels [a].state = (alels [a].state == 0) ? 1 : 0;
				alels [a].time = Random.Range (ga.maxWait, ga.maxWait * 1000) / 1000;
			}
		}
	}

	public void Print() {
		var str = "state (";
		for (int a = 0; a < n; a++)
		{
			str += alels[a].state + " ";
		}
		Debug.Log (str + ")");
		str = "time (";
		for (int a = 0; a < n; a++)
		{
			str += Mathf.Round(alels[a].time) + " ";
		}
		Debug.Log (str + ")");
	}


}
