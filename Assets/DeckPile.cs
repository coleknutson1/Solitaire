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
	int currentDiscard = 1;
	public void Start()
	{
		deckObject = gameObject;
		showCard = GameObject.Find("ShowCard");
	}
	public void OnMouseDown()
	{

		
		if (GameManager.Instance.deck.Count < 1)
		{ 			
			deckObject.GetComponent<SpriteRenderer>().enabled = true;
			while (discardPile.Count != 0)
			{
				var nullFixCheck = discardPile.Pop();
				if (nullFixCheck != null)
				{
					GameManager.Instance.deck.Push(nullFixCheck);
				}
			}
			discardPile = new Stack<GameObject>();
			return;
		}
		discardPile.Push(newCard);
		newCard = GameManager.Instance.deck.Pop();
		newCard.layer = currentDiscard;
		newCard.GetComponent<SpriteRenderer>().sortingOrder = currentDiscard;
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
