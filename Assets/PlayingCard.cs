﻿using UnityEngine;

public enum SuitColor { BLACK, RED }
public enum Suit { CLUB, SPADE, DIAMOND, HEART }
public enum Rank { FEED = 0, ACE = 1, TWO = 2, THREE = 3, FOUR = 4, FIVE = 5, SIX = 6, SEVEN = 7, EIGHT = 8, NINE = 9, TEN = 10, JACK = 11, QUEEN = 12, KING = 13, EMPTY_COLUMN = 99 }
public class PlayingCard : MonoBehaviour
{
	#region Variables
	public Sprite cardBack;
	public Sprite cardFront;
	public SuitColor suitColor;
	public Suit suit;
	public Rank rank;
	public bool isFaceUp;
	public bool isDragging;
	private Vector3 offset;
	private Vector3 holdPosition;
	private int holdSortingOrder;
	public GameObject selectedCard = null;
	bool cardFlipping = false;
	#endregion

	public void Awake()
	{
		isFaceUp = true;
		cardFront = GetComponent<SpriteRenderer>().sprite;
	}

	public void OnMouseDown()
	{
		cardFlipping = false;
		selectedCard = gameObject;
		holdPosition = gameObject.transform.position;
		holdSortingOrder = selectedCard.GetComponent<SpriteRenderer>().sortingOrder;
		if (!selectedCard.GetComponent<PlayingCard>().isFaceUp)
		{
			selectedCard.GetComponent<PlayingCard>().FlipCard();
			cardFlipping = true;
			return;
		}
		selectedCard.GetComponent<SpriteRenderer>().sortingOrder = 87;

		//If it has children, we want to take them with it
		if (selectedCard.transform.childCount > 0)
		{
			var sortingOrder = 87;
			var layerOrder = 1;
			RecursivelySetChildCount(selectedCard.transform, ref sortingOrder, ref layerOrder);
		}

		offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
		isDragging = true;
	}

	public void OnMouseUp()
	{
		if (cardFlipping) { return; }
		isDragging = false;
		var closestObject = CheckOverlap();
		if (closestObject == null)
		{
			selectedCard.GetComponent<SpriteRenderer>().sortingOrder = holdSortingOrder;
			transform.position = holdPosition;
			return;
		}
		//If our closest object is a feed, and our card is an ace, place it
		var closestObjectPlayingCard = closestObject.GetComponent<PlayingCard>();
		var currentPlayingCard = GetComponent<PlayingCard>();
		var isFromDeck = false;

		if(currentPlayingCard.transform.parent.name.Contains("ShowCard")) { isFromDeck = true; }
		Transform closestObjectPlayingCardParentCheck = closestObjectPlayingCard.transform;
		//If it's a valid lay, reparent current to new column
		if (closestObjectPlayingCard.rank == Rank.EMPTY_COLUMN || (currentPlayingCard.suitColor != closestObjectPlayingCard.suitColor && currentPlayingCard.rank == closestObjectPlayingCard.rank - 1))
		{
			//Parent to lowest faceup card
			transform.parent = closestObject.transform;
			if (closestObjectPlayingCard.rank == Rank.EMPTY_COLUMN)
			{
				transform.position = closestObject.transform.position;
			}
			else
			{
				transform.localPosition = new Vector3(0f, closestObjectPlayingCard.transform.parent.transform.childCount * -1.5f, 0f);
			}
			int childCount = 0;
			RecursivelyGetChildCount(closestObject.transform, ref childCount);
			selectedCard.GetComponent<SpriteRenderer>().sortingOrder = childCount + 1;
			if (isFromDeck)
			{
				DeckPile.Instance.discardPile.Pop();
			}
		}
		else if ((currentPlayingCard.rank == Rank.ACE && closestObjectPlayingCard.rank == Rank.FEED) ||
			(currentPlayingCard.suit == closestObjectPlayingCard.suit && currentPlayingCard.rank == closestObjectPlayingCard.rank + 1) && IsInFeed(ref closestObjectPlayingCardParentCheck))
		{
			int childCount = 0;
			transform.parent = closestObject.transform;
			transform.position = transform.parent.position;
			RecursivelyGetChildCount(transform, ref childCount);
			GetComponent<SpriteRenderer>().sortingOrder = childCount;
			if (isFromDeck)
			{
				DeckPile.Instance.discardPile.Pop();
			}
		}
		else
		{
			selectedCard.GetComponent<SpriteRenderer>().sortingOrder = holdSortingOrder;
			transform.position = holdPosition;
		}
		RecheckColumns();
	}

