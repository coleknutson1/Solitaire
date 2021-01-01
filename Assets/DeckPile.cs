using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckPile : MonoBehaviour
{
	Stack<GameObject> discardPile = new Stack<GameObject>();
	GameObject showCard;
	GameObject newCard;
	GameObject deckObject;
	public void Start()
	{
		deckObject = gameObject;
		showCard = GameObject.Find("ShowCard");
	}
	public void OnMouseDown()
	{
		while (discardPile.Count != 0)
		{
			var nullFixCheck = discardPile.Pop();
			if (nullFixCheck != null)
			{
				GameManager.Instance.deck.Push(nullFixCheck);
			}
		}
		if (GameManager.Instance.deck.Count < 1)
		{ 			
			deckObject.GetComponent<SpriteRenderer>().enabled = true;
			
			discardPile = new Stack<GameObject>();
			return;
		}
		discardPile.Push(newCard);
		newCard = GameManager.Instance.deck.Pop();
		newCard.layer = 10;
		Instantiate(newCard, showCard.transform.position, Quaternion.identity, showCard.transform);
		if (GameManager.Instance.deck.Count < 1)
		{
			deckObject.GetComponent<SpriteRenderer>().enabled = false;
		}
	}

	public void OnMouseUp()
	{
	}
}
