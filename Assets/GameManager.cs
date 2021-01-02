﻿using System;
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
	public GameObject selectedCard = null;

	//Reevaluate the column after we have
	internal void RecheckColumns()
	{
		foreach (var column in columns)
		{
			Transform[] children = new Transform[column.transform.childCount];
			for (int i = 0; i < column.transform.childCount; i++)
			{
				children[i] = column.transform.GetChild(i);
			}
			children[column.transform.childCount - 1].GetComponent<Collider2D>().enabled = true;
		}
	}

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

	void RecursivelyGetYoungestChild(Transform trans, ref Transform childest)
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