	private bool IsInFeed(ref Transform closestObjectPlayingCard)
	{
		if(closestObjectPlayingCard.name.Contains("Feed"))
		{
			return true;
		}
		if(closestObjectPlayingCard.transform.parent != null)
		{
			var parent = closestObjectPlayingCard.transform.parent;
			return IsInFeed(ref parent);
		}
		return false;
	}

	public float GetDistanceFromDraggingCard(Vector2 dCard)
	{
		return Vector2.Distance(dCard, (Vector2)transform.position);
	}

	void Update()
	{
		if (isDragging)
		{
			Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position - offset;
			transform.Translate(mousePosition);
		}
	}
	public void FlipCard()
	{
		isFaceUp = !isFaceUp;
		if (isFaceUp)
		{
			GetComponent<SpriteRenderer>().sprite = cardFront;
		}
		else
		{
			GetComponent<SpriteRenderer>().sprite = cardBack;
		}
	}

	void RecursivelyGetChildCount(Transform trans, ref int childCount)
	{
		if (trans.parent != null)
		{
			trans = trans.parent;
			childCount++;
			RecursivelyGetChildCount(trans, ref childCount);
		}
	}

	void RecursivelySetChildCount(Transform trans, ref int childCount, ref int layerCount)
	{
		if (trans.childCount > 0)
		{
			childCount++;
			if(trans.GetComponent<PlayingCard>().isFaceUp){
				trans.GetComponent<BoxCollider2D>().offset = new Vector2(0f, 2.762386f);
				trans.GetComponent<BoxCollider2D>().size = new Vector2(4.7f, 1.625227f);
			}

			trans = trans.GetChild(0);
			trans.GetComponent<SpriteRenderer>().sortingOrder = childCount;
			if(layerCount == 2) { layerCount++; }
			trans.gameObject.layer = LayerMask.NameToLayer(layerCount.ToString());
			layerCount++;
			RecursivelySetChildCount(trans, ref childCount, ref layerCount);
		}
	}

	internal GameObject CheckOverlap()
	{
		var faceupCards = GameManager.Instance.GetFaceupCards();
		GameObject closestCard = null;
		float closestCardDist = 1000f;

		foreach (var card in faceupCards)
		{
			if (card != selectedCard)
			{
				var distance = card.GetComponent<PlayingCard>().GetDistanceFromDraggingCard(Camera.main.ScreenToWorldPoint(Input.mousePosition));
				if (closestCard == null || (distance < closestCardDist && card.GetComponent<PlayingCard>().isFaceUp))
				{
					closestCard = card;
					closestCardDist = distance;
				}
			}
		}
		Debug.Log(closestCard + ":" + closestCardDist);
		if (closestCardDist < 2.5f)
			return closestCard;
		return null;
	}

	internal void RecheckColumns()
	{
		foreach (var column in GameManager.Instance.columns)
		{
			var childCount = 10;
			var layerCount = 1;
			if (column.transform.childCount < 1)
				continue;

			var youngestChild = column.transform.GetChild(0);
			GameManager.Instance.RecursivelyGetYoungestChild(youngestChild, ref youngestChild);
			youngestChild.GetComponent<BoxCollider2D>().enabled = true;
			RecursivelySetChildCount(column.transform, ref childCount, ref layerCount);
		}
	}
}
