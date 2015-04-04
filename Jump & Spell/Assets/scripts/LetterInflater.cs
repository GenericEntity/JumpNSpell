using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LetterInflater : MonoBehaviour
{
	private List<GameObject> spawns;
	private Vector3 regScale;

	void Start()
	{
		spawns = GameObject.FindGameObjectWithTag("Spawner").GetComponent<LetterSpawner>().Spawns;
		regScale = new Vector3(1, 1, 1);
	}

	public void InflateLetters(float scaleFactor)
	{
		for (int i = 0; i < spawns.Count; ++i)
		{
			if (spawns[i].activeInHierarchy)
			{
				Transform t = spawns[i].GetComponentInChildren<Transform>();
				Vector3 scale = t.localScale;
				t.localScale = new Vector3(scale.x * scaleFactor, scale.y * scaleFactor, scale.z);
			}
		}
	}

	public void DeflateLetters()
	{
		for (int i = 0; i < spawns.Count; ++i)
		{
			if (spawns[i].activeInHierarchy)
				spawns[i].GetComponentInChildren<Transform>().localScale = regScale;
		}
	}
}