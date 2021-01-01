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
	Stack<GameObject> deck;
	public List<GameObject> deckPrefabs = new List<GameObject>();
	public float scale = .3f;
	public List<GameObject> columns = new List<GameObject>();
	public GameObject selectedCard = null;
	static System.Random _random = new System.Random();

	internal void RecheckColumns()
	{
		throw new NotImplementedException();
	}

	// Start is called before the first frame update
	void Start()
	{
		InitializeDeck();
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
			playingCardList.Add(col.GetComponentInChildren<PlayingCard>().gameObject);
		}
		return playingCardList;
	}

	internal void CheckOverlap()
	{
		var faceupCards = GetFaceupCards();
		GameObject closestCard = null;
		float closestCardDist = 1000f;

		foreach (var card in faceupCards)
		{
			if (card != GameManager.Instance.selectedCard)
			{
				var distance = card.GetComponent<DragAndDrop>().GetDistanceFromDraggingCard(Camera.main.ScreenToWorldPoint(Input.mousePosition));
				if (closestCard == null || distance < closestCardDist)
				{
					closestCard = card;
					closestCardDist = distance;
				}
			}

		}
		Debug.Log($"{closestCard.name}...{closestCardDist}");
	}

	private void InitializeDeck()
	{
		Shuffle(ref deckPrefabs);
		deck = new Stack<GameObject>(deckPrefabs.ToList());
		var columnIndex = 1;
		foreach (var column in columns)
		{
			for (var cardColumnCount = 0; cardColumnCount < columnIndex; cardColumnCount++)
			{

			}
			InstantiateCard(column);
		}
	}

	private void InstantiateCard(GameObject parent)
	{
		var layThisDown = deck.Pop();
		var cardGameObject = Instantiate(layThisDown, parent.transform.position, Quaternion.identity, parent.transform);
		cardGameObject.layer = 8;
		cardGameObject.GetComponent<SpriteRenderer>().sortingLayerName = "FaceUp";
	}

	static void Shuffle(ref List<GameObject> array)
	{
		int n = array.Count;
		for (int i = 0; i < (n - 1); i++)
		{
			// Use Next on random instance with an argument.
			// ... The argument is an exclusive bound.
			//     So we will not go past the end of the array.
			int r = i + _random.Next(n - i);
			GameObject t = array[r];
			array[r] = array[i];
			array[i] = t;
		}
	}

}
