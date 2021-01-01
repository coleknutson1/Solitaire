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
	public Stack<GameObject> deck;
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
			if(col.GetComponentInChildren<PlayingCard>() != null)
			{
				var numberOfColumns = col.GetComponentsInChildren<PlayingCard>()?.Length != null ? col.GetComponentsInChildren<PlayingCard>()?.Length : 0;
				playingCardList.Add(col.GetComponentInChildren<PlayingCard>().gameObject);
				Debug.Log(numberOfColumns);
			}
		}
		return playingCardList;
	}

	internal GameObject CheckOverlap()
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
		Debug.Log(closestCardDist);
		if(closestCardDist<1.5f)
			return closestCard;
		return null;
	}

	private void InitializeDeck()
	{
		Shuffle(ref deckPrefabs);
		deck = new Stack<GameObject>(deckPrefabs.ToList());
		var columnIndex = 1;
		foreach (var column in columns)
		{
			var numberInStack = 0;
			for (var cardColumnCount = 0; cardColumnCount < columnIndex+1; cardColumnCount++)
			{
				var newCard = Instantiate(deck.Pop(),column.transform.position + new Vector3(0, numberInStack * -.1f,0), Quaternion.identity,column.transform);
				newCard.layer = numberInStack + 8;
				newCard.GetComponent<SpriteRenderer>().sortingLayerName = (numberInStack+1).ToString().TrimStart(new Char[] { '0' });
				newCard.GetComponent<PlayingCard>().SetLayer(numberInStack);
				numberInStack++;
				if(cardColumnCount != columnIndex)
				{
					newCard.GetComponent<PlayingCard>().FlipCard();
				}
			}
			columnIndex++;
		}
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
