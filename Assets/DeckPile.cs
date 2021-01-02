using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckPile : MonoBehaviour
{
	private static DeckPile _instance;
	public Stack<GameObject> deck;

	public static DeckPile Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<DeckPile>();
			}

			return _instance;
		}
	}

	void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

	static System.Random _random = new System.Random();
	Stack<GameObject> discardPile = new Stack<GameObject>();
	GameObject showCard;
	GameObject newCard;
	GameObject deckObject;
	int currentDiscard = 1;

	public void Start()
	{
		deckObject = gameObject;
		showCard = GameObject.Find("ShowCard");
	}
	public void OnMouseDown()
	{
		if (deck.Count < 1)
		{
			currentDiscard = 0;
			deckObject.GetComponent<SpriteRenderer>().enabled = true;
			while (discardPile.Count != 0)
			{
				var nullFixCheck = discardPile.Pop();
				if (nullFixCheck != null)
				{
					deck.Push(nullFixCheck);
				}
			}
			foreach(Transform child in showCard)
			{
				Destroy(child);
			}
			discardPile = new Stack<GameObject>();
			return;
		}
		discardPile.Push(newCard);
		newCard = deck.Pop();
		newCard.layer = currentDiscard;
		newCard.GetComponent<SpriteRenderer>().sortingOrder = currentDiscard;
		currentDiscard++;
		Instantiate(newCard, showCard.transform.position, Quaternion.identity, showCard.transform);
		if (deck.Count < 1)
		{
			deckObject.GetComponent<SpriteRenderer>().enabled = false;
		}
	}

	public void InitializeDeck(List<GameObject> deckPrefabs, List<GameObject> columns)
	{
		Shuffle(ref deckPrefabs);
		deck = new Stack<GameObject>(deckPrefabs.ToList());
		var columnIndex = 1;
		foreach (var column in columns)
		{
			var numberInStack = 0;
			for (var cardColumnCount = 0; cardColumnCount < columnIndex + 1; cardColumnCount++)
			{
				var newCard = Instantiate(deck.Pop(), column.transform.position + new Vector3(0, numberInStack * -.1f, 0), Quaternion.identity, column.transform);
				newCard.GetComponent<SpriteRenderer>().sortingOrder = numberInStack + 1;
				newCard.layer = numberInStack;
				numberInStack++;
				if (cardColumnCount != columnIndex)
				{
					newCard.GetComponent<PlayingCard>().FlipCard();
					newCard.GetComponent<Collider2D>().enabled = false;
				}
			}
			columnIndex++;
		}
		//RecheckColumns();
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
