using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LetterInflater : MonoBehaviour
{
	private List<GameObject> spawns;	// List of letter instances
	private Vector3 regScale;	// Regular scale of letter instance
	private Vector2 regSize;	// Regular size of collider
	private bool isInflated;	// Indicates if the letters are already inflated
	private float currInflationFactor;	// Holds the scaling factor of the letters for maintaining the scale during letter respawn

	void Start()
	{
		LetterSpawner spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<LetterSpawner>();
		spawns = spawner.Spawns;
		regScale = spawner.letterPrefab.transform.localScale;
		regSize = spawner.letterPrefab.GetComponent<BoxCollider2D>().size;
		isInflated = false;
		currInflationFactor = 0F;
	}

	/// <summary>
	/// Maintains inflation of letter sprites (to be used if letters have respawned while the letters were inflated).
	/// </summary>
	public void MaintainLetterSpritesInflation()
	{
		if(isInflated && currInflationFactor != 0F)
		{
			for (int i = 0; i < spawns.Count; ++i)
			{
				if (spawns[i].activeInHierarchy)
				{
					Vector3 scale = spawns[i].transform.localScale;
					spawns[i].transform.localScale = new Vector3(
						scale.x * currInflationFactor,
						scale.y * currInflationFactor,
						scale.z);

					BoxCollider2D col = spawns[i].GetComponent<BoxCollider2D>();
					col.size = new Vector2(
						col.size.x / currInflationFactor,
						col.size.y / currInflationFactor);
				}
			}
		}
	}

	/// <summary>
	/// Inflates all letter sprites by the input factor but keeps their colliders unchanged. Only does this if letters are currently uninflated.
	/// </summary>
	/// <param name="inflationFactor">The scaling factor to inflate by</param>
	public void InflateLetterSprites(float inflationFactor)
	{
		if (!isInflated)
		{
			for (int i = 0; i < spawns.Count; ++i)
			{
				if (spawns[i].activeInHierarchy)
				{
					Vector3 scale = spawns[i].transform.localScale;
					spawns[i].transform.localScale = new Vector3(
						scale.x * inflationFactor,
						scale.y * inflationFactor,
						scale.z);

					BoxCollider2D col = spawns[i].GetComponent<BoxCollider2D>();
					col.size = new Vector2(
						col.size.x / inflationFactor,
						col.size.y / inflationFactor);
				}
			}

			currInflationFactor = inflationFactor;
			isInflated = true;
		}
	}

	/// <summary>
	/// Deflates all letter sprites if they were previously inflated.
	/// </summary>
	public void DeflateLetterSprites()
	{
		if (isInflated)
		{
			for (int i = 0; i < spawns.Count; ++i)
			{
				if (spawns[i].activeInHierarchy)
				{
					spawns[i].transform.localScale = regScale;
					spawns[i].GetComponent<BoxCollider2D>().size = regSize;
				}
			}

			currInflationFactor = 0F;
			isInflated = false;
		}
	}
}