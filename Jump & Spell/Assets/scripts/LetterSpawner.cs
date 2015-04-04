using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class LetterSpawner : MonoBehaviour
{
	private GameObject[] spawnPoints; // Used to mark the locations in the scene where letters spawn
	private List<GameObject> availableSpawnPoints; // Used to hold all spawn points which are available for use
	private List<GameObject> spawns;	// Used to hold all spawned letter instances
	public Sprite[] sprites;	// Used to reference sprites for letter spawns
	public GameObject letterPrefab;	// The prefab referenced for spawning new letters

	public List<GameObject> Spawns
	{
		get { return spawns; }
	}

	void Awake()
	{
		spawnPoints = GameObject.FindGameObjectsWithTag("LetterSpawnPoint");
		availableSpawnPoints = spawnPoints.ToList<GameObject>();
		spawns = new List<GameObject>();
	}

	public void ClearAllLetters()
	{
		// Clear all existing letters
		for (int i = 0; i < spawns.Count; ++i)
		{
			Destroy(spawns[i]);
		}
		spawns.Clear();
		availableSpawnPoints = spawnPoints.ToList<GameObject>();
	}

	public void SpawnNewLetters(string goal)
	{
		// Error checking
		if (goal.Length > availableSpawnPoints.Count)
			Debug.Log(string.Format("Insufficient available spawn points. There are {0} letter spawns to fit into {1} spawn points", goal.Length, availableSpawnPoints.Count));

		// Determine new letters needed
		foreach(char c in goal)
		{
			// Instantiate new letter prefab (list for future reference)
			spawns.Add(Instantiate<GameObject>(letterPrefab));
			GameObject justAdded = spawns[spawns.Count - 1];

			// Spawn randomly
			int index = Random.Range(0, availableSpawnPoints.Count);
			justAdded.transform.position = availableSpawnPoints[index].transform.position;
			availableSpawnPoints.RemoveAt(index);

			// Give it an identity
			justAdded.GetComponent<PickUpLetter>().letter = c;
			for (int i = 0; i < sprites.Length; ++i )
				if(sprites[i].name.Equals(c.ToString()))
				{
					justAdded.GetComponentInChildren<SpriteRenderer>().sprite = sprites[i];
					break;
				}
		}

		// Fill the rest with random letters
		while(availableSpawnPoints.Count > 0)
		{
			spawns.Add(Instantiate<GameObject>(letterPrefab));
			GameObject justAdded = spawns[spawns.Count - 1];
			justAdded.transform.position = availableSpawnPoints[0].transform.position;
			availableSpawnPoints.RemoveAt(0);

			// Set it to a random character
			int index = Random.Range(0, sprites.Length);
			Sprite chosenSprite = sprites[index];
			justAdded.GetComponent<PickUpLetter>().letter = chosenSprite.name[0];
			justAdded.GetComponentInChildren<SpriteRenderer>().sprite = chosenSprite;
		}
	}
}
