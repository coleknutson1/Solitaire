using UnityEngine;

public enum SuitColor { BLACK, RED }
public enum Suit { CLUB, SPADE, DIAMOND, HEART }
public enum Rank { ACE = 1, TWO = 2, THREE = 3, FOUR = 4, FIVE = 5, SIX = 6, SEVEN = 7, EIGHT = 8, NINE = 9, TEN = 10, JACK = 11, QUEEN = 12, KING = 13 }
public class PlayingCard : MonoBehaviour
{
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

	public void Awake()
	{
		isFaceUp = true;
		cardFront = GetComponent<SpriteRenderer>().sprite;
	}

	public void OnMouseDown()
	{
		GameManager.Instance.selectedCard = gameObject;
		if (!GameManager.Instance.selectedCard.GetComponent<PlayingCard>().isFaceUp)
		{
			GameManager.Instance.selectedCard.GetComponent<PlayingCard>().FlipCard();
			return;
		}
		holdSortingOrder = GameManager.Instance.selectedCard.GetComponent<SpriteRenderer>().sortingOrder;
		GameManager.Instance.selectedCard.GetComponent<SpriteRenderer>().sortingOrder = 100;
		holdPosition = gameObject.transform.position;

		offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
		isDragging = true;
	}

	public void OnMouseUp()
	{
		isDragging = false;
		var closestObject = CheckOverlap();
		if (closestObject == null)
		{
			GameManager.Instance.selectedCard.GetComponent<SpriteRenderer>().sortingOrder = holdSortingOrder;
			transform.position = holdPosition;
			return;
		}
		var closestObjectPlayingCard = closestObject.GetComponent<PlayingCard>();
		var currentPlayingCard = GetComponent<PlayingCard>();

		//If it's a valid lay, reparent current to new column
		if (currentPlayingCard.suitColor != closestObjectPlayingCard.suitColor)
		{
			//Parent to lowest faceup card
			transform.parent = closestObject.transform;
			transform.localPosition = new Vector3(0f, closestObjectPlayingCard.transform.parent.transform.childCount * -.6f, 0f);
			GameManager.Instance.selectedCard.GetComponent<SpriteRenderer>().sortingOrder = closestObjectPlayingCard.transform.parent.transform.childCount;

		}
		else
		{
			GameManager.Instance.selectedCard.GetComponent<SpriteRenderer>().sortingOrder = 100;
			transform.position = holdPosition;
		}
		//GameManager.Instance.RecheckColumns();
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

	internal GameObject CheckOverlap()
	{
		var faceupCards = GameManager.Instance.GetFaceupCards();
		GameObject closestCard = null;
		float closestCardDist = 1000f;

		foreach (var card in faceupCards)
		{
			if (card != GameManager.Instance.selectedCard)
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
}
