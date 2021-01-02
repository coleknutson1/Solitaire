using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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

	public List<GameObject> deckPrefabs = new List<GameObject>();
	public List<GameObject> columns = new List<GameObject>();
	public List<GameObject> feeds = new List<GameObject>();

	// Start is called before the first frame update
	void Start()
	{
		DeckPile.Instance.InitializeDeck(deckPrefabs, columns);
		Screen.fullScreen = true;
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	public List<GameObject> GetFaceupCards()
	{
		List<GameObject> playingCardList = new List<GameObject>();
		foreach (var col in columns)
		{
			if (col.transform.childCount > 0)
			{
				var youngestChild = col.transform.GetChild(0);
				RecursivelyGetYoungestChild(col.transform, ref youngestChild);
				playingCardList.Add(youngestChild.gameObject);
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
			else//base case
			{
				childest = child.transform;
			}
		}
	}
}
