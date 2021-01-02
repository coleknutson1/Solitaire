using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	#region Singleton
	private static GameManager _instance;

	public static GameManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<GameManager>();
			}

			return _instance;
		}
	}

	void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

	#endregion

	#region Public Variables
	public List<GameObject> deckPrefabs = new List<GameObject>();
	public List<GameObject> columns = new List<GameObject>();
	public List<GameObject> feeds = new List<GameObject>();
	#endregion

	#region Start/Update
	void Start()
	{
		DeckPile.Instance.InitializeDeck(deckPrefabs, columns);
		Screen.fullScreen = true;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}
	#endregion

	public List<GameObject> GetFaceupCards()
	{
		List<GameObject> playingCardList = new List<GameObject>();
		foreach (var col in columns)
		{
			if(col.transform.childCount > 0)
			{
				var youngestChild = col.transform.GetChild(0);
				RecursivelyGetYoungestChild(col.transform, ref youngestChild);
				playingCardList.Add(youngestChild.gameObject);
			}
			else
			{
				playingCardList.Add(col);
			}
		}
		foreach (var feed in feeds)
		{
			if (feed.transform.childCount > 0)
			{
				var youngestChild = feed.transform.GetChild(0);
				RecursivelyGetYoungestChild(feed.transform, ref youngestChild);
				playingCardList.Add(youngestChild.gameObject);
			}
			else
			{
				playingCardList.Add(feed.gameObject);
			}
		}
		return playingCardList;
	}

	public void RecursivelyGetYoungestChild(Transform trans, ref Transform childest)
	{
		foreach (Transform child in trans)
		{
			if (child.childCount > 0)
			{
				RecursivelyGetYoungestChild(child, ref childest);
			}
			else //base case
			{
				childest = child.transform;
			}
		}
	}
}
