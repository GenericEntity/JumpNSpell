using UnityEngine;
using System.Collections.Generic;
using System.Linq;

	/// <summary>
	/// Manages the operations that take place on letters.
	/// </summary>
	public class LetterManager_JnS : MonoBehaviour
	{
		private GameObject[] spawnPoints; // Used to mark the locations in the scene where letters spawn
		private List<GameObject> availableSpawnPoints; // Used to hold all spawn points which are available for use
		private List<GameObject> spawns;	// Used to hold all spawned letter instances
		public Sprite[] sprites;	// Used to reference sprites for letter spawns
		public GameObject letterPrefab;	// The prefab referenced for spawning new letters

		private Vector3 regScale;	// Regular scale of letter instance
		private Vector2 regSize;	// Regular size of collider
		private bool isInflated;	// Indicates if the letters are already inflated
		private float currInflationFactor;	// Holds the scaling factor of the letters for maintaining the scale during letter respawn

		private bool disablePickup;

		public List<GameObject> Spawns
		{
			get { return spawns; }
		}

		public int SpawnPointCount
		{
			get { return spawnPoints.Length; }
		}

		void Awake()
		{
			spawnPoints = GameObject.FindGameObjectsWithTag("LetterSpawnPoint");
			availableSpawnPoints = spawnPoints.ToList<GameObject>();
			spawns = new List<GameObject>();
		}

		void Start()
		{
			regScale = letterPrefab.transform.localScale;
			regSize = letterPrefab.GetComponent<BoxCollider2D>().size;
			isInflated = false;
			currInflationFactor = 0F;
			disablePickup = false;
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
			foreach (char c in goal)
			{
				// Instantiate new letter prefab (list for future reference)
				spawns.Add(Instantiate<GameObject>(letterPrefab));
				GameObject justAdded = spawns[spawns.Count - 1];

				// Spawn randomly
				int index = Random.Range(0, availableSpawnPoints.Count);
				justAdded.transform.position = availableSpawnPoints[index].transform.position;
				availableSpawnPoints.RemoveAt(index);

				// Give it an identity
				justAdded.GetComponent<LetterPickup_JnS>().Letter = c;
				for (int i = 0; i < sprites.Length; ++i)
					if (sprites[i].name.Equals(c.ToString()))
					{
						justAdded.GetComponent<SpriteRenderer>().sprite = sprites[i];
						break;
					}
			}

			// Fill the rest with random letters
			while (availableSpawnPoints.Count > 0)
			{
				spawns.Add(Instantiate<GameObject>(letterPrefab));
				GameObject justAdded = spawns[spawns.Count - 1];
				justAdded.transform.position = availableSpawnPoints[0].transform.position;
				availableSpawnPoints.RemoveAt(0);

				// Set it to a random character
				int index = Random.Range(0, sprites.Length);
				Sprite chosenSprite = sprites[index];
				justAdded.GetComponent<LetterPickup_JnS>().Letter = chosenSprite.name[0];
				justAdded.GetComponent<SpriteRenderer>().sprite = chosenSprite;
			}

			if (disablePickup)
				for (int i = 0; i < spawns.Count; ++i)
				{
					spawns[i].GetComponent<BoxCollider2D>().enabled = false;
				}

			// Maintains the scale size if the camera was zoomed out when respawning
			MaintainLetterSpritesInflation();
		}

		public void DisableLetterPickup()
		{
			disablePickup = true;
			for (int i = 0; i < spawns.Count; ++i)
			{
				spawns[i].GetComponent<BoxCollider2D>().enabled = false;
			}
		}

		/// <summary>
		/// Maintains inflation of letter sprites (to be used if letters have respawned while the letters were inflated).
		/// </summary>
		public void MaintainLetterSpritesInflation()
		{
			if (isInflated && currInflationFactor != 0F)
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
