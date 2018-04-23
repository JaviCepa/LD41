using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using DG.Tweening;

public class GameDirector : SerializedMonoBehaviour
{

	public Dictionary<SpawnType, GameObject> spawnPrefabs;

	ZombieSpawner[] zombieSpawners;
	HumanSpawner[] humanSpawners;
	CrateSpawner[] crateSpawners;

	public AudioClip pickupWeaponClip;
	public GameObject healthBarPrefab;
	public HumanBase humanBase;
	public HumanPlayer humanPlayer;

	public static GameDirector instance;
	public CanvasGroup titleCanvas;
	public CanvasGroup gameOverCanvasGroup;
	public Text loseText;
	public Text winText;
	public Text coinText;
	public Text dayText;

	public static int actorCount = 0;

	public int maxActorCount = 50;
	public float initialDelay = 5f;
	public float dayDuration = 120f;

	static int collectedCoins = 0;

	bool gameFinished = false;

	private void Awake()
	{
		instance = this;
		titleCanvas.alpha = 1f;
		zombieSpawners = FindObjectsOfType<ZombieSpawner>();
		humanSpawners = FindObjectsOfType<HumanSpawner>();
		crateSpawners = FindObjectsOfType<CrateSpawner>();
		humanBase = FindObjectOfType<HumanBase>();
		loseText.transform.localScale = Vector3.zero;
		winText.transform.localScale = Vector3.zero;
	}

	void Start()
	{
		StartCoroutine(RunningGame());
	}

	private void Update()
	{
		if (collectedCoins >= 100 && !gameFinished)
		{
			gameFinished = true;
			gameOverCanvasGroup.DOFade(1f, 0.5f);
			winText.transform.DOScale(1f, 0.5f);
		}
		if (humanPlayer == null && !gameFinished)
		{
			gameFinished = true;
			gameOverCanvasGroup.DOFade(1f, 0.5f);
			loseText.transform.DOScale(1f, 0.5f);
		}

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(0);
		}

	}

	private IEnumerator RunningGame()
	{
		Time.timeScale = 0f;
		yield return new WaitUntil(() => Input.anyKeyDown);
		Time.timeScale = 1f;

		yield return new WaitForSeconds(initialDelay);
		GetComponent<AudioSource>().Play();
		int currentLevel = 1;
		do
		{
			Debug.Log("New level " + currentLevel);
			dayText.text = "Day " + currentLevel;
			dayText.transform.DOPunchScale(Vector3.one*0.3f, 0.5f);
			int humanCount = Random.Range(1, 3);
			for (int i = 0; i < humanCount; i++)
			{
				AddActor(SpawnType.Human);
			}
			int crateCount = Random.Range(2, 3);
			for (int i = 0; i < crateCount; i++)
			{
				AddActor(SpawnType.CrateBox);
			}
			yield return new WaitForSeconds(dayDuration * 0.25f);
			if (currentLevel > 4)
			{
				for (int i = 0; i < currentLevel - 4; i++)
				{
					AddActor(SpawnType.HugeZombie);
				}
			}
			if (currentLevel > 2)
			{
				for (int i = 0; i < (currentLevel - 1) * 2; i++)
				{
					AddActor(SpawnType.FatZombie);
				}
			}
			for (int i = 0; i < currentLevel * 3 + 10; i++)
			{
				AddActor(SpawnType.CommonZombie);
			}
			yield return new WaitForSeconds(dayDuration * 0.75f);
			currentLevel++;
		} while (true);
	}

	public static void AddCoin()
	{
		collectedCoins++;
		instance.coinText.text = collectedCoins + " / 100";
	}

	void AddActor(SpawnType actorType)
	{
		Debug.Log("Added " + actorType.ToString());

		if (actorCount <= maxActorCount)
		{
			var position = GetSpawnPoint(actorType);
			Instantiate(spawnPrefabs[actorType], position, Quaternion.identity);
			actorCount++;
		}
	}

	public static void RemoveActor()
	{
		actorCount--;
	}

	private Vector3 GetSpawnPoint(SpawnType spawnType)
	{
		Vector3 result = Vector3.zero;
		if (spawnType.ToString().Contains("Zombie"))
		{
			result = zombieSpawners[Random.Range(0, zombieSpawners.Length)].GetSpawnPoint();
		}
		if (spawnType.ToString().Contains("Human"))
		{
			result = humanSpawners[Random.Range(0, humanSpawners.Length)].GetSpawnPoint();
		}
		if (spawnType.ToString().Contains("Crate"))
		{
			result = crateSpawners[Random.Range(0, crateSpawners.Length)].GetSpawnPoint();
		}
		return result;
	}
}

public enum SpawnType { None, Human, CommonZombie, FatZombie, HugeZombie, CrateBox }
